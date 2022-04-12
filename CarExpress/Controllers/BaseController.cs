using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarExpress.Models;
using static CarExpress.Helpers.ApplicationHelper;

namespace CarExpress.Controllers
{
    public class BaseController : Controller
    {
        protected db_entities Database { get; set; }
        public BaseController()
        {
            Database = new db_entities();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            WebsiteModel webRecord = GetWebsiteData(Database);
            ViewBag.AppLocationId = webRecord.LocationID;
            ViewBag.AppUserId = webRecord.UserID;
            ViewBag.WebsiteURL = webRecord.WebsiteURL;
            ViewBag.WebsiteImageURL = webRecord.WebsiteImageURL;
            ViewBag.CurrencySymbol = webRecord.Currency;
            ViewBag.AppStatusId = webRecord.StatusID;
            if (IsCustomerLogin())
            {
                var customerRecord = GetCustomerData();
                ViewBag.CustomerRecord = customerRecord;
                ViewBag.CustomerWishListCount = Database.CustomerWishLists.Where(o => o.CustomerID == customerRecord.ID).Count();
            }
            ViewBag.CategoryMenuRecords = Database.Categories.Where(o => o.LocationID == webRecord.LocationID && o.StatusID == webRecord.StatusID && o.Image != null).OrderBy(o => o.DisplayOrder).Take(10).Select(s => new CategoryModel { ID = s.ID, Name = s.Name, Image = s.Image, Description = s.Description }).ToList();
            base.OnActionExecuting(filterContext);
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //if (filterContext.HttpContext.Request.IsAjaxRequest() == false && !filterContext.HttpContext.Request.Path.Equals("/captcha-image") && !filterContext.HttpContext.Request.Path.Equals("/logout"))
            //{

            //}
            base.OnActionExecuted(filterContext);
        }
        protected override void Dispose(bool disposing)
        {
            Database.Dispose();
            base.Dispose(disposing);
        }
    }
}