using Autofac;
using Autofac.Extensions.DependencyInjection;
using Coldairarrow.Business.Base_SysManage;
using Coldairarrow.DataRepository;
using Coldairarrow.Entity.Base_SysManage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Coldairarrow.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region Jwt验证

            string issuer = Configuration["Jwt:Issuer"];
            string audience = Configuration["Jwt:Audience"];
            string expire = Configuration["Jwt:ExpireMinutes"];
            TimeSpan expiration = TimeSpan.FromMinutes(Convert.ToDouble(expire));
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecurityKey"]));
            services.AddAuthorization(options =>  //1.添加基于策略授权的方法
            {
                //1、Definition authorization policy
                options.AddPolicy("Permission",
                   policy => policy.Requirements.Add(new PolicyRequirement()));
            }).AddAuthentication(s =>  //2.验证授权模式是否是Jwt bearer
            {
                //2、Authentication
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(s =>  //3.Jwt bearer token 信息验证
            {
                //3、Use Jwt bearer 
                s.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = key,
                    ClockSkew = expiration,
                    ValidateLifetime = true
                };
                s.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //Token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            #endregion

            #region 跨域配置
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //跨域配置
            services.AddCors(options =>
               options.AddPolicy("Standard", builder => {
                   builder.AllowAnyOrigin()
                  //builder.WithOrigins(new string[] { "http://localhost:8082", "http://localhost:51126" }) 
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials(); //允许Cookie信息
               })
            );
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                #region Jwt验证
                //添加授权
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "请输入带有Bearer开头的Token",
                    Name = "Authorization",//Jwt default param name
                    In = "header",//Jwt store address
                    Type = "apiKey"//Security scheme type
                });
                //认证方式，此方式为全局添加
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

                #endregion

                c.SwaggerDoc("通用", new Info { Version = "通用", Title = "通用" });
                c.SwaggerDoc("登陆API", new Info { Title = "登陆API", Version = "登陆API" });
                c.SwaggerDoc("权限API", new Info { Title = "权限API", Version = "权限API" });

                var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Coldairarrow.WebApi.xml");
                c.IncludeXmlComments(xmlPath, true);
                var xmlModelPath = Path.Combine(basePath, "Coldairarrow.Entity.xml");
                c.IncludeXmlComments(xmlModelPath);


                c.DocInclusionPredicate((docName, apiDesc) => {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
                        return false;
                    var versions = methodInfo.DeclaringType
                    .GetCustomAttributes(true)
                    .OfType<ApiExplorerSettingsAttribute>()
                    .Select(attr => attr.GroupName);
                    if (docName.ToLower() == "通用" && versions.FirstOrDefault() == null)
                        return true; //无ApiExplorerSettingsAttribute配置的将在通用中显示
                    return versions.Any(v => v.ToString() == docName);
                });
            });

            #endregion

            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<GlobalActionFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;//设置忽略值为null的属性,解决d2Admin问题
                options.SerializerSettings.ContractResolver = new DefaultContractResolver(); //不更改元数据的key的大小写
            });
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IJwtAppService, JwtAppService>(); //token处理
            services.AddTransient<IUserOperationLog, UserOperationLog>(); //用户操作日志
            services.AddSingleton(Configuration);
            services.AddSingleton<IAuthorizationHandler, PolicyHandler>(); //注入自定义的基于策略授权的实现
            services.AddLogging();

            //使用Autofac替换自带IOC
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<BusinessModule>();
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            AutofacHelper.Container = container;
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                #region Swagger
                app.UseSwagger(c => {
                    c.RouteTemplate = "swagger/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/通用/swagger.json", "通用");
                    c.SwaggerEndpoint("/swagger/登陆API/swagger.json", "登陆API");
                    c.SwaggerEndpoint("/swagger/权限API/swagger.json", "权限API");
                    c.RoutePrefix = "";
                });
                #endregion
            }
            app.UseCors("Standard");
            app.UseStaticFiles();
            app.UseMvc();

            InitEF();
        }
        private void InitEF()
        {
            Task.Run(() =>
            {
                try
                {
                    DbFactory.GetRepository(null, null, null).GetIQueryable<Base_User>().ToList();
                }
                catch
                {

                }
            });
        }
    }
    public class BusinessModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HomeBusiness>().AsImplementedInterfaces();
        }
    }
}
