using Autofac;
using Autofac.Extensions.DependencyInjection;
using Coldairarrow.Business.Base_SysManage;
using Coldairarrow.DataRepository;
using Coldairarrow.Entity.Base_SysManage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Web
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
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton(Configuration);
            services.AddLogging();

            //ʹ��Autofac�滻�Դ�IOC
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
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                //Ĭ��·��
                routes.MapRoute(
                    name: "Default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                //defaults: new { controller = "Home", action = "Index" }
                );

                //����
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

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

    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HomeBusiness>().AsImplementedInterfaces();
        }
    }
}
