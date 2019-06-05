using Coldairarrow.Business.Cache;
using Coldairarrow.Business.Common;
using Coldairarrow.Entity.Base_SysManage;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Coldairarrow.Business.Base_SysManage
{
    public class Base_UserBusiness : BaseBusiness<Base_User>
    {
        static Base_UserModelCache _cache { get; } = new Base_UserModelCache();

        #region Mvc�汾

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <param name="keyword">�ؼ���</param>
        /// <returns></returns>
        public List<Base_UserModel> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var where = LinqHelper.True<Base_UserModel>();

            Expression<Func<Base_User, Base_UserModel>> selectExpre = a => new Base_UserModel
            {

            };
            selectExpre = selectExpre.BuildExtendSelectExpre();

            var q = from a in GetIQueryable().AsExpandable()
                    select selectExpre.Invoke(a);

            //ģ����ѯ
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);

            var list = q.Where(where).GetPagination(pagination).ToList();
            SetProperty(list);

            return list;

            void SetProperty(List<Base_UserModel> users)
            {
                //�����û���ɫ����
                List<string> userIds = users.Select(x => x.UserId).ToList();
                var userRoles = (from a in Service.GetIQueryable<Base_UserRoleMap>()
                                 join b in Service.GetIQueryable<Base_SysRole>() on a.RoleId equals b.RoleId
                                 where userIds.Contains(a.UserId)
                                 select new
                                 {
                                     a.UserId,
                                     b.RoleId,
                                     b.RoleName
                                 }).ToList();
                users.ForEach(aUser =>
                {
                    aUser.RoleIdList = userRoles.Where(x => x.UserId == aUser.UserId).Select(x => x.RoleId).ToList();
                    aUser.RoleNameList = userRoles.Where(x => x.UserId == aUser.UserId).Select(x => x.RoleName).ToList();
                });
            }
        }

        /// <summary>
        /// ��ȡָ���ĵ�������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public Base_User GetTheData(string id)
        {
            return GetEntity(id);
        }

        public void AddData(Base_User newData)
        {
            if (GetIQueryable().Any(x => x.UserName == newData.UserName))
                throw new Exception("���û����Ѵ��ڣ�");

            Insert(newData);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void UpdateData(Base_User theData)
        {
            if (theData.UserId == "Admin" && Operator.UserId != theData.UserId)
                throw new Exception("��ֹ���ĳ�������Ա��");

            Update(theData);
            _cache.UpdateCache(theData.UserId);
        }

        public void SetUserRole(string userId, List<string> roleIds)
        {
            Service.Delete<Base_UserRoleMap>(x => x.UserId == userId);
            var insertList = roleIds.Select(x => new Base_UserRoleMap
            {
                Id = GuidHelper.GenerateKey(),
                UserId = userId,
                RoleId = x
            }).ToList();

            Service.Insert(insertList);
            _cache.UpdateCache(userId);
            PermissionManage.UpdateUserPermissionCache(userId);
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="theData">ɾ��������</param>
        public void DeleteData(List<string> ids)
        {
            var userIds = GetIQueryable().Where(x => ids.Contains(x.Id)).Select(x => x.UserId).ToList();
            var adminUser = GetTheUser("Admin");
            if (ids.Contains(adminUser.Id))
                throw new Exception("��������Ա�������˺�,��ֹɾ����");

            Delete(ids);
            _cache.UpdateCache(userIds);
        }

        /// <summary>
        /// ��ȡ��ǰ��������Ϣ
        /// </summary>
        /// <returns></returns>
        public static Base_UserModel GetCurrentUser()
        {
            return GetTheUser(Operator.UserId);
        }

        /// <summary>
        /// ��ȡ�û���Ϣ
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns></returns>
        public static Base_UserModel GetTheUser(string userId)
        {
            return _cache.GetCache(userId);
        }

        public static List<string> GetUserRoleIds(string userId)
        {
            return GetTheUser(userId).RoleIdList;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="oldPwd">������</param>
        /// <param name="newPwd">������</param>
        public AjaxResult ChangePwd(string oldPwd, string newPwd)
        {
            AjaxResult res = new AjaxResult() { Success = true };
            string userId = Operator.UserId;
            oldPwd = oldPwd.ToMD5String();
            newPwd = newPwd.ToMD5String();
            var theUser = GetIQueryable().Where(x => x.UserId == userId && x.Password == oldPwd).FirstOrDefault();
            if (theUser == null)
            {
                res.Success = false;
                res.Msg = "ԭ���벻��ȷ��";
            }
            else
            {
                theUser.Password = newPwd;
                Update(theUser);
            }

            _cache.UpdateCache(userId);

            return res;
        }

        /// <summary>
        /// ����Ȩ��
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="permissions">Ȩ��ֵ</param>
        public void SavePermission(string userId, List<string> permissions)
        {
            Service.Delete<Base_PermissionUser>(x => x.UserId == userId);
            var roleIdList = Service.GetIQueryable<Base_UserRoleMap>().Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
            var existsPermissions = Service.GetIQueryable<Base_PermissionRole>()
                .Where(x => roleIdList.Contains(x.RoleId) && permissions.Contains(x.PermissionValue))
                .GroupBy(x => x.PermissionValue)
                .Select(x => x.Key)
                .ToList();
            permissions.RemoveAll(x => existsPermissions.Contains(x));

            List<Base_PermissionUser> insertList = new List<Base_PermissionUser>();
            permissions.ForEach(newPermission =>
            {
                insertList.Add(new Base_PermissionUser
                {
                    Id = Guid.NewGuid().ToSequentialGuid(),
                    UserId = userId,
                    PermissionValue = newPermission
                });
            });

            Service.Insert(insertList);
        }

        #endregion

        #region WeiApi�汾

        #region ����UserID��ȡ�û�����Ȩ����Ϣ

        /// <summary>
        /// ����UserID��ȡ�û�����Ȩ����Ϣ
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns></returns>
        public Base_User GetCurrentUserInfo(string userId)
        {
            var param = new List<DbParameter>
            {
                new SqlParameter("@userId", userId)
            };
            Base_User data=null;
            string sqlStr = @"SELECT * FROM dbo.Base_User WHERE UserId=@userId;
SELECT a.* FROM dbo.Base_SysRole a INNER JOIN dbo.Base_UserRoleMap b ON a.RoleId=b.RoleId WHERE b.UserId=@userId;
SELECT DISTINCT(a.Id),a.* FROM dbo.Base_SysNavigation a 
INNER JOIN dbo.Base_SysRoleNavMap b ON a.Id=b.NavId
INNER JOIN dbo.Base_SysRole c ON b.RoleId=c.RoleId
INNER JOIN dbo.Base_UserRoleMap d ON c.RoleId=d.RoleId WHERE d.UserId=@userId;
SELECT DISTINCT(a.Id),a.* FROM dbo.Base_SysButton a 
INNER JOIN dbo.Base_SysRoleButtonMap b ON a.Id=b.BtnId
INNER JOIN dbo.Base_SysRole c ON b.RoleId=c.RoleId
INNER JOIN dbo.Base_UserRoleMap d ON c.RoleId=d.RoleId WHERE d.UserId=@userId;
";
            var dataSet = GetDataSetWithSql(sqlStr, param);
            if (dataSet.Tables.Count != 0)
            {
                //dataSet[0] �û���Ϣ
                if (dataSet.Tables[0] != null)
                {
                    data=dataSet.Tables[0].ToList<Base_User>().FirstOrDefault();
                }
                //dataSet[1] ��ɫ����
                if (dataSet.Tables[1] != null && data != null)
                {
                    data.Base_SysRoles = dataSet.Tables[1].ToList<Base_SysRole>();
                }
                //dataSet[2] �˵�����
                if (dataSet.Tables[2] != null && data != null)
                {
                    var sysnavs = dataSet.Tables[2].ToList<Base_SysNavigation>();
                    var topNav = sysnavs.Where(c => string.IsNullOrEmpty(c.ParentId)).ToList();
                    data.Base_SysNavigations = Base_UserBusiness.navigationDtos(sysnavs, topNav);
                }
                //dataSet[3] ��ť����
                if (dataSet.Tables[3] != null && data != null)
                {
                    data.Base_SysButtons = dataSet.Tables[3].ToList<Base_SysButton>();
                }
            }
            return data;

        }

        /// <summary>
        /// �ݹ�ѭ����һ���˵���
        /// ���κ�ֵ��ȡ����Ϊ��̬ʹ��
        /// </summary>
        /// <param name="base_SysNavigationsAll">ȫ��������</param>
        /// <param name="base_SysNavigations">ֻ��������������</param>
        /// <returns></returns>
        public static List<Base_SysNavigationDto> navigationDtos(List<Base_SysNavigation> base_SysNavigationsAll, List<Base_SysNavigation> base_SysNavigations)
        {
            List<Base_SysNavigationDto> base_SysNavigationDto = new List<Base_SysNavigationDto>();
            foreach (var item in base_SysNavigations)
            {
                //���ж�����������û���Ӽ�
                var isHaveChilds = base_SysNavigationsAll.Where(c => c.ParentId == item.Id).ToList();
                if (isHaveChilds.Count() > 0)
                {
                    Base_SysNavigationDto sysnav = new Base_SysNavigationDto
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        Path = item.Path,
                        Title = item.NavName,
                        Icon = item.Icon,
                        IconSvg = item.IconSvg,
                        Label=item.NavName
                    };
                    sysnav.Children = navigationDtos(base_SysNavigationsAll, isHaveChilds);
                    base_SysNavigationDto.Add(sysnav);
                }
                else
                {
                    Base_SysNavigationDto sysnav = new Base_SysNavigationDto
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        Path = item.Path,
                        Title = item.NavName,
                        Icon = item.Icon,
                        IconSvg = item.IconSvg,
                        Label = item.NavName
                    };
                    base_SysNavigationDto.Add(sysnav);
                }
            }
            return base_SysNavigationDto;
        }
        #endregion

        #endregion

        #region ˽�г�Ա

        #endregion

        #region ����ģ��

        #endregion
    }

    public class Base_UserModel : Base_User
    {
        public List<string> RoleIdList { get; set; }

        public List<string> RoleNameList { get; set; }
        public string RoleNames { get => string.Join(",", RoleNameList); }
        public EnumType.RoleType RoleType
        {
            get
            {
                int type = 0;

                var values = typeof(EnumType.RoleType).GetEnumValues();
                foreach (var aValue in values)
                {
                    if (RoleNames.Contains(aValue.ToString()))
                        type += (int)aValue;
                }

                return (EnumType.RoleType)type;
            }
        }
    }
}