using InfoWebApp.Generate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfoWebApp.Entity
{
    public class BaseController : Controller
    {
        public BaseController(){
            if (System.Web.HttpContext.Current.Session["UIConvert"] == null)
            {
                Common.ListMenuUlTag.Clear();
                Common.ListMenuSelectTag.Clear();
                Common.ListMenuBasic.Clear();
                var menuDb = new MenuDb();
                var list = menuDb.GetAll();
                var listAfterConvert = Common.GetTree(list, 0);
                Common.GetMenuUlTag(listAfterConvert);
                Common.GetMenuSelectTag(listAfterConvert);
                System.Web.HttpContext.Current.Session["UIConvert"] = Common.ListMenuUlTag.ToString();
                System.Web.HttpContext.Current.Session["UIMenu"] = Common.ListMenuSelectTag.ToString();
            }
        }
    }
}