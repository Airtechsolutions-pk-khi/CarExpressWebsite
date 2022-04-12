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
    public class HomeController : BaseController
    {
        public void TestEmail()
        {
            string BodyEmail = System.IO.File.ReadAllText(Server.MapPath("~/assets/emailtemplates/emailpattern.txt"));
            string BodyEmailadmin = System.IO.File.ReadAllText(Server.MapPath("~/assets/emailtemplates/emailpattern-admin.txt"));

            var orderRecord1 = Database.Orders.Where(o => o.ID == 6541).FirstOrDefault();
            string items = "";
            foreach (var record in orderRecord1.OrderDetails)
            {
                items += "<table border = '0' cellpadding = '0' cellspacing = '0' align = 'center' width = '100%' role = 'module' data - type = 'columns' style = 'padding:20px 20px 20px 30px;' bgcolor = '#FFFFFF'>"
                          + "<tbody>"
                          + "<tr role = 'module-content'>"
                          + "<td height = '100%' valign = 'top'>"
                          + "<table class='column' width='137' style='width:137px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>"
                          + "<tbody>"
                          + "<tr>"
                          + "<td style = 'padding:0px;margin:0px;border-spacing:0;'><table class='wrapper' role='module' data-type='image' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='239f10b7-5807-4e0b-8f01-f2b8d25ec9d7'>"
                          + "<tbody>"
                          + "<tr>"
                          + "<td style = 'font-size:6px; line-height:10px; padding:0px 0px 0px 0px;' valign='top' align='left'>"
                          + "<img src = '" + ViewBag.WebsiteImageURL + record.Item.Image + "' class='max-width' border='0' style='display:block;width: 108px;height: 108px;object-fit: contain;' alt='' >"
                          + "</td>"
                          + "</tr>"
                          + "</tbody>"
                          + "</table></td>"
                          + "</tr>"
                          + "</tbody>"
                          + "</table>"
                          + "<table class='column' style='display: contents; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>"
                          + "<tbody>"
                          + "<tr>"
                          + "<td style = 'padding:0px;margin:0px;border-spacing:0;' ><table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='f404b7dc-487b-443c-bd6f-131ccde745e2'>"
                          + "<tbody>"
                          + "<tr>"
                          + "<td style = 'padding:18px 0px 18px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'><div>"
                          + "<div style = 'font-family: inherit; text-align: inherit'> " + record.Item.Name + "</div>"
                          + "<div style = 'font-family: inherit; text-align: inherit'> Qty : " + record.Quantity + "</div>"
                          + "<div style = 'font-family: inherit; text-align: inherit'><span style='color: #006782'>" + ViewBag.CurrencySymbol + " " + record.Price.Value.ToString("0.00") + "</span></div>"
                          + "<div></div></div></td>"
                          + "</tr>"
                          + "</tbody>"
                          + "</table>"
                          + "</td>"
                          + "</tr>"
                          + "</tbody>"
                          + "</table>"
                          + "</td>"
                          + "</tr>"
                          + "</tbody>"
                          + "</table>";
            }
            string gifts = "";
            foreach (var record in orderRecord1.OrderDetails.ToList())
            {
                foreach (var item in record.OrderModifierDetails)
                {
                    gifts += "<table border = '0' cellpadding = '0' cellspacing = '0' align = 'center' width = '100%' role = 'module' data - type = 'columns' style = 'padding:20px 20px 20px 30px;' bgcolor = '#FFFFFF'>"
                         + "<tbody>"
                         + "<tr role = 'module-content'>"
                         + "<td height = '100%' valign = 'top'>"
                         + "<table class='column' width='137' style='width:137px; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>"
                         + "<tbody>"
                         + "<tr>"
                         + "<td style = 'padding:0px;margin:0px;border-spacing:0;' ><table class='wrapper' role='module' data-type='image' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='239f10b7-5807-4e0b-8f01-f2b8d25ec9d7'>"
                         + "<tbody>"
                         + "<tr>"
                         + "<td style = 'font-size:6px; line-height:10px; padding:0px 0px 0px 0px;' valign='top' align='left'>"
                         + "<img src = '" + ViewBag.WebsiteImageURL + item.Modifier.Image + "' class='max-width' border='0' style='display:block;width: 108px;height: 108px;object-fit: contain;' alt='' >"
                         + "</td>"
                         + "</tr>"
                         + "</tbody>"
                         + "</table></td>"
                         + "</tr>"
                         + "</tbody>"
                         + "</table>"
                         + "<table class='column' style='display: contents; border-spacing:0; border-collapse:collapse; margin:0px 0px 0px 0px;' cellpadding='0' cellspacing='0' align='left' border='0' bgcolor=''>"
                         + "<tbody>"
                         + "<tr>"
                         + "<td style = 'padding:0px;margin:0px;border-spacing:0;' ><table class='module' role='module' data-type='text' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' data-muid='f404b7dc-487b-443c-bd6f-131ccde745e2'>"
                         + "<tbody>"
                         + "<tr>"
                         + "<td style = 'padding:18px 0px 18px 0px; line-height:22px; text-align:inherit;' height='100%' valign='top' bgcolor='' role='module-content'><div>"
                         + "<div style = 'font-family: inherit; text-align: inherit'> " + item.Modifier.Name + "</div>"
                         + "<div style = 'font-family: inherit; text-align: inherit'><span style='color: #006782'>" + ViewBag.CurrencySymbol + " " + item.Price.Value.ToString("0.00") + "</span></div>"
                         + "<div></div></div></td>"
                         + "</tr>"
                         + "</tbody>"
                         + "</table>"
                         + "</td>"
                         + "</tr>"
                         + "</tbody>"
                         + "</table>"
                         + "</td>"
                         + "</tr>"
                         + "</tbody>"
                         + "</table>";
                }
                gifts += "<table class='module' role='module' data-type='divider' border='0' cellpadding='0' cellspacing='0' width='100%' style='table-layout: fixed;' datad-muid='f7373f10-9ba4-4ca7-9a2e-1a2ba700deb9.1'>"
                + "<tbody>"
                + "<tr>"
                + "<td style='padding:20px 30px 0px 30px;' role='module-content' height='100%' valign='top' >"
                + "<table border='0' cellpadding='0' cellspacing='0' align='center' width='100%' height='3px' style='line-height:3px; font-size:3px;'>"
                + "<tbody>"
                + "<tr>"
                + "<td style='padding:0px 0px 3px 0px;background-color: #EF7C13'></td>"
                + "</tr>"
                + "</tbody>"
                + "</table>"
                + "</td>"
                + "</tr>"
                + "</tbody>"
                + "</table>";
            }
            var orderCustomer = orderRecord1.CustomerOrders.FirstOrDefault();
            var orderCheckout = orderRecord1.OrderCheckouts.FirstOrDefault();
            BodyEmail = BodyEmail.Replace("#ReceiverName#", orderCustomer.CustomerName);
            BodyEmail = BodyEmail.Replace("#OrderNo#", orderRecord1.OrderNo.ToString());
            BodyEmail = BodyEmail.Replace("#items#", items.ToString());
            BodyEmail = BodyEmail.Replace("#gifts#", gifts.ToString());

            BodyEmailadmin = BodyEmailadmin.Replace("#ReceiverName#", orderCustomer.CustomerName.ToString());
            BodyEmailadmin = BodyEmailadmin.Replace("#OrderNo#", orderRecord1.OrderNo.ToString());
            BodyEmailadmin = BodyEmailadmin.Replace("#items#", items.ToString());
            BodyEmailadmin = BodyEmailadmin.Replace("#gifts#", gifts.ToString());
            DateTime dateTime = DateTime.UtcNow.AddMinutes(180);
            BodyEmail = BodyEmail.Replace("#Customer#", orderCustomer.SenderName.ToString());
            BodyEmail = BodyEmail.Replace("#CustomerAddress#", orderCustomer.Address.ToString());
            //BodyEmail = BodyEmail.Replace("#CustomerContact#", data.SenderName.ToString());
            BodyEmail = BodyEmail.Replace("#SelectedTime#", orderCustomer.DeliveryTime.ToString());
            BodyEmail = BodyEmail.Replace("#DeliveryDate#", orderCustomer.DeliveryDate.Value.ToString("dd/MMM/yyyy"));
            BodyEmail = BodyEmail.Replace("#OrderDate#", orderRecord1.CreatedOn.Value.ToString("dd/MMM/yyyy"));
            BodyEmail = BodyEmail.Replace("#Address#", orderCustomer.Address.ToString());

            BodyEmailadmin = BodyEmailadmin.Replace("#Customer#", orderCustomer.SenderName.ToString());
            BodyEmailadmin = BodyEmailadmin.Replace("#CustomerAddress#", orderCustomer.Address.ToString());
            BodyEmailadmin = BodyEmailadmin.Replace("#CustomerContact#", orderCustomer.SenderContact.ToString());
            BodyEmailadmin = BodyEmailadmin.Replace("#SelectedTime#", orderCustomer.DeliveryTime.ToString());
            BodyEmailadmin = BodyEmailadmin.Replace("#DeliveryDate#", orderCustomer.DeliveryDate.Value.ToString("dd/MMM/yyyy"));
            BodyEmailadmin = BodyEmailadmin.Replace("#OrderDate#", orderRecord1.CreatedOn.Value.ToString("dd/MMM/yyyy"));
            BodyEmailadmin = BodyEmailadmin.Replace("#Address#", orderCustomer.Address.ToString());
            //string PaymentType = "";
            //PaymentType = data.PaymentMethodID == 3 ? "Paypal" : data.PaymentMethodID == 5 ? "Easypaisa" : data.PaymentMethodID == 3 ? "Banktransfer" : "Cash on delivery";
            //BodyEmail = BodyEmail.Replace("#PaymentType#", PaymentType.ToString());
            BodyEmail = BodyEmail.Replace("#Description#", orderCustomer.CardNotes.ToString());
            if (orderCheckout.PaymentMode.Equals("Cash"))
            {
                BodyEmail = BodyEmail.Replace("#PaymentType#", "Cash");
            }
            else
            {
                BodyEmail = BodyEmail.Replace("#PaymentType#", "Credit/Debit");
            }
            BodyEmail = BodyEmail.Replace("#TotalItems#", "");
            BodyEmail = BodyEmail.Replace("#SubTotal#", ViewBag.CurrencySymbol + " " + orderCheckout.AmountTotal.Value.ToString("0.00"));
            if (orderCheckout.Tax > 0)
            {
                BodyEmail = BodyEmail.Replace("#Tax#", ViewBag.CurrencySymbol + " " + orderCheckout.Tax.Value.ToString("0.00"));
            }
            else
            {
                BodyEmail = BodyEmail.Replace("#Tax#", ViewBag.CurrencySymbol + " 0.00");
            }
            if (orderCheckout.ServiceCharges > 0)
            {
                BodyEmail = BodyEmail.Replace("#DeliveryAmount#", ViewBag.CurrencySymbol + " " + orderCheckout.ServiceCharges.Value.ToString("0.00"));
            }
            else
            {
                BodyEmail = BodyEmail.Replace("#DeliveryAmount#", ViewBag.CurrencySymbol + " 0.00");
            }
            BodyEmail = BodyEmail.Replace("#GrandTotal#", ViewBag.CurrencySymbol + " " + orderCheckout.GrandTotal.Value.ToString("0.00"));
            //BodyEmailadmin = BodyEmailadmin.Replace("#PaymentType#", PaymentType.ToString());
            BodyEmailadmin = BodyEmailadmin.Replace("#Description#", orderCustomer.CardNotes.ToString());
            if (orderCheckout.PaymentMode.Equals("Cash"))
            {
                BodyEmailadmin = BodyEmailadmin.Replace("#PaymentType#", "Cash");
            }
            else
            {
                BodyEmailadmin = BodyEmailadmin.Replace("#PaymentType#", "Credit/Debit");
            }
            BodyEmailadmin = BodyEmailadmin.Replace("#TotalItems#", "");
            BodyEmailadmin = BodyEmailadmin.Replace("#SubTotal#", ViewBag.CurrencySymbol + " " + orderCheckout.AmountTotal.Value.ToString("0.00"));
            if (orderCheckout.Tax > 0)
            {
                BodyEmailadmin = BodyEmailadmin.Replace("#Tax#", ViewBag.CurrencySymbol + " " + orderCheckout.Tax.Value.ToString("0.00"));
            }
            else
            {
                BodyEmailadmin = BodyEmailadmin.Replace("#Tax#", ViewBag.CurrencySymbol + " 0.00");
            }
            if (orderCheckout.ServiceCharges > 0)
            {
                BodyEmailadmin = BodyEmailadmin.Replace("#DeliveryAmount#", ViewBag.CurrencySymbol + " " + orderCheckout.ServiceCharges.Value.ToString("0.00"));
            }
            else
            {
                BodyEmailadmin = BodyEmailadmin.Replace("#DeliveryAmount#", ViewBag.CurrencySymbol + " 0.00");
            }
            BodyEmailadmin = BodyEmailadmin.Replace("#GrandTotal#", ViewBag.CurrencySymbol + " " + orderCheckout.GrandTotal.Value.ToString("0.00"));


            MailObject OrderAdminMailObject = new MailObject();
            OrderAdminMailObject.MailFrom = ParseString(ConfigurationManager.AppSettings["EmailSender"]);
            OrderAdminMailObject.MailTo = "ubaidsaeedkhan@gmail.com";
            OrderAdminMailObject.Subject = "New Order #" + orderRecord1.ID;
            OrderAdminMailObject.Message = BodyEmailadmin;
            SendEmail(OrderAdminMailObject);
            MailObject OrderCustomerMailObject = new MailObject();
            OrderCustomerMailObject.MailFrom = ParseString(ConfigurationManager.AppSettings["EmailSender"]);
            OrderCustomerMailObject.MailTo = "ubaidsaeedkhan@gmail.com";
            OrderCustomerMailObject.Subject = "Thank You For Order #" + orderRecord1.ID;
            OrderCustomerMailObject.Message = BodyEmail;
            SendEmail(OrderCustomerMailObject);
        }
        [ValidateInput(false)]
        public ActionResult Page()
        {
            string currentURL = ParseString(System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["url"]).ToLower();
            if (WithOutLoginView().Contains(currentURL))
            {
                if (IsCustomerLogin())
                {
                    return Redirect(ViewBag.WebsiteURL + "/" + PageDashboard);
                }
            }
            else if (WithLoginView().Contains(currentURL))
            {
                if (!IsCustomerLogin())
                {
                    return Redirect("/");
                }
            }
            var CustomerRecord = GetCustomerData();
            bool isPageFound = false;
            int appLocationID = ViewBag.AppLocationId;
            int appUserID = ViewBag.AppUserId;
            int appStatusID = ViewBag.AppStatusId;
            List<BreadCrumbMenu> breadCrumbList = new List<BreadCrumbMenu>();
            switch (currentURL)
            {
                case PageLogout:
                    RemoveSession(Session_Customer_Login);
                    return Redirect("/");
                case PageLogin:
                    ViewBag.PageTitle = "Login";
                    ViewBag.LoginType = ParseString(Request["type"]);
                    LoginModel LoginRecordModel = new LoginModel();
                    string cookieEmailAddress = ParseString(GetCookie(Cookie_Customer_Email_Address));
                    string cookiePassword = ParseString(GetCookie(Cookie_Customer_Password));
                    if (!string.IsNullOrWhiteSpace(cookieEmailAddress) && !string.IsNullOrWhiteSpace(cookiePassword))
                    {
                        LoginRecordModel.EmailAddress = cookieEmailAddress;
                        LoginRecordModel.Password = cookiePassword;
                    }
                    return View(GetViewName("Customer/Login"), LoginRecordModel);
                case PageRegister:
                    ViewBag.PageTitle = "Register";
                    RegisterModel RegisterRecordModel = new RegisterModel();
                    return View(GetViewName("Customer/Register"), RegisterRecordModel);
                case PageForgot:
                    ViewBag.PageTitle = "Forgot Password";
                    return View(GetViewName("Customer/Forgot"));
                case PageDashboard:
                    ViewBag.PageTitle = "Dashboard";
                    var orderRecords = Database.Orders.Where(o => o.CustomerID == CustomerRecord.ID).ToList();
                    if(orderRecords.Count > 0)
                    {
                        ViewBag.OrderRecords = orderRecords;
                    }
                    return View(GetViewName("Customer/Dashboard"));
                case PageOrderDetail:
                    ViewBag.PageTitle = "Order Detail";
                    int orderID = ParseInt(Request["id"]);
                    var orderRecord = Database.Orders.FirstOrDefault(o => o.ID == orderID && o.CustomerID == CustomerRecord.ID);
                    if(orderRecord != null)
                    {
                        ViewBag.OrderRecord = orderRecord;
                        ViewBag.PageTitle = "Order Detail - " + orderRecord.ID;
                        return View(GetViewName("Customer/OrderDetail"));
                    }
                    else
                    {
                        return Redirect(ViewBag.WebsiteURL + "/" + PageDashboard);
                    }
                case PageProfile:
                    ViewBag.PageTitle = "Profile";
                    RegisterModel profileModel = new RegisterModel();
                    profileModel.Name = CustomerRecord.FullName;
                    profileModel.EmailAddress = CustomerRecord.Email;
                    profileModel.Mobile = CustomerRecord.Mobile;
                    profileModel.Address = CustomerRecord.Address;
                    profileModel.Password = CustomerRecord.Password;
                    return View(GetViewName("Customer/Profile"), profileModel);
                case PageCartListing:
                    ViewBag.PageTitle = "Cart";
                    return View(GetViewName("Pages/Cart"));
                case PageWishListing:
                    ViewBag.PageTitle = "Wish List";
                    if (IsCustomerLogin())
                    {
                        var WishListRecords = Database.CustomerWishLists.Where(o => o.CustomerID == CustomerRecord.ID).ToList();
                        if (WishListRecords.Count > 0)
                        {
                            ViewBag.WishListRecords = WishListRecords;
                        }
                    }
                    return View(GetViewName("Pages/Wish"));
                case PageCheckout:
                    CheckoutModel checkoutModel = new CheckoutModel();
                    ViewBag.PageTitle = "Checkout";
                    if (!IsCustomerLogin())
                    {
                        string type = ParseString(Request["type"]);
                        if (!type.ToLower().Equals("guest"))
                        {
                            return Redirect("/" + PageLogin + "?type=checkout");
                        }
                    }
                    else
                    {
                        if (IsCustomerLogin())
                        {
                            
                            checkoutModel.SenderName = CustomerRecord.FullName;
                            checkoutModel.SenderEmailAddress = CustomerRecord.Email;
                            checkoutModel.SenderMobileNumber = CustomerRecord.Mobile;
                        }
                    }
                    return View(GetViewName("Pages/Checkout"), checkoutModel);
                case PageProductDetail:
                    ViewBag.PageTitle = "ProductDetail";
                    int requestItemID = ParseInt(Request["item"]);
                    //Remove orignal price column
                    //ProductModel ProductModelRecord = Database.Items.Where(o => o.SubCategory.Category.LocationID == appLocationID && o.ID == requestItemID && o.StatusID == appStatusID).Select(s => new ProductModel { ID = s.ID, Name = s.Name, Description = s.Description, SubCategory = s.SubCategory.Name, Category = s.SubCategory.Category.Name, Image = s.Image, Barcode = s.Barcode, SKU = s.SKU, DisplayOrder = s.DisplayOrder, Price = s.Price, Cost = s.Cost, ItemType = s.ItemType, OriginalPrice = s.OriginalPrice, StarRating = s.Reveiws.Average(o => o.Stars) }).FirstOrDefault();
                    ProductModel ProductModelRecord = Database.Items.Where(o => o.SubCategory.Category.LocationID == appLocationID && o.ID == requestItemID && o.StatusID == appStatusID).Select(s => new ProductModel { ID = s.ID, Name = s.Name, Description = s.Description, SubCategory = s.SubCategory.Name, Category = s.SubCategory.Category.Name, Image = s.Image, Barcode = s.Barcode, SKU = s.SKU, DisplayOrder = s.DisplayOrder, Price = s.Price, Cost = s.Cost, ItemType = s.ItemType, StarRating = s.Reveiws.Average(o => o.Stars) }).FirstOrDefault();
                    if(ProductModelRecord.ID != 0)
                    {
                        breadCrumbList.Add(new BreadCrumbMenu() { Name = "Shop", AccessURL = "/" + PageShopListing });
                        ProductModelRecord.ItemImages = Database.ItemImages.Where(o => o.ItemID == ProductModelRecord.ID).Select(s => new ProductImageModel { ID = s.ItemImagesID, Image = s.Image }).ToList();
                        ProductModelRecord.Reviews = Database.Reveiws.Where(o => o.ItemID == ProductModelRecord.ID && o.StatusID == appStatusID).OrderBy(o => o.ID).Select(s => new ProductReviewModel { ID = s.ID, Name = s.Name, Email = s.Email, Description = s.Description, Contact = s.Contact, Star = s.Stars }).ToList();
                        ViewBag.BreadCrumbMenus = breadCrumbList;
                        ViewBag.ProductRecord = ProductModelRecord;
                        var GiftModelRecords = Database.Modifiers.Where(o => o.StatusID == appStatusID && o.UserID == appUserID).Select(s => new GiftModel { ID = s.ID, Name = s.Name, Image = s.Image, Description = s.Description, Barcode = s.Barcode, SKU = s.SKU, DisplayOrder = s.DisplayOrder, Price = s.Price, Type = s.Type }).ToList();
                        if(GiftModelRecords.Count > 0)
                        {
                            ViewBag.GiftRecords = GiftModelRecords;
                        }
                        ViewBag.PageTitle = ProductModelRecord.Name;
                        ProductReviewModel reviewModel = new ProductReviewModel();
                        reviewModel.ItemID = ProductModelRecord.ID;
                        reviewModel.Star = 1;
                        return View(GetViewName("Pages/Detail"), reviewModel);
                    }
                    break;
                case PageShopListing:
                    ViewBag.PageTitle = "Shop";
                    ShopDataModel ShopDataRecord = GetShopData(Database, ParseInt(Request["page"]), ParseString(Request["dir"]).ToLower(), ParseString(Request["sort_by"]).ToLower(), ParseString(Request["categories"]).ToLower(), 0, 0, appLocationID, appUserID, appStatusID, ParseString(Request["search_text"]).ToLower(), ParseInt(Request["categoryid"]));
                    ViewBag.MaxPrice = 500;
                    if (ShopDataRecord.ProductRecords.Count > 0)
                    {
                        ViewDataDictionary _viewData = new ViewDataDictionary();
                        _viewData.Add("ShopDataRecord", ShopDataRecord);
                        _viewData.Add("WebsiteURL", ViewBag.WebsiteURL);
                        _viewData.Add("WebsiteImageURL", ViewBag.WebsiteImageURL);
                        _viewData.Add("CurrencySymbol", ViewBag.CurrencySymbol);
                        TempDataDictionary _tempData = new TempDataDictionary();
                        string renderHTML = RenderPartialToString(this, "~/Views/Shared/_Shop.cshtml", null, _viewData, _tempData);
                        ViewBag.ShopContent = renderHTML;
                        if (ShopDataRecord.PaginationCount > 0)
                        {
                            ViewBag.IsLoadMore = true;
                        }
                    }
                    ViewBag.CategoryRecords = Database.Categories.Where(o => o.LocationID == appLocationID && o.StatusID == appStatusID).Select(s => new ShopCategoryModel() { ID = s.ID, Name = s.Name }).ToList();
                    ViewBag.SearchText = ParseString(Request["search_text"]);
                    ViewBag.MainCategoryID = ParseInt(Request["categoryid"]);
                    //Remove Orignal price column
                    //ViewBag.BestProductRecords = Database.Items.Where(o => o.SubCategory.Category.LocationID == appLocationID && o.StatusID == appStatusID).OrderByDescending(o => o.ID).Take(5).Select(s => new ProductModel { ID = s.ID, Name = s.Name, Description = s.Description, SubCategory = s.SubCategory.Name, Category = s.SubCategory.Category.Name, Image = s.Image, Barcode = s.Barcode, SKU = s.SKU, DisplayOrder = s.DisplayOrder, OriginalPrice = s.OriginalPrice, Price = s.Price, Cost = s.Cost, ItemType = s.ItemType, StarRating = s.Reveiws.Average(o => o.Stars) }).ToList();
                    ViewBag.BestProductRecords = Database.Items.Where(o => o.SubCategory.Category.LocationID == appLocationID && o.StatusID == appStatusID).OrderByDescending(o => o.ID).Take(5).Select(s => new ProductModel { ID = s.ID, Name = s.Name, Description = s.Description, SubCategory = s.SubCategory.Name, Category = s.SubCategory.Category.Name, Image = s.Image, Barcode = s.Barcode, SKU = s.SKU, DisplayOrder = s.DisplayOrder,  Price = s.Price, Cost = s.Cost, ItemType = s.ItemType, StarRating = s.Reveiws.Average(o => o.Stars) }).ToList(); 
                    return View(GetViewName("Pages/Shop"));
                case PageAboutUs:
                    ViewBag.PageTitle = "About Us";
                    ViewBag.ReviewRecords = Database.Reveiws.Where(o => o.StatusID == appStatusID).OrderByDescending(o => o.Stars).Take(5).Select(s => new ProductReviewModel { ID = s.ID, Name = s.Name, Email = s.Email, Description = s.Description, Contact = s.Contact, Star = s.Stars }).ToList();
                    return View(GetViewName("ContentPages/About"));
                case PageTerm:
                    ViewBag.PageTitle = "Terms & Condition";
                    return View(GetViewName("ContentPages/Term"));
                case PagePrivacy:
                    ViewBag.PageTitle = "Privacy Policy";
                    return View(GetViewName("ContentPages/Privacy"));
                case PageContactUs:
                    ViewBag.PageTitle = "Contact Us";
                    ContactModel contactModel = new ContactModel();
                    return View(GetViewName("ContentPages/Contact"), contactModel);
                case PageThankYou:
                    int oID = ParseInt(Request["id"]);
                    var oRecord = Database.Orders.FirstOrDefault(o => o.ID == oID); //o.CustomerID == CustomerRecord.ID &&
                    if (oRecord != null)
                    {
                        ViewBag.PageTitle = "Thank You";
                        ViewBag.OrderID = oRecord.OrderNo;
                        return View(GetViewName("ContentPages/ThankYou"));
                    }
                    break;
                case "":
                   // TestEmail();
                    ViewBag.BannerRecords = Database.Banners.Where(o => o.StatusID == appStatusID && o.LocationID == appLocationID).ToList();
                    // Remove Orignalprice column 
                    //ViewBag.FeatureProductRecords = Database.Items.Where(o => o.SubCategory.Category.LocationID == appLocationID && o.StatusID == appStatusID).OrderBy(o => Guid.NewGuid()).Take(4).Select(s => new ProductModel { ID = s.ID, Name = s.Name, Description = s.Description, SubCategory = s.SubCategory.Name, Category = s.SubCategory.Category.Name, Image = s.Image, Barcode = s.Barcode, SKU = s.SKU, DisplayOrder = s.DisplayOrder, OriginalPrice = s.OriginalPrice, Price = s.Price, Cost = s.Cost, ItemType = s.ItemType, StarRating = s.Reveiws.Average(o => o.Stars) }).ToList();
                    ViewBag.FeatureProductRecords = Database.Items.Where(o => o.SubCategory.Category.LocationID == appLocationID && o.StatusID == appStatusID).OrderBy(o => Guid.NewGuid()).Take(4).Select(s => new ProductModel { ID = s.ID, Name = s.Name, Description = s.Description, SubCategory = s.SubCategory.Name, Category = s.SubCategory.Category.Name, Image = s.Image, Barcode = s.Barcode, SKU = s.SKU, DisplayOrder = s.DisplayOrder, Price = s.Price, Cost = s.Cost, ItemType = s.ItemType, StarRating = s.Reveiws.Average(o => o.Stars) }).ToList();
                    ViewBag.LatestProductRecords = Database.Items.Where(o => o.SubCategory.Category.LocationID == appLocationID && o.StatusID == appStatusID).OrderBy(o => Guid.NewGuid()).Take(12).Select(s => new ProductModel { ID = s.ID, Name = s.Name, Description = s.Description, SubCategory = s.SubCategory.Name, Category = s.SubCategory.Category.Name, Image = s.Image, Barcode = s.Barcode, SKU = s.SKU, DisplayOrder = s.DisplayOrder, Price = s.Price, Cost = s.Cost, ItemType = s.ItemType, StarRating = s.Reveiws.Average(o => o.Stars) }).ToList();          
                    ViewBag.BestProductRecords = Database.Items.Where(o => o.SubCategory.Category.LocationID == appLocationID && o.StatusID == appStatusID).OrderBy(o => Guid.NewGuid()).Take(12).Select(s => new ProductModel { ID = s.ID, Name = s.Name, Description = s.Description, SubCategory = s.SubCategory.Name, Category = s.SubCategory.Category.Name, Image = s.Image, Barcode = s.Barcode, SKU = s.SKU, DisplayOrder = s.DisplayOrder, Price = s.Price, Cost = s.Cost, ItemType = s.ItemType, StarRating = s.Reveiws.Average(o => o.Stars) }).ToList();
                    ViewBag.ReviewRecords = Database.Reveiws.Where(o => o.StatusID == appStatusID).OrderByDescending(o => o.Stars).Take(5).Select(s => new ProductReviewModel { ID = s.ID, Name = s.Name, Email = s.Email, Description = s.Description, Contact = s.Contact, Star = s.Stars }).ToList();
                    ViewBag.PageTitle = "Home";
                    return View(GetViewName("Pages/Home"), new SubscribeModel());
                default:
                    break;
            }
            ViewBag.PageTitle = "404";
            return View(GetViewName("Pages/Page"));
        }
    }
}