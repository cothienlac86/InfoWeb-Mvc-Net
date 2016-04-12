using InfoWebApp.Entity;
using InfoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace InfoWebApp.Controllers
{
    public class PrivateNewsController : Controller
    {
        //
        // GET: /TinChinhChu/

        [Authorize]
        [HttpGet]
        public ActionResult Index(
            int page = 1,
            int row = 20)
        {
            var areaDb = new AreaDb();
            var listArea = areaDb.GetAll();
            ViewBag.listArea = listArea;
            var listAreaChild = areaDb.GetListAreaById(1);
            ViewBag.listAreaChild = listAreaChild;
            var listStatus = new List<ListItem> { 
                new ListItem{
                    Text = "Lựa chọn loại tin",
                    Value = "0"
                },
                new ListItem{
                    Text = "Tin chính chủ",
                    Value = "1"
                },
                new ListItem{
                    Text = "Tin chưa được lọc",
                    Value = "2"
                },
                new ListItem{
                    Text = "Tin spam",
                    Value = "3"
                },
            };
            ViewBag.listStatus = listStatus;
            var privateNewsModel = new PrivateNewsModel();
            if (Session["ListOfPrivateNews"] != null)
            {
                privateNewsModel = (PrivateNewsModel)Session["ListOfPrivateNews"];
            }
            var privateNewsDb = new PrivateNewsDb();
            int intCount;
            privateNewsModel.listPrivateNews = privateNewsDb.SearchNews(out intCount,
                privateNewsModel.StartDate,
                privateNewsModel.EndDate,
                privateNewsModel.Title,
                privateNewsModel.Address, 
                privateNewsModel.Price, page, row, 
                privateNewsModel.Status);
            ViewBag.NumberOfPages = intCount / row + (intCount % row > 0 ? 1 : 0);
            ViewBag.CurrentPage = page;
            return View(privateNewsModel);
        }

        [Authorize]
        [HttpPost]
        public RedirectToRouteResult Index(PrivateNewsModel model)
        {
            Session["ListOfPrivateNews"] = model;
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Detail(int Id = 1)
        {
            var privateNewsDb = new PrivateNewsDb();
            var model = privateNewsDb.GetDetail(Id);
            return View(model);
        }

    }
}
