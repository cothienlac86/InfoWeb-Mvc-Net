﻿using InfoWebApp.Entity;
using InfoWebApp.Generate;
using InfoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfoWebApp.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        [HttpGet]
        public ViewResult CreateMenu()
        {
            //Get Parent Menu
            var menu = new MenuDb();
            ViewBag.ListParentMenu = menu.GetParentMenu();
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMenu(MenuModels menuModels) {
            var menuDb = new MenuDb();
            menuModels = menuDb.Add(menuModels);
            RefreshMenu();
            Session["AddMenuSuccessMessage"] = "Thêm menu thành công!";
            return RedirectToAction("EditMenu", "Home", new { @Id = menuModels .Id});
        }

        [Authorize]
        [HttpGet]
        public ViewResult EditMenu(int Id = 1) {
            //Get Parent Menu
            var menuDb = new MenuDb();
            ViewBag.ListParentMenu = menuDb.GetParentMenu();
            //GetMenuById
            var model = menuDb.GetMenuById(Id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMenu(MenuModels menuModels) {
            var menuDb = new MenuDb();
            menuDb.Update(menuModels);
            ViewBag.ListParentMenu = menuDb.GetParentMenu();
            RefreshMenu();
            Session["AddMenuSuccessMessage"] = "Sửa menu thành công!";
            return View(menuModels);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Menu(){
            RefreshMenu();
            ViewBag.listMenuBasic = Common.ListMenuBasic;
            return View();
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteMenu(int Id) {
            var id = Id;
            var menuDb = new MenuDb();
            menuDb.DeleteMenuById(Id);
            RefreshMenu();
            return Json("Delete complete", JsonRequestBehavior.AllowGet);
        } 

        /// <summary>
        /// Refresh menu
        /// </summary>
        private void RefreshMenu() {
            var menuDb = new MenuDb();
            //Update menu again
            Common.ListMenuUlTag.Clear();
            Common.ListMenuSelectTag.Clear();
            Common.ListMenuBasic.Clear();
            var list = menuDb.GetAll();
            var listAfterConvert = Common.GetTree(list, 0);
            Common.GetMenuUlTag(listAfterConvert);
            Common.GetMenuSelectTag(listAfterConvert);
            Common.GetMenuList(listAfterConvert);
            System.Web.HttpContext.Current.Session["UIConvert"] = Common.ListMenuUlTag.ToString();
            System.Web.HttpContext.Current.Session["UIMenu"] = Common.ListMenuSelectTag.ToString();
        }
    }
}