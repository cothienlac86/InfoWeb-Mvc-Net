using InfoWebApp.Entity;
using InfoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace InfoWebApp.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserModel model)
        {
            if (Membership.ValidateUser(model.PhoneNumber, model.Password) && ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(model.PhoneNumber, model.Remember);
            }
            else {
                ModelState.AddModelError("Password", "Số điện thoại, mật khẩu không đúng hoặc tài khoản chưa được kích hoạt.");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Logout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterUserModel model, string CaptchaText)
        {
            if (this.Session["CaptchaImageText"].ToString() != CaptchaText)
            {
                ModelState.AddModelError("CaptchaText", "Mã xác nhận không đúng");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userDb = new UserDb();
            userDb.Register(model);
            return RedirectToAction("RegisterSuccess");
        }

        [HttpGet]
        public ActionResult ForgetPassword() {
            return View();    
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangePassword() {
            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model, string CaptchaText)
        {
            if (this.Session["CaptchaImageText"].ToString() != CaptchaText)
            {
                ModelState.AddModelError("CaptchaText", "Mã xác nhận không đúng");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.PhoneNumber = User.Identity.Name;
            var userDb = new UserDb();
            userDb.ChangePassword(model);
            Session["ChangePasswordSuccessMessage"] = "Thay đổi mật khẩu thành công!";
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Info()
        {
            var infoUser = new UserDb().GetUserInfo(User.Identity.Name);
            return View(infoUser);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Info(InfoUserModel model, string CaptchaText) {
            if (this.Session["CaptchaImageText"].ToString() != CaptchaText)
            {
                ModelState.AddModelError("CaptchaText", "Mã xác nhận không đúng");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userDb = new UserDb();
            userDb.UpdateInfo(model);
            return View(model);
        }

        public JsonResult CheckPhoneNumber(string phoneNumber)
        {
            var userDb = new UserDb();
            if (!userDb.CheckPhoneNumberExist(phoneNumber))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            string errorMessage = "Số điện thoại đã tồn tại";
            return Json(errorMessage, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegisterSuccess() {
            return View();
        }

    }
}
