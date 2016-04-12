using InfoWebApp.Entity;
using InfoWebApp.Models;
using SRVTextToImage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfoWebApp.Controllers
{
    public class NewsController : BaseController
    {
        //
        // GET: /News/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Create news post
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult CreatePost() {
            var areaDb = new AreaDb();
            var listArea = areaDb.GetAll();
            ViewBag.listArea = listArea;
            var listAreaChild = areaDb.GetListAreaById(1);
            ViewBag.listAreaChild = listAreaChild;
            return View();
        }

        /// <summary>
        /// Create news post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost(NewsModels model, string CaptchaText)
        {
            var areaDb = new AreaDb();
            var listArea = areaDb.GetAll();
            ViewBag.listArea = listArea;
            var listAreaChild = areaDb.GetListAreaById(1);
            ViewBag.listAreaChild = listAreaChild;

            if (model.MenuId == 0)
            {
                ModelState.AddModelError("MenuId", "Vui lòng chọn chuyên mục.");
            }

            if (model.QuanHuyenId == 0)
            {
                ModelState.AddModelError("QuanHuyenId", "Vui lòng chọn quận huyện.");
            }

            if (model.TinhThanhId == 0)
            {
                ModelState.AddModelError("TinhThanhId", "Vui lòng chọn tỉnh thành.");
            }

            if (this.Session["CaptchaImageText"].ToString() != CaptchaText)
            {
                ModelState.AddModelError("CaptchaText", "Mã xác nhận không đúng");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newsDb = new NewsDb();
            model.UserId = new UserDb().GetUserInfo(User.Identity.Name).Id;
            var Id = newsDb.Add(model).Id;
            Session["AddNewsSuccessMessage"] = "Đăng bài viết thành công!";
            return RedirectToAction("EditPost", "News", new { @Id = Id });
        }

        /// <summary>
        /// Create news post
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult EditPost(int Id = 1)
        {
            var newsDb = new NewsDb();
            var model = newsDb.GetNewsById(Id);

            var areaDb = new AreaDb();
            var listArea = areaDb.GetAll();
            ViewBag.listArea = listArea;
            var listAreaChild = areaDb.GetListAreaById(1);
            ViewBag.listAreaChild = listAreaChild;

            return View(model);
        }

        /// <summary>
        /// Edit news post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(NewsModels model, string CaptchaText)
        {
            var areaDb = new AreaDb();
            var listArea = areaDb.GetAll();
            ViewBag.listArea = listArea;
            var listAreaChild = areaDb.GetListAreaById(1);
            ViewBag.listAreaChild = listAreaChild;

            if (model.MenuId == 0)
            {
                ModelState.AddModelError("MenuId", "Vui lòng chọn chuyên mục.");
            }

            if (model.QuanHuyenId == 0)
            {
                ModelState.AddModelError("QuanHuyenId", "Vui lòng chọn quận huyện.");
            }

            if (model.TinhThanhId == 0)
            {
                ModelState.AddModelError("TinhThanhId", "Vui lòng chọn tỉnh thành.");
            }

            if (this.Session["CaptchaImageText"].ToString() != CaptchaText)
            {
                ModelState.AddModelError("CaptchaText", "Mã xác nhận không đúng");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newsDb = new NewsDb();
            
            var userId = new UserDb().GetUserInfo(User.Identity.Name).Id;
            if (model.UserId == userId)
            {
                newsDb.Update(model);
                Session["AddNewsSuccessMessage"] = "Sửa bài viết thành công!";
                return RedirectToAction("EditPost", "News", new { @Id = model.Id });
            }
            else {
                return RedirectToAction("MyPost", "News");
            }
        }

        /// <summary>
        /// Detail of News
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DetailPost(int Id = 1, int menuId = 1)
        {
            var newsDb = new NewsDb();
            var newsModel = new NewsModels();
            newsModel = newsDb.GetDetailNews(Id);
            return View(newsModel);
        }

        /// <summary>
        /// Get new by id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetNewsById(int Id = 1) {
            var id = Id;
            var area = new AreaDb();
            var list = area.GetListAreaById(id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get captcha
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")] // This is for output cache false
        public FileResult GetCaptchaImage()
        {
            CaptchaRandomImage CI = new CaptchaRandomImage();
            this.Session["CaptchaImageText"] = CI.GetRandomString(5); // here 5 means I want to get 5 char long captcha
            //CI.GenerateImage(this.Session["CaptchaImageText"].ToString(), 300, 75);
            // Or We can use another one for get custom color Captcha Image 
            CI.GenerateImage(this.Session["CaptchaImageText"].ToString(), 300, 75, Color.DarkGray, Color.White);
            MemoryStream stream = new MemoryStream();
            CI.Image.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stream, "image/png");
        }

        /// <summary>
        /// Show Post
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult ShowPost(
            int id = 1,
            int page = 1,
            int row = 20
            )
        {
            var areaDb = new AreaDb();
            var listArea = areaDb.GetAll();
            ViewBag.listArea = listArea;
            var listAreaChild = areaDb.GetListAreaById(1);
            ViewBag.listAreaChild = listAreaChild;

            var menuListModel = new MenuListModels();
            if (Session["ListOfPost"] != null)
            {
                menuListModel = (MenuListModels)Session["ListOfPost"];
            }
            
            var newsDb = new NewsDb();
            var menuDb = new MenuDb();
            int intCount;
            menuListModel.MenuId = id;
            menuListModel.MenuName = menuDb.GetMenuById(id).Name;
            menuListModel.newsModel = newsDb.SearchNews(out intCount, menuListModel.TinhThanhId, menuListModel.QuanHuyenId, menuListModel.Title, menuListModel.Price, page, row, id);

            ViewBag.NumberOfPages = intCount / row + (intCount % row > 0 ? 1 : 0);
            ViewBag.CurrentPage = page;
            ViewBag.MenuId = id;
            return View(menuListModel);
        }

        [HttpPost]
        public RedirectToRouteResult ShowPost(MenuListModels model) {
            Session["ListOfPost"] = model;
            return RedirectToAction("ShowPost",new {@id = model.MenuId});
        }

        [Authorize]
        [HttpGet]
        public ViewResult MyPost(
            int id = 1,
            int page = 1,
            int row = 20)
        {
            var areaDb = new AreaDb();
            var listArea = areaDb.GetAll();
            ViewBag.listArea = listArea;
            var listAreaChild = areaDb.GetListAreaById(1);
            ViewBag.listAreaChild = listAreaChild;

            var menuListModel = new MenuListModels();
            if (Session["ListOfMyPost"] != null)
            {
                menuListModel = (MenuListModels)Session["ListOfMyPost"];
            }

            var newsDb = new NewsDb();
            int intCount;
            id= new UserDb().GetUserInfo(User.Identity.Name).Id;
            menuListModel.newsModel = newsDb.SearchMyPost(out intCount, menuListModel.TinhThanhId, menuListModel.QuanHuyenId, menuListModel.Title, menuListModel.Price, page, row, id);
            ViewBag.NumberOfPages = intCount / row + (intCount % row > 0 ? 1 : 0);
            ViewBag.CurrentPage = page;
            return View(menuListModel);
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult MyPost(MenuListModels model)
        {
            Session["ListOfMyPost"] = model;
            return RedirectToAction("MyPost", new { @id = model.MenuId });
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeletePost(int Id = 0, int UserId = 0)
        {
            
            var newsDb = new NewsDb();
            var newsModel = newsDb.GetDetailNews(Id);
            var userId = new UserDb().GetUserInfo(User.Identity.Name).Id;
            if (newsModel.UserId == userId)
            {
                newsDb.Delete(Id);
                return Json("Xóa thành công", JsonRequestBehavior.AllowGet);
            }
            else {
                return Json("Bạn không có quyền xóa item này", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
