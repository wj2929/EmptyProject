using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainServices.Entity;
using System.Web.Security;

namespace EmptyProject.Manage.Models
{
    public class ManageModels
    {
        IList<PermissionInfo> userPermissionInfos = new List<PermissionInfo>();
        public ManageModels()
        {
            this.MainMenus = new List<ManageMenuModel>();
            if (Authorized)
            {
                userPermissionInfos = TrainServices.WEB.UserBAL.GetUserPermissions(UserName,Roles.GetRolesForUser(), Membership.ApplicationName,string.Empty);
                userPermissionInfos = userPermissionInfos.Where(p => p.CanDisplay).ToList();
                string msg = String.Empty;

                IList<PermissionInfo> topNodes = userPermissionInfos
                    .Where(p => p.ParentId == null).OrderBy(p => p.Order).ToList();

                foreach (PermissionInfo p in topNodes)
                {
                    ManageMenuModel MainMenu = new ManageMenuModel(p.Name, p.Url);
                    CreateSubMenus(p, MainMenu);
                    this.MainMenus.Add(MainMenu);
                }
            }
        }

        void CreateSubMenus(PermissionInfo parentPermission, ManageMenuModel menu)
        {
            IList<PermissionInfo> children = userPermissionInfos
                .Where(p => p.ParentId == parentPermission.Id).OrderBy(p => p.Order).ToList();

            foreach (PermissionInfo p in children)
            {
                ManageMenuModel SubMenu = new ManageMenuModel(p.Name, p.Url);
                CreateSubMenus(p, SubMenu);
                menu.SubMenus.Add(SubMenu);
            }
                        
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Authorized
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }
        public string UserName
        {
            get
            {
                if (Authorized)
                    return HttpContext.Current.User.Identity.Name;
                else
                    return "匿名";
            }
        }

        public IList<ManageMenuModel> MainMenus;
    }

    public class ManageMenuModel
    {
        public ManageMenuModel(string Title, string Url)
        {
            this.Title = Title;
            this.Url = Url;
            SubMenus = new List<ManageMenuModel>();
        }
        /// <summary>
        /// 菜单标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 菜单Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public IList<ManageMenuModel> SubMenus { get; set; }
    }
}