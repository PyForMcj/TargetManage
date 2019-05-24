using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Coldairarrow.Entity;
using Coldairarrow.Business.Base_SysManage;
using Coldairarrow.Util;

namespace Coldairarrow.WebApi.Controllers.Login_Manage
{
    /// <summary>
    /// 登录管理控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Permission")]
    [ApiExplorerSettings(GroupName = "登陆API")]
    public class Base_LoginController : ControllerBase
    {
        #region 私有属性

        /// <summary>
        /// Jwt 服务
        /// </summary>
        private readonly IJwtAppService _jwtApp;

        /// <summary>
        /// 用户登录
        /// </summary>
        private readonly IHomebusiness _homeBus;

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="jwtApp"></param>
        /// <param name="homebus"></param>
        /// <param name="userOperationLog"></param>
        public Base_LoginController(IJwtAppService jwtApp, IHomebusiness homebus, IUserOperationLog userOperationLog)
        {
            _jwtApp = jwtApp;
            _homeBus = homebus;
        }

        #region 对外方法

        /// <summary>
        /// 获取 Jwt 授权数据
        /// </summary>
        /// <param name="dto">授权用户信息</param>
        [HttpPost("LoginSubmit")]
        [AllowAnonymous]
        [CheckParamNotEmpty("userName", "password")]
        [TypeFilter(typeof(UserOperationLogAttribute),Arguments = new object[] { "登录系统"})]
        public IActionResult LoginSubmit(SecretDto dto)
        {
            //Todo：获取用户信息
            var user = _homeBus.JwtSubmitLogin(dto.userName, dto.password);

            if (user == null)
                return Ok(
                    new AjaxResult
                    {
                        Success = true,
                        Msg = "无权访问！",
                        Data = new JwtResponseDto
                        {
                            Access = "无权访问！",
                            Type = "Bearer",
                            Profile = new Profile
                            {
                                UserName = dto.userName,
                                Auths = 0,
                                Expires = 0
                            }
                        },
                        ErrorCode = 0
                    });

            var jwt = _jwtApp.Create(user);
            return Ok(
                new AjaxResult
                {
                    Success = true,
                    Msg = "登录成功！",
                    Data = new JwtResponseDto
                    {
                        Access = jwt.Token,
                        Type = "Bearer",
                        Profile = new Profile
                        {
                            UserId = user.UserId,
                            UserName = user.UserName,
                            RealName = user.RealName,
                            Auths = jwt.Auths,
                            Expires = jwt.Expires
                        }
                    },
                    ErrorCode = 0
                });
        }

        /// <summary>
        /// 停用Jwt授权数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("CancelAccessToken")]
        [TypeFilter(typeof(UserOperationLogAttribute), Arguments = new object[] { "退出系统" })]
        public IActionResult CancelAccessToken()
        {
            _jwtApp.DeactivateCurrentAsync();
            return Ok(new AjaxResult
            {
                Success = true,
                Msg = "停用成功！",
                Data = null,
                ErrorCode = 0
            });
        }

        /// <summary>
        /// 刷新Jwt授权数据
        /// </summary>
        /// <param name="dto">刷新授权用户信息</param>
        /// <returns></returns>
        [HttpPost("RefreshAccessToken")]
        [TypeFilter(typeof(UserOperationLogAttribute), Arguments = new object[] { "刷新Token" })]
        public IActionResult RefreshAccessToken(SecretDto dto)
        {
            //Todo：获取用户信息
            var user = _homeBus.JwtSubmitLogin(dto.userName, dto.password);

            if (user == null)
                return Ok(
                    new AjaxResult
                    {
                        Success = true,
                        Msg = "无权访问！",
                        Data = new JwtResponseDto
                        {
                            Access = "无权访问！",
                            Type = "Bearer",
                            Profile = new Profile
                            {
                                UserName = dto.userName,
                                Auths = 0,
                                Expires = 0
                            }
                        },
                        ErrorCode = 0
                    });
        
            var jwt = _jwtApp.RefreshAsync(dto.Token, user);

            return Ok(
                new AjaxResult
                {
                    Success = true,
                    Msg = "刷新成功！",
                    Data = new JwtResponseDto
                    {
                        Access = jwt.Token,
                        Type = "Bearer",
                        Profile = new Profile
                        {
                            UserId = user.UserId,
                            UserName = user.UserName,
                            RealName = user.RealName,
                            Auths = jwt.Success ? jwt.Auths : 0,
                            Expires = jwt.Success ? jwt.Expires : 0
                        }
                    },
                    ErrorCode = 0
                });
        }

        #endregion
    }
}