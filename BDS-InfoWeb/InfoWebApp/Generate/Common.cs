using InfoWebApp.Entity;
using InfoWebApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoWebApp.Generate
{
    /// <summary>
    /// Common function
    /// </summary>
    public static class Common
    {
        public static StringBuilder ListMenuSelectTag = new StringBuilder();
        public static StringBuilder ListMenuUlTag = new StringBuilder();
        public static List<MenuModels> ListMenuBasic = new List<MenuModels>();

        /// <summary>
        /// Get list tree
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<Tree> GetTree(List<MenuDb> list, int parent)
        {
            return list.Where(x => x.ParentId == parent).Select(x => new Tree
            {
                Id = x.Id,
                Name = x.Name,
                List = ((x.Id == 91) ? new List<Tree>() : GetTree(list, x.Id))
            }).ToList();
        }

        /// <summary>
        /// Get list tree
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<Tree> GetTree4Cbo(List<MenuModels> list, int parent)
        {
            return list.Where(x => x.ParentId == parent).Select(x => new Tree
            {
                Id = x.Id,
                Name = x.Name,
                List = GetTree4Cbo(list, x.Id)
            }).ToList();
        }

        /// <summary>
        /// Get list link for edit menu
        /// </summary>
        /// <param name="list"></param>
        public static void GetMenuList(List<Tree> list)
        {
            foreach (var item in list)
            {
                ListMenuBasic.Add(new MenuModels
                {
                    Name = item.Name,
                    Id = item.Id
                });
                if (item.List.Count > 0)
                {
                    GetMenuList(item.List);
                }
            }
        }

        /// <summary>
        /// Get menu with option for select tag
        /// </summary>
        /// <param name="list"></param>
        /// <param name="source"></param>
        public static void GetMenuSelectTag(List<Tree> list)
        {
            foreach (var item in list)
            {
                if (item.Id == 91) continue;
                if (item.List.Count > 0)
                {
                    ListMenuSelectTag.AppendLine("<optgroup label='" + item.Name + "'>");
                    GetMenuSelectTag(item.List);
                    ListMenuSelectTag.AppendLine("</optgroup>");
                }
                else
                {
                    ListMenuSelectTag.AppendLine("<option value='" + item.Id + "'>" + item.Name + "</option>");
                }
            }
        }
        /// <summary>
        /// Get menu with option for select tag
        /// </summary>
        /// <param name="list"></param>
        /// <param name="source"></param>
        public static string GetMenuSelectTag4Cbo(List<Tree> list)
        {
            StringBuilder menuTag = new StringBuilder();
            foreach (var item in list)
            {
                if (item.List.Count > 0)
                {
                    // <option value="0">--Chọn chuyên mục--</option>
                    menuTag.AppendLine("<option value='" +item.Id +"'>"+ item.Name + "</option>");
                    GetMenuSelectTag4Cbo(item.List);
                    menuTag.AppendLine("</option>");
                }
                else
                {
                    menuTag.AppendLine("<option value='" + item.Id + "'>" + item.Name + "</option>");
                }                
            }
            return menuTag.ToString();
        }

        /// <summary>
        /// Get menu with ul,li for ul tag
        /// </summary>
        /// <param name="list"></param>
        /// <param name="source"></param>
        public static void GetMenuUlTag(List<Tree> list, int source = 0)
        {
            if (source == 0)
            {
                ListMenuUlTag.AppendLine("<ul class='nav navbar-nav'>");
            }                
            else
            {
                ListMenuUlTag.AppendLine("<ul class='dropdown-menu'>");
            }
            var loop = 1;
            foreach (var item in list)
            {
                if (item.List.Count > 0)
                {
                    ListMenuUlTag.AppendLine("<li id='" + item.Id + "' class='dropdown' >");
                    ListMenuUlTag.AppendLine("<a href'#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>" + item.Name + "<span class='caret'></span></a>");
                }
                else
                {
                    ListMenuUlTag.AppendLine("<li id='" + item.Id + "' >");
                    ListMenuUlTag.AppendLine("<a href='/News/ShowPost/" + item.Id + "'>" + item.Name + "</a>");
                    if (loop < list.Count)
                        ListMenuUlTag.AppendLine("<li role='separator' class='divider'></li>");
                }

                if (item.List.Count > 0 && item.Id != 91)
                {
                    GetMenuUlTag(item.List, 91);
                }
                ListMenuUlTag.AppendLine("</li>");
                loop++;
            }
            ListMenuUlTag.AppendLine("</ul>");
        }

        /// <summary>
        /// Get menu with ul,li for ul tag
        /// </summary>
        /// <param name="list"></param>
        /// <param name="source"></param>
        public static void GetNewMenuUlTag(List<Tree> list, int source = 0)
        {
            var loop = 0;
            if (source == 0)
                ListMenuUlTag.AppendLine("<ul class='nav navbar-nav'>");
            else
            {
                ListMenuUlTag.AppendLine("<ul class='dropdown-menu'>");
            }


            foreach (var item in list)
            {
                if (item.List.Count > 0)
                {
                    if (item.Id == 1)
                    {
                        ListMenuUlTag.AppendLine("<li id='" + item.Id + "' >");
                        ListMenuUlTag.AppendLine("<a href='/News/ShowPost/" + item.Id + "'>" + item.Name + "</a>");
                    }
                    else
                    {
                        ListMenuUlTag.AppendLine("<li id='" + item.Id + "' class='dropdown' >");
                        ListMenuUlTag.AppendLine("<a href'#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>" + item.Name + "<span class='caret'></span></a>");
                    }
                }
                else
                {
                    ListMenuUlTag.AppendLine("<li id='" + item.Id + "' >");
                    ListMenuUlTag.AppendLine("<a href='/News/ShowPost/" + item.Id + "'>" + item.Name + "</a>");
                    if (loop < list.Count)
                        ListMenuUlTag.AppendLine("<li role='separator' class='divider'></li>");
                }

                if (item.List.Count > 0)
                {
                    GetMenuUlTag(item.List, 1);
                }
                ListMenuUlTag.AppendLine("</li>");
                loop++;
            }
            ListMenuUlTag.AppendLine("</ul>");
        }
    }
}