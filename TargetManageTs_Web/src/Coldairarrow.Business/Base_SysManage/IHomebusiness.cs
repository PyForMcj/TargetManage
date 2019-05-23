using Coldairarrow.Util;
using Coldairarrow.Entity;
using Coldairarrow.Entity.Base_SysManage;

namespace Coldairarrow.Business.Base_SysManage
{
    public interface IHomebusiness
    {
        AjaxResult SubmitLogin(string userName, string password);

        Base_User JwtSubmitLogin(string userName, string password);
    }
}