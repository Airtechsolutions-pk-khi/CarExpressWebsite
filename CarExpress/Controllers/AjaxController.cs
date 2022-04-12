using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CarExpress.Models;
using static CarExpress.Helpers.ApplicationHelper;

namespace CarExpress.Controllers
{
    public class AjaxController : Controller
    {
        protected db_entities Database { get; set; }
        public AjaxController()
        {
            Database = new db_entities();
        }
        [ValidateInput(false)]
        public ActionResult Page()
        {
            AjaxResponse AjaxResponse = new AjaxResponse();
            AjaxResponse.Success = false;
            AjaxResponse.Type = EnumJQueryResponseType.MessageOnly;
            AjaxResponse.Message = "No Post Data";
            try
            {
                if (Request.Form.Count > 0)
                {
                    WebsiteModel webRecord = GetWebsiteData(Database);
                    string currentURL = ParseString(System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["url"]);
                    switch (currentURL)
                    {
                        case PageAjaxCoupon:
                            string CouponCode = ParseString(Request.Form["code"]);
                            var couponRecord = Database.Coupons.Where(o => o.CouponCode.Equals(CouponCode) && o.StatusID == webRecord.StatusID && o.UserID == webRecord.UserID).Select(s => new CouponModel { ID = s.ID, Code = s.CouponCode, Type = s.Type, Amount = s.Amount }).FirstOrDefault();
                            if(couponRecord != null)
                            {
                                AjaxResponse.Success = true;
                                AjaxResponse.Type = EnumJQueryResponseType.DataOnly;
                                AjaxResponse.Message = "";
                                AjaxResponse.Data = couponRecord;
                            }
                            else
                            {
                                AjaxResponse.Message = "Invalid Coupon Code";
                            }
                            break;
                        case PageWishListRemove:
                            int cWID = ParseInt(Request.Form["id"]);
                            if (IsCustomerLogin())
                            {
                                Customer CustomerRecord = GetCustomerData();
                                var wishListRecord = Database.CustomerWishLists.FirstOrDefault(o => o.CustomerID == CustomerRecord.ID && o.ID == cWID);
                                if (wishListRecord != null)
                                {
                                    Database.CustomerWishLists.Remove(wishListRecord);
                                    Database.SaveChanges();
                                    AjaxResponse.Success = true;
                                    AjaxResponse.Type = EnumJQueryResponseType.DataOnly;
                                    AjaxResponse.Message = "Item Removed";
                                    AjaxResponse.Data = Database.CustomerWishLists.Where(o => o.CustomerID == CustomerRecord.ID).Count();
                                }
                                else
                                {
                                    AjaxResponse.Message = "Item not found in wishlist";
                                }
                            }
                            else
                            {
                                AjaxResponse.Message = "Please login to add item in wishlist";
                            }
                            break;
                        case PageWishListing:
                            int wItemID = ParseInt(Request.Form["id"]);
                            if (IsCustomerLogin())
                            {
                                Customer CustomerRecord = GetCustomerData();
                                var wishListRecord = Database.CustomerWishLists.FirstOrDefault(o => o.CustomerID == CustomerRecord.ID && o.ItemID == wItemID);
                                if (wishListRecord == null)
                                {
                                    wishListRecord = Database.CustomerWishLists.Create();
                                    wishListRecord.CustomerID = CustomerRecord.ID;
                                    wishListRecord.ItemID = wItemID;
                                    wishListRecord.CreatedOn = GetDateTime();
                                    Database.CustomerWishLists.Add(wishListRecord);
                                    Database.SaveChanges();
                                    AjaxResponse.Success = true;
                                    AjaxResponse.Type = EnumJQueryResponseType.DataOnly;
                                    AjaxResponse.Message = "Item Added to wishlist";
                                    AjaxResponse.Data = Database.CustomerWishLists.Where(o => o.CustomerID == CustomerRecord.ID).Count();
                                }
                                else
                                {
                                    AjaxResponse.Message = "Item already added";
                                }
                            }
                            else
                            {
                                AjaxResponse.Message = "Please login to add item in wishlist";
                            }
                            break;
                        case PageSetting:
                            AjaxResponse.Success = true;
                            AjaxResponse.Type = EnumJQueryResponseType.DataOnly;
                            AjaxResponse.Data = webRecord;
                            break;
                        case PageQuickView:
                            int ProductQuickViewID = ParseInt(Request.Form["id"]);
                            //Remove orignal price column
                            //var ProductRecord = Database.Items.Where(o => o.ID == ProductQuickViewID).Select(s => new ProductModel { ID = s.ID, Name = s.Name, SubCategory = s.SubCategory.Name, Category = s.SubCategory.Category.Name, Image = s.Image, Barcode = s.Barcode, SKU = s.SKU, DisplayOrder = s.DisplayOrder, Price = s.Price, OriginalPrice = s.OriginalPrice, Description = s.Description, StarRating = s.Reveiws.Average(o => o.Stars), Cost = s.Cost, ItemType = s.ItemType }).FirstOrDefault();
                            var ProductRecord = Database.Items.Where(o => o.ID == ProductQuickViewID).Select(s => new ProductModel { ID = s.ID, Name = s.Name, SubCategory = s.SubCategory.Name, Category = s.SubCategory.Category.Name, Image = s.Image, Barcode = s.Barcode, SKU = s.SKU, DisplayOrder = s.DisplayOrder, Price = s.Price, Description = s.Description, StarRating = s.Reveiws.Average(o => o.Stars), Cost = s.Cost, ItemType = s.ItemType }).FirstOrDefault();
                            if(ProductRecord.ID != 0)
                            {
                                ProductRecord.ItemImages = Database.ItemImages.Where(o => o.ItemID == ProductRecord.ID).Select(s => new ProductImageModel { ID = s.ItemImagesID, Image = s.Image }).ToList();
                                ProductRecord.Reviews = Database.Reveiws.Where(o => o.ItemID == ProductRecord.ID && o.StatusID == webRecord.StatusID).OrderBy(o => o.ID).Select(s => new ProductReviewModel { ID = s.ID, Name = s.Name, Email = s.Email, Description = s.Description, Contact = s.Contact, Star = s.Stars }).ToList();
                                ViewDataDictionary _viewData = new ViewDataDictionary();
                                _viewData.Add("WebsiteImageURL", ParseString(ConfigurationManager.AppSettings["WebsiteImageURL"]));
                                _viewData.Add("CurrencySymbol", ParseString(ConfigurationManager.AppSettings["CurrencySymbol"]));
                                _viewData.Add("ProductRecord", ProductRecord);
                                TempDataDictionary _tempData = new TempDataDictionary();
                                string renderHTML = RenderPartialToString(this, "~/Views/Shared/_QuickView.cshtml", null, _viewData, _tempData);
                                AjaxResponse.Success = true;
                                AjaxResponse.Type = EnumJQueryResponseType.DataOnly;
                                AjaxResponse.Data = renderHTML;
                            }
                            break;
                        case PageShopListing:
                            AjaxResponse.FieldName = "";
                           ShopDataModel ShopDataRecord = GetShopData(Database, ParseInt(Request.Form["page"]), ParseString(Request.Form["dir"]).ToLower(), ParseString(Request.Form["sort_by"]).ToLower(), ParseString(Request.Form["categories"]).ToLower(), ParseDouble(Request.Form["min_price"]), ParseDouble(Request.Form["max_price"]), webRecord.LocationID, webRecord.UserID, webRecord.StatusID, ParseString(Request["search_text"]).ToLower(), 0);
                            if(ShopDataRecord.ProductRecords.Count > 0)
                            {
                                ViewDataDictionary _viewData1 = new ViewDataDictionary();
                                _viewData1.Add("ShopDataRecord", ShopDataRecord);
                                _viewData1.Add("WebsiteURL", webRecord.WebsiteURL);
                                _viewData1.Add("WebsiteImageURL", webRecord.WebsiteImageURL);
                                _viewData1.Add("CurrencySymbol", webRecord.CurrencySymbol);
                                TempDataDictionary _tempData1 = new TempDataDictionary();
                                string renderHTML1 = RenderPartialToString(this, "~/Views/Shared/_Shop.cshtml", null, _viewData1, _tempData1);
                                //ViewBag.ShopContent = renderHTML1;
                                AjaxResponse.Success = true;
                                AjaxResponse.Type = EnumJQueryResponseType.DataOnly;
                                AjaxResponse.Data = renderHTML1;
                                AjaxResponse.Message = "";
                                int nextPage = ParseInt(Request.Form["page"]) + 1;
                                if (ShopDataRecord.PaginationCount > nextPage)
                                {
                                    AjaxResponse.FieldName = nextPage.ToString();
                                }
                                
                            }
                           
                            else
                            {
                                AjaxResponse.Message = "No More Data";
                            }

                            break;
                        case PageLogin:
                            string LoginEmailAddressValue = ParseString(Request.Form["EmailAddress"]);
                            string LoginPasswordValue = ParseString(Request.Form["Password"]);
                            string LoginTypeValue = ParseString(Request.Form["LoginType"]);
                            if (!string.IsNullOrWhiteSpace(LoginEmailAddressValue) && !string.IsNullOrWhiteSpace(LoginPasswordValue))
                            {
                                string LoginRememberMeValue = ParseString(Request.Form["RememberMe"]);
                                var CustomerRecord = Database.Customers.FirstOrDefault(o => o.Email.Equals(LoginEmailAddressValue) && o.Password.Equals(LoginPasswordValue) && o.StatusID == webRecord.StatusID && o.UserID == webRecord.UserID);
                                if(CustomerRecord != null)
                                {
                                    AddSession(Session_Customer_Login, CustomerRecord);
                                    if (!string.IsNullOrWhiteSpace(LoginRememberMeValue))
                                    {
                                        AddCookie(Cookie_Customer_Email_Address, LoginEmailAddressValue);
                                        AddCookie(Cookie_Customer_Password, LoginPasswordValue);
                                    }
                                    else
                                    {
                                        RemoveCookie(Cookie_Customer_Email_Address);
                                        RemoveCookie(Cookie_Customer_Password);
                                    }
                                    AjaxResponse.Success = true;
                                    AjaxResponse.Message = "";
                                    AjaxResponse.Type = EnumJQueryResponseType.RedirectOnly;
                                    if (LoginTypeValue.Equals("checkout"))
                                    {
                                        AjaxResponse.TargetURL = webRecord.WebsiteURL + "/" + PageCheckout;
                                    }
                                    else
                                    {
                                        AjaxResponse.TargetURL = webRecord.WebsiteURL + "/" + PageDashboard;
                                    }           
                                }
                                else
                                {
                                    AjaxResponse.Message = "Invalid login attempt.";
                                }
                            }
                            break;
                        case PageForgot:
                            string ForgotEmailAddressValue = ParseString(Request.Form["EmailAddress"]);
                            if (!string.IsNullOrWhiteSpace(ForgotEmailAddressValue))
                            {
                                var CustomerRecord = Database.Customers.FirstOrDefault(o => o.Email.Equals(ForgotEmailAddressValue) && o.StatusID == webRecord.StatusID && o.UserID == webRecord.UserID);
                                if (CustomerRecord != null)
                                {
                                    string ForgotBodyEmail = System.IO.File.ReadAllText(Server.MapPath("~/assets/emailtemplates/forgot-password.txt"));
                                    MailObject ForgotMailObject = new MailObject();
                                    ForgotMailObject.MailFrom = ParseString(ConfigurationManager.AppSettings["EmailSender"]);
                                    ForgotMailObject.MailTo = ForgotEmailAddressValue;
                                    ForgotMailObject.Subject = "Forgot Password - SamiaCrafts";
                                    ForgotBodyEmail = ForgotBodyEmail.Replace("[Password]", CustomerRecord.Password);
                                    ForgotMailObject.Message = ForgotBodyEmail;
                                    SendEmail(ForgotMailObject);
                                    AjaxResponse.Success = true;
                                    AjaxResponse.Type = EnumJQueryResponseType.MessageAndReloadWithDelay;
                                    AjaxResponse.Message = "Email with your password detail send successfully.";
                                }
                                else
                                {
                                    AjaxResponse.Message = "Invalid email address.";
                                }
                            }
                            break;
                        case PageProductDetail:
                            AjaxResponse.Message = "Please enter fields correctly";
                            string ReviewNameValue = ParseString(Request.Form["Name"]);
                            string ReviewEmailAddressValue = "";
                                //ParseString(Request.Form["Email"]);
                            string ReviewContactValue = "";
                            //ParseString(Request.Form["Contact"]);
                            string ReviewDescriptionValue = ParseString(Request.Form["Description"]);
                            int ReviewStarValue = ParseInt(Request.Form["Star"]);
                            int ReviewItemIDValue = ParseInt(Request.Form["ItemID"]);
                            if (!string.IsNullOrWhiteSpace(ReviewNameValue)  && !string.IsNullOrWhiteSpace(ReviewDescriptionValue) && ReviewItemIDValue != 0 && ReviewStarValue != 0)
                            {
                                var reviewRecord = Database.Reveiws.Create();
                                reviewRecord.Name = ReviewNameValue;
                                reviewRecord.Email = ReviewEmailAddressValue;
                                reviewRecord.Description = ReviewDescriptionValue;
                                reviewRecord.Contact = ReviewContactValue;
                                reviewRecord.Stars = ReviewStarValue;
                                reviewRecord.ItemID = ReviewItemIDValue;
                                reviewRecord.StatusID = webRecord.StatusID;
                                Database.Reveiws.Add(reviewRecord);
                                Database.SaveChanges();
                                AjaxResponse.Success = true;
                                AjaxResponse.Message = "Review Submitted Successfully";
                                AjaxResponse.Type = EnumJQueryResponseType.MessageAndReloadWithDelay;
                            }
                            break;
                        case PageRegister:
                            string RegisterNameValue = ParseString(Request.Form["Name"]);
                            string RegisterEmailAddressValue = ParseString(Request.Form["EmailAddress"]);
                            string RegisterPasswordValue = ParseString(Request.Form["Password"]);
                            string RegisterMobileValue = ParseString(Request.Form["Mobile"]);
                            string RegisterAddressValue = ParseString(Request.Form["Address"]);
                            if (!string.IsNullOrWhiteSpace(RegisterNameValue) && !string.IsNullOrWhiteSpace(RegisterEmailAddressValue) && !string.IsNullOrWhiteSpace(RegisterPasswordValue))
                            {
                                var CustomerRecord = Database.Customers.FirstOrDefault(o => o.Email.Equals(RegisterEmailAddressValue) && o.UserID == webRecord.StatusID);
                                if (CustomerRecord != null)
                                {
                                    AjaxResponse.Message = "Email Address already exist";
                                    AjaxResponse.FieldName = "EmailAddress";
                                    AjaxResponse.Type = EnumJQueryResponseType.FieldOnly;
                                }
                                else
                                {
                                    CustomerRecord = Database.Customers.Create();
                                    CustomerRecord.UserID = webRecord.UserID;
                                    CustomerRecord.StatusID = webRecord.StatusID;
                                    CustomerRecord.FullName = RegisterNameValue;
                                    CustomerRecord.Email = RegisterEmailAddressValue;
                                    CustomerRecord.Password = RegisterPasswordValue;
                                    CustomerRecord.Mobile = RegisterMobileValue;
                                    CustomerRecord.Address = RegisterAddressValue;
                                    CustomerRecord.CreatedOn = GetDateTime();
                                    Database.Customers.Add(CustomerRecord);
                                    Database.SaveChanges();
                                    var CustomerRecord1 = Database.Customers.FirstOrDefault(o => o.ID == CustomerRecord.ID);
                                    AddSession(Session_Customer_Login, CustomerRecord1);
                                    AjaxResponse.Success = true;
                                    AjaxResponse.Message = "";
                                    AjaxResponse.Type = EnumJQueryResponseType.RedirectOnly;
                                    AjaxResponse.TargetURL = webRecord.WebsiteURL + "/" + PageDashboard;
                                }
                            }
                            break;
                        case PageProfile:
                            string ProfileNameValue = ParseString(Request.Form["Name"]);
                            string ProfileEmailAddressValue = ParseString(Request.Form["EmailAddress"]);
                            string ProfilePasswordValue = ParseString(Request.Form["Password"]);
                            string ProfileMobileValue = ParseString(Request.Form["Mobile"]);
                            string ProfileAddressValue = ParseString(Request.Form["Address"]);
                            if (!string.IsNullOrWhiteSpace(ProfileNameValue) && !string.IsNullOrWhiteSpace(ProfileEmailAddressValue) && !string.IsNullOrWhiteSpace(ProfilePasswordValue))
                            {
                                Customer CustomerProfileRecord = GetCustomerData();
                                var CustomerEmailRecord = Database.Customers.FirstOrDefault(o => o.ID != CustomerProfileRecord.ID && o.Email.Equals(ProfileEmailAddressValue) && o.UserID == webRecord.StatusID);
                                if (CustomerEmailRecord != null)
                                {
                                    AjaxResponse.Message = "Email Address already exist";
                                    AjaxResponse.FieldName = "EmailAddress";
                                    AjaxResponse.Type = EnumJQueryResponseType.FieldOnly;
                                }
                                else
                                {
                                    var CustomerRecord = Database.Customers.FirstOrDefault(o => o.ID == CustomerProfileRecord.ID);
                                    CustomerRecord.UserID = webRecord.UserID;
                                    CustomerRecord.StatusID = webRecord.StatusID;
                                    CustomerRecord.FullName = ProfileNameValue;
                                    CustomerRecord.Email = ProfileEmailAddressValue;
                                    CustomerRecord.Password = ProfilePasswordValue;
                                    CustomerRecord.Mobile = ProfileMobileValue;
                                    CustomerRecord.Address = ProfileAddressValue;
                                    CustomerRecord.CreatedOn = GetDateTime();
                                    Database.Customers.Add(CustomerRecord);
                                    Database.SaveChanges();
                                    var CustomerRecord1 = Database.Customers.FirstOrDefault(o => o.ID == CustomerRecord.ID);
                                    AddSession(Session_Customer_Login, CustomerRecord1);
                                    AjaxResponse.Success = true;
                                    AjaxResponse.Message = "";
                                    AjaxResponse.Type = EnumJQueryResponseType.MessageAndReloadWithDelay;
                                    AjaxResponse.Message = "Profile Updated Successfully";
                                }
                            }
                            break;
                        case PageContactUs:
                            string ContactFullNameValue = ParseString(Request.Form["FullName"]);
                            string ContactEmailAddressValue = ParseString(Request.Form["EmailAddress"]);
                            string ContactSubjectValue = ParseString(Request.Form["Subject"]);
                            string ContactMessageValue = ParseString(Request.Form["Message"]);
                            if (!string.IsNullOrWhiteSpace(ContactFullNameValue) && !string.IsNullOrWhiteSpace(ContactEmailAddressValue) && !string.IsNullOrWhiteSpace(ContactSubjectValue) && !string.IsNullOrWhiteSpace(ContactMessageValue))
                            {
                                string ContactCustomerBodyEmail = System.IO.File.ReadAllText(Server.MapPath("~/assets/emailtemplates/contact-customer.txt"));
                                string ContactAdminBodyEmail = System.IO.File.ReadAllText(Server.MapPath("~/assets/emailtemplates/contact-admin.txt"));
                                ContactAdminBodyEmail = ContactAdminBodyEmail.Replace("[Message]", ContactMessageValue);
                                MailObject ContactAdminMailObject = new MailObject();
                                ContactAdminMailObject.MailFrom = ParseString(ConfigurationManager.AppSettings["EmailSender"]);
                                ContactAdminMailObject.MailTo = ParseString(ConfigurationManager.AppSettings["EmailReceivers"]);
                                ContactAdminMailObject.Subject = "Message From " + ContactFullNameValue + " (" + ContactEmailAddressValue + ") on the subject of " + ContactSubjectValue;
                                ContactAdminMailObject.Message = ContactAdminBodyEmail;
                                SendEmail(ContactAdminMailObject);
                                MailObject ContactCustomerMailObject = new MailObject();
                                ContactCustomerMailObject.MailFrom = ParseString(ConfigurationManager.AppSettings["EmailSender"]);
                                ContactCustomerMailObject.MailTo = ContactEmailAddressValue;
                                ContactCustomerMailObject.Subject = "Thank You For Contacting SamiaCrafts";
                                ContactCustomerMailObject.Message = ContactCustomerBodyEmail;
                                SendEmail(ContactCustomerMailObject);
                                AjaxResponse.Success = true;
                                AjaxResponse.Type = EnumJQueryResponseType.MessageAndReloadWithDelay;
                                AjaxResponse.Message = "Thank you for your query. We will get back to you as soon as we can.";
                            }
                            break;
                        case PageCheckout:
                            JArray SessionData = JArray.Parse(ParseString(Request.Form["SessionData"]));
                            string DiscountData = ParseString(Request.Form["DiscountData"]);
                            string SenderName = ParseString(Request.Form["SenderName"]);
                            string SenderEmailAddress = ParseString(Request.Form["SenderEmailAddress"]);
                            string SenderMobileNumber = ParseString(Request.Form["SenderMobileNumber"]);
                            string RecipientName = ParseString(Request.Form["RecipientName"]);
                            string RecipientMobileNumber = ParseString(Request.Form["RecipientMobileNumber"]);
                            string DeliveryDate = ParseString(Request.Form["DeliveryDate"]);
                            string DeliveryTime = ParseString(Request.Form["DeliveryTime"]);
                            string Country = ParseString(Request.Form["Country"]);
                            string City = ParseString(Request.Form["City"]);
                            string RecipientAddress = ParseString(Request.Form["RecipientAddress"]);
                            string PlaceType = ParseString(Request.Form["PlaceType"]);
                            string PaymentType = ParseString(Request.Form["payment_type"]);
                            string NearestPlace = ParseString(Request.Form["NearestPlace"]);
                            string Notes = ParseString(Request.Form["Notes"]);
                            double SubTotal = ParseDouble(Request.Form["SubTotal"]);
                            double DiscountTotal = ParseDouble(Request.Form["DiscountTotal"]);
                            double DeliveryCharges = ParseDouble(Request.Form["DeliveryCharges"]);
                            double TaxTotal = ParseDouble(Request.Form["Tax"]);
                            double GrandTotal = ParseDouble(Request.Form["GrandTotal"]);
                            int CustomerID = 0;
                            if (IsCustomerLogin())
                            {
                                Customer CustomerRecordDetail = GetCustomerData();
                                CustomerID = CustomerRecordDetail.ID;
                            }
                            else
                            {
                                Customer CustomerRecordDetail = Database.Customers.Create();
                                CustomerRecordDetail.UserID = webRecord.UserID;
                                CustomerRecordDetail.StatusID = webRecord.StatusID;
                                CustomerRecordDetail.FullName = SenderName;
                                CustomerRecordDetail.Email = SenderEmailAddress;
                                CustomerRecordDetail.Password = "";
                                CustomerRecordDetail.Mobile = SenderMobileNumber;
                                CustomerRecordDetail.Address = "";
                                CustomerRecordDetail.CreatedOn = GetDateTime();
                                Database.Customers.Add(CustomerRecordDetail);
                                Database.SaveChanges();
                                CustomerID = CustomerRecordDetail.ID;
                            }       
                            DateTime dt = GetDateTime();
                            int OrderNumber = 1;
                            int TransactionNo = 1;
                            var OrderMaxOrderNoValue = Database.Orders.Where(o => o.LocationID == webRecord.LocationID && DbFunctions.TruncateTime(o.CreatedOn) == DbFunctions.TruncateTime(dt)).Max(o => o.OrderNo);
                            if(OrderMaxOrderNoValue.HasValue)
                            {
                                OrderNumber = OrderMaxOrderNoValue.Value + 1;
                            }
                            var OrderMaxTransactionValue = Database.Orders.Where(o => o.LocationID == webRecord.LocationID).Max(o => o.TransactionNo);
                            if (OrderMaxTransactionValue.HasValue)
                            {
                                TransactionNo = OrderMaxTransactionValue.Value + 1;
                            }
                            var OrderRecord = Database.Orders.Create();
                            OrderRecord.CustomerID = CustomerID;
                            OrderRecord.LocationID = webRecord.LocationID;
                            OrderRecord.TransactionNo = TransactionNo;
                            OrderRecord.OrderNo = OrderNumber;
                            OrderRecord.StatusID = 2;
                            OrderRecord.DeliveryStatus = 101;
                            OrderRecord.OrderType = "Website";
                            OrderRecord.CreatedOn = dt;
                            OrderRecord.OrderCreatedDT = dt;
                            OrderRecord.IsAvailiable = true;
                            Database.Orders.Add(OrderRecord);
                            Database.SaveChanges();
                            
                            CustomerOrder CustomerOrderRecord = Database.CustomerOrders.Create();
                            CustomerOrderRecord.OrderID = OrderRecord.ID;
                            CustomerOrderRecord.Address = RecipientAddress;
                            CustomerOrderRecord.NearestPlace = NearestPlace;
                            CustomerOrderRecord.Country = Country;
                            CustomerOrderRecord.City = City;
                            CustomerOrderRecord.ContactNo = RecipientMobileNumber;
                            CustomerOrderRecord.OrderID = OrderRecord.ID;
                            CustomerOrderRecord.DeliveryDate = ParseDateTime(DeliveryDate);
                            CustomerOrderRecord.DeliveryTime = DeliveryTime;
                            CustomerOrderRecord.CustomerName = RecipientName;
                            CustomerOrderRecord.CustomerID = CustomerID;
                            CustomerOrderRecord.PlaceType = PlaceType;
                            CustomerOrderRecord.SenderName = SenderName;
                            CustomerOrderRecord.SenderEmail = SenderEmailAddress;
                            CustomerOrderRecord.SenderContact = SenderMobileNumber;
                            CustomerOrderRecord.CardNotes = Notes;
                            Database.CustomerOrders.Add(CustomerOrderRecord);
                            Database.SaveChanges();
                            if (SessionData.Count > 0)
                            {
                                for (int i = 0; i < SessionData.Count; i++)
                                {
                                    JToken CartRecord = JToken.Parse(SessionData[i].ToString());
                                    if (CartRecord != null)
                                    {
                                        int ProductQuantity = ParseInt(CartRecord["qty"]);
                                        OrderDetail OrderDetailRecord = Database.OrderDetails.Create();
                                        OrderDetailRecord.OrderID = OrderRecord.ID;
                                        OrderDetailRecord.ItemId = ParseInt(CartRecord["id"]);
                                        OrderDetailRecord.Quantity = ProductQuantity;
                                        double _Price = ParseDouble(CartRecord["price"]);
                                        OrderDetailRecord.Price = _Price * ProductQuantity;
                                        OrderDetailRecord.Cost = OrderDetailRecord.Price;
                                        OrderDetailRecord.TransactionNo = TransactionNo;
                                        OrderDetailRecord.OrderNo = OrderNumber;
                                        OrderDetailRecord.Name = ParseString(CartRecord["title"]);
                                        OrderDetailRecord.StatusID = webRecord.StatusID;
                                        OrderDetailRecord.CreatedOn = dt;
                                        Database.OrderDetails.Add(OrderDetailRecord);
                                        Database.SaveChanges();

                                        JArray GiftData = JArray.Parse(ParseString(CartRecord["gifts"]));
                                        if (GiftData.Count > 0)
                                        {
                                            for (int k = 0; k < GiftData.Count; k++)
                                            {
                                                JToken GiftRecord = JToken.Parse(GiftData[k].ToString());
                                                if(GiftRecord != null)
                                                {
                                                    OrderModifierDetail OrderModifierDetailRecord = Database.OrderModifierDetails.Create();
                                                    OrderModifierDetailRecord.OrderDetailID = OrderDetailRecord.ID;
                                                    OrderModifierDetailRecord.ModifierID = ParseInt(GiftRecord["id"]);
                                                    decimal _GiftPrice = ParseDecimal(GiftRecord["price"]);
                                                    OrderModifierDetailRecord.Price = _GiftPrice;
                                                    OrderModifierDetailRecord.Type = "Modifier";
                                                    OrderModifierDetailRecord.StatusID = webRecord.StatusID;
                                                    OrderModifierDetailRecord.Name = ParseString(GiftRecord["title"]);
                                                    Database.OrderModifierDetails.Add(OrderModifierDetailRecord);
                                                    Database.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            var OrderCheckOutRecord = Database.OrderCheckouts.Create();
                            OrderCheckOutRecord.OrderID = OrderRecord.ID;
                            OrderCheckOutRecord.LocationID = OrderRecord.LocationID;
                            OrderCheckOutRecord.CustomerID = CustomerID;
                            OrderCheckOutRecord.TransactionNo = OrderRecord.TransactionNo;
                            OrderCheckOutRecord.OrderNo = OrderRecord.OrderNo;
                            if (PaymentType.Equals("Cash")){
                                OrderCheckOutRecord.PaymentMode = 1;
                            }
                            else
                            {
                                OrderCheckOutRecord.PaymentMode = 2;
                            }
                            OrderCheckOutRecord.AmountTotal = SubTotal;
                            if(DeliveryCharges > 0)
                            {
                                OrderCheckOutRecord.ServiceCharges = DeliveryCharges;
                            }
                            if (TaxTotal > 0)
                            {
                                OrderCheckOutRecord.Tax = TaxTotal;
                            }
                            OrderCheckOutRecord.AmountDiscount = 0;
                            if (!string.IsNullOrWhiteSpace(DiscountData) && DiscountData != "null")
                            {
                                var returnDiscountModel = JsonConvert.DeserializeObject<CouponModel>(DiscountData);
                                if(returnDiscountModel != null)
                                {
                                    OrderCheckOutRecord.AmountDiscount = DiscountTotal;
                                    if (returnDiscountModel.Type.Equals("Percent"))
                                    {
                                        OrderCheckOutRecord.DiscountPercent = ParseDouble(returnDiscountModel.Amount);
                                        OrderCheckOutRecord.DiscountType = 1;
                                    }
                                    else
                                    {
                                        OrderCheckOutRecord.DiscountType = 2;
                                    }
                                }
                            }
                            OrderCheckOutRecord.AmountPaid = GrandTotal;
                            OrderCheckOutRecord.GrandTotal = GrandTotal;
                            OrderCheckOutRecord.OrderStatus = OrderRecord.StatusID;
                            OrderCheckOutRecord.CreatedOn = dt;
                            OrderCheckOutRecord.CheckoutDate = dt;
                            Database.OrderCheckouts.Add(OrderCheckOutRecord);
                            Database.SaveChanges();

                            string BodyEmail = System.IO.File.ReadAllText(Server.MapPath("~/assets/emailtemplates/order-customer.txt"));
                            string BodyEmailadmin = System.IO.File.ReadAllText(Server.MapPath("~/assets/emailtemplates/order-admin.txt"));

                            var orderRecord1 = Database.Orders.Where(o => o.ID == OrderRecord.ID).FirstOrDefault();
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
                                          + "<img src = '" + webRecord.WebsiteImageURL + record.Item.Image + "' class='max-width' border='0' style='display:block;width: 108px;height: 108px;object-fit: contain;' alt='' >"
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
                                          + "<div style = 'font-family: inherit; text-align: inherit'><span style='color: #006782'>" + webRecord.CurrencySymbol + " " + record.Price.Value.ToString("0.00") + "</span></div>"
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
                                         + "<img src = '" + webRecord.WebsiteImageURL + item.Modifier.Image + "' class='max-width' border='0' style='display:block;width: 108px;height: 108px;object-fit: contain;' alt='' >"
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
                                         + "<div style = 'font-family: inherit; text-align: inherit'><span style='color: #006782'>" + webRecord.CurrencySymbol + " " + item.Price.Value.ToString("0.00") + "</span></div>"
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
                            if (orderCheckout.PaymentMode == 1)
                            {
                                BodyEmail = BodyEmail.Replace("#PaymentType#", "Cash");
                            }
                            else
                            {
                                BodyEmail = BodyEmail.Replace("#PaymentType#", "Credit/Debit");
                            }
                            BodyEmail = BodyEmail.Replace("#TotalItems#", "");
                            BodyEmail = BodyEmail.Replace("#SubTotal#", webRecord.CurrencySymbol + " " + orderCheckout.AmountTotal.Value.ToString("0.00"));
                            if (orderCheckout.Tax > 0)
                            {
                                BodyEmail = BodyEmail.Replace("#Tax#", webRecord.CurrencySymbol + " " + orderCheckout.Tax.Value.ToString("0.00"));
                            }
                            else
                            {
                                BodyEmail = BodyEmail.Replace("#Tax#", webRecord.CurrencySymbol + " 0.00");
                            }
                            if (orderCheckout.ServiceCharges > 0)
                            {
                                BodyEmail = BodyEmail.Replace("#DeliveryAmount#", webRecord.CurrencySymbol + " " + orderCheckout.ServiceCharges.Value.ToString("0.00"));
                            }
                            else
                            {
                                BodyEmail = BodyEmail.Replace("#DeliveryAmount#", webRecord.CurrencySymbol + " 0.00");
                            }
                            BodyEmail = BodyEmail.Replace("#GrandTotal#", webRecord.CurrencySymbol + " " + orderCheckout.GrandTotal.Value.ToString("0.00"));
                            //BodyEmailadmin = BodyEmailadmin.Replace("#PaymentType#", PaymentType.ToString());
                            BodyEmailadmin = BodyEmailadmin.Replace("#Description#", orderCustomer.CardNotes.ToString());
                            if (orderCheckout.PaymentMode == 1)
                            {
                                BodyEmailadmin = BodyEmailadmin.Replace("#PaymentType#", "Cash");
                            }
                            else
                            {
                                BodyEmailadmin = BodyEmailadmin.Replace("#PaymentType#", "Credit/Debit");
                            }
                            BodyEmailadmin = BodyEmailadmin.Replace("#TotalItems#", "");
                            BodyEmailadmin = BodyEmailadmin.Replace("#SubTotal#", webRecord.CurrencySymbol + " " + orderCheckout.AmountTotal.Value.ToString("0.00"));
                            if (orderCheckout.Tax > 0)
                            {
                                BodyEmailadmin = BodyEmailadmin.Replace("#Tax#", webRecord.CurrencySymbol + " " + orderCheckout.Tax.Value.ToString("0.00"));
                            }
                            else
                            {
                                BodyEmailadmin = BodyEmailadmin.Replace("#Tax#", webRecord.CurrencySymbol + " 0.00");
                            }
                            if (orderCheckout.ServiceCharges > 0)
                            {
                                BodyEmailadmin = BodyEmailadmin.Replace("#DeliveryAmount#", webRecord.CurrencySymbol + " " + orderCheckout.ServiceCharges.Value.ToString("0.00"));
                            }
                            else
                            {
                                BodyEmailadmin = BodyEmailadmin.Replace("#DeliveryAmount#", webRecord.CurrencySymbol + " 0.00");
                            }
                            BodyEmailadmin = BodyEmailadmin.Replace("#GrandTotal#", webRecord.CurrencySymbol + " " + orderCheckout.GrandTotal.Value.ToString("0.00"));


                            MailObject OrderAdminMailObject = new MailObject();
                            OrderAdminMailObject.MailFrom = ParseString(ConfigurationManager.AppSettings["EmailSender"]);
                            OrderAdminMailObject.MailTo = ParseString(ConfigurationManager.AppSettings["EmailReceivers"]);
                            OrderAdminMailObject.Subject = "New Order #" + OrderRecord.OrderNo;
                            OrderAdminMailObject.Message = BodyEmailadmin;
                            SendEmail(OrderAdminMailObject);
                            MailObject OrderCustomerMailObject = new MailObject();
                            OrderCustomerMailObject.MailFrom = ParseString(ConfigurationManager.AppSettings["EmailSender"]);
                            OrderCustomerMailObject.MailTo = SenderEmailAddress;
                            OrderCustomerMailObject.Subject = "Thank You For Order #" + OrderRecord.OrderNo;
                            OrderCustomerMailObject.Message = BodyEmail;
                            SendEmail(OrderCustomerMailObject);

                            AjaxResponse.Success = true;
                            AjaxResponse.Message = "Order Created Successfully ";
                            AjaxResponse.Type = EnumJQueryResponseType.MessageAndRedirectWithDelay;
                            AjaxResponse.TargetURL = webRecord.WebsiteURL + "/" + PageThankYou + "?id=" + OrderRecord.ID;
                            break;
                        case "":
                            string SubscribeEmailAddressValue = ParseString(Request.Form["EmailAddress"]);
                            if (!string.IsNullOrWhiteSpace(SubscribeEmailAddressValue))
                            {
                                MailObject SubscribeAdminMailObject = new MailObject();
                                SubscribeAdminMailObject.MailFrom = ParseString(ConfigurationManager.AppSettings["EmailSender"]);
                                SubscribeAdminMailObject.MailTo = ParseString(ConfigurationManager.AppSettings["EmailSender"]);
                                SubscribeAdminMailObject.Subject = "New Subscriber : " + SubscribeEmailAddressValue;
                                SubscribeAdminMailObject.Message = "New Subscriber";
                                SendEmail(SubscribeAdminMailObject);
                                MailObject SubscribeCustomerMailObject = new MailObject();
                                SubscribeCustomerMailObject.MailFrom = ParseString(ConfigurationManager.AppSettings["EmailSender"]);
                                SubscribeCustomerMailObject.MailTo = SubscribeEmailAddressValue;
                                SubscribeCustomerMailObject.Subject = "Thank You";
                                SubscribeCustomerMailObject.Message = "Thank You For Subscribe with us";
                                SendEmail(SubscribeCustomerMailObject);
                                AjaxResponse.Success = true;
                                AjaxResponse.Type = EnumJQueryResponseType.MessageAndReloadWithDelay;
                                AjaxResponse.Message = "Thank You for subscribing with us.";
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string _catchMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    _catchMessage += "<br/>" + ex.InnerException.Message;
                }
                AjaxResponse.Message = _catchMessage;
            }
            return Json(AjaxResponse, "json");
        }
    }
}