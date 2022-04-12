using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Linq;
using System.Web.Caching;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using CarExpress.Models;
using System.Configuration;

namespace CarExpress.Helpers
{
    public static class ApplicationHelper
    {
        #region "Website Value Helper"
        public const string Cookie_Customer_Email_Address = "Cookie_Customer_Email_Address";
        public const string Cookie_Customer_Password = "Cookie_Customer_Password";
        public const string Session_Customer_Login = "Session_Customer_Login";
        public const string Session_Website_Data = "Session_Website_Data";
        public const string jQuery_Date_Format = "DD/MM/YYYY";
        public const string jQuery_Date_Time_Format = "DD/MM/YYYY HH:mm:ss";
        public const string Website_Date_Format = "dd/MM/yyyy";
        public const string Website_Date_Time_Format = "dd/MM/yyyy HH:mm:ss";
        public static string[] allowedImageExtensions = { ".jpg", ".png", ".gif", ".jpeg" };
        public static string[] DayNames = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        public static string[] JQueryDayNamesIndex = { "1", "2", "3", "4", "5", "6", "0" };
        public static string[] sortByArray = { "id", "name", "price" };
        public static string[] directionArray = { "desc", "asc" };
        #endregion
        #region "Page Name Value Helper"
        public const string PageQuickView = "quick-view";
        public const string PageWishListing = "wishlist";
        public const string PageWishListRemove = "wishlist-remove";
        public const string PageCartListing = "cart";
        public const string PageShopListing = "shop";
        public const string PageCheckout = "checkout";
        public const string PageLogin = "login";
        public const string PageRegister = "register";
        public const string PageForgot = "forgot";
        public const string PageReset = "reset";
        public const string PageDashboard = "dashboard";
        public const string PageThankYou = "thank-you";
        public const string PageProfile = "profile";
        public const string PageLogout = "logout";
        public const string PageProductDetail = "product-detail";
        public const string PageAboutUs = "about-us";
        public const string PageContactUs = "contact-us";
        public const string PageTerm = "t-c";
        public const string PagePrivacy = "privacy-policy";
        public const string PageReview = "review";
        public const string PageSetting = "get-setting";
        public const string PageOrderDetail = "order-detail";
        public const string PageSubscribe = "subscribe";
        public const string PageAjaxCoupon = "apply-coupon";
        #endregion
        #region "Redirect Page Helper"
        public static List<string> WithOutLoginView()
        {
            List<string> PageList = new List<string>();
            PageList.Add(PageLogin);
            PageList.Add(PageForgot);
            PageList.Add(PageReset);
            PageList.Add(PageRegister);
            return PageList;
        }
        public static List<string> WithLoginView()
        {
            List<string> PageList = new List<string>();
            PageList.Add(PageDashboard);
            PageList.Add(PageOrderDetail);
            PageList.Add(PageProfile);
            PageList.Add(PageLogout);
            return PageList;
        }
        #endregion
        #region "Enum Helper"
        public static class EnumJQueryResponseType
        {
            public const string DataOnly = "D";
            public const string MessageOnly = "M";
            public const string FieldOnly = "F";
            public const string RedirectOnly = "T";
            public const string RefreshOnly = "R";
            public const string ReloadOnly = "RL";
            public const string MessageAndRedirect = "M-T";
            public const string MessageAndRedirectWithDelay = "M-TD";
            public const string MessageAndRefresh = "M-R";
            public const string MessageRefreshRedirect = "M-R-T";
            public const string MessageRefreshRedirectWithDelay = "M-R-TD";
            public const string RefreshAndRedirect = "R-T";
            public const string RefreshAndRedirectWithDelay = "R-TD";
            public const string RedirectWithDelay = "TD";
            public const string MessageAndReloadWithDelay = "M-RLD";
        }
        public static class EnumYesNo
        {
            public const string Yes = "Yes";
            public const string No = "No";
        }
        #endregion
        #region "Core Functions"
        public class AjaxResponse
        {
            public bool Success { get; set; }
            public string Type { get; set; }
            public string FieldName { get; set; }
            public string Message { get; set; }
            public string TargetURL { get; set; }
            public object Data { get; set; }
        }
        public static WebsiteModel GetWebsiteData(db_entities Database)
        {
            WebsiteModel WebsiteRecord = null;
            if (IsWebsiteDataExist())
            {
                WebsiteRecord = (WebsiteModel)GetSession(Session_Website_Data);
            }
            else
            {
                WebsiteRecord = new WebsiteModel();
                WebsiteRecord.LocationID = ParseInt(ConfigurationManager.AppSettings["AppLocationId"]);
                WebsiteRecord.UserID = ParseInt(ConfigurationManager.AppSettings["AppUserId"]);
                WebsiteRecord.WebsiteURL = ParseString(ConfigurationManager.AppSettings["WebsiteURL"]);
                WebsiteRecord.WebsiteImageURL = ParseString(ConfigurationManager.AppSettings["WebsiteImageURL"]);
                WebsiteRecord.DeliveryCharges = 0;
                WebsiteRecord.TaxPercentage = 0;
                WebsiteRecord.MinimumOrderAmount = 0;
                WebsiteRecord.StatusID = 1;
                var companyRecord = Database.Companies.Where(o => o.UserID == WebsiteRecord.UserID).Select(s => new { s.TAX, s.Currency }).FirstOrDefault();
                if (companyRecord != null)
                {
                    WebsiteRecord.Currency = companyRecord.Currency;
                    WebsiteRecord.CurrencySymbol = companyRecord.Currency;
                    WebsiteRecord.TaxPercentage = ParseDecimal(companyRecord.TAX);
                }
                var locationRecord = Database.Locations.Where(o => o.ID == WebsiteRecord.LocationID).Select(s => new { s.DeliveryCharges, s.MinOrderAmount }).FirstOrDefault();
                if (locationRecord != null)
                {
                    WebsiteRecord.DeliveryCharges = ParseDouble(locationRecord.DeliveryCharges);
                    WebsiteRecord.MinimumOrderAmount = ParseDouble(locationRecord.MinOrderAmount);
                }
            }
            return WebsiteRecord;
        }
        public static ShopDataModel GetShopData(db_entities Database, int pageNumber, string direction, string sortBy, string categories, double minPrice, double maxPrice, int appLocationID, int appUserID, int appStatusID, string search_title, int mainCategoryID)
        {
            ShopDataModel ShopDataRecord = new ShopDataModel();
            ShopDataRecord.PaginationCount = 0;
            int startWith = 0;
            int limit = 32;
            string pageURL = "/" + PageShopListing + "?";
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }
            if (!directionArray.Contains(direction))
            {
                direction = directionArray[0];
            }
            else
            {
                pageURL += "dir=" + direction;
            }
            if (limit == 0 || limit > 32)
            {
                limit = 32;
            }
            if (!sortByArray.Contains(sortBy))
            {
                sortBy = sortByArray[0];
            }
            else
            {
                pageURL += "sort_by=" + sortBy;
            }
            startWith = (pageNumber - 1) * limit;
            //Remove orignal price column
            //string selectQuery = "select i.ID, i.Name, sc.Name as SubCategory, c.Name as Category, i.Image, i.Barcode, i.SKU, i.DisplayOrder, i.Price, i.OriginalPrice, i.Cost, i.ItemType";
            string selectQuery = "select i.ID, i.Name, sc.Name as SubCategory, c.Name as Category, i.Image, i.Barcode, i.SKU, i.DisplayOrder, i.Price, i.Cost, i.ItemType";
            string joinQuery = " from Item as i inner join SubCategory as sc on sc.ID=i.SubCategoryID";
            joinQuery += " inner join Category as c on c.ID=sc.CategoryID";
            joinQuery += " where c.LocationID=" + appLocationID + " and i.StatusID=" + appStatusID;
            if(minPrice > 0)
            {
                joinQuery += " and i.Price >= '" + minPrice + "'";
            }
            if (maxPrice > 0)
            {
                joinQuery += " and i.Price <= '" + maxPrice + "'";
            }
            if (!string.IsNullOrWhiteSpace(search_title))
            {
                joinQuery += " and i.Name like '%" + search_title + "%'";
            }
            if(mainCategoryID != 0)
            {
                categories += mainCategoryID + ",";
            }
            if (!string.IsNullOrWhiteSpace(categories))
            {
                categories = categories.Remove(categories.Length - 1, 1);
                joinQuery += " and c.ID in (" + categories + ")";
            }
            selectQuery += joinQuery;
            selectQuery += " order by i." + sortBy + " " + direction;
            selectQuery += " OFFSET " + startWith + " ROWS FETCH FIRST " + limit + " ROWS ONLY";
            var ProductRecords = Database.Database.SqlQuery<ProductModel>(selectQuery).OrderBy(o => Guid.NewGuid()).ToList();
            if (ProductRecords.Count > 0)
            {
                foreach(ProductModel Record in ProductRecords)
                {
                    Record.StarRating = Database.Reveiws.Where(o => o.ItemID == Record.ID).Average(s => s.Stars);
                }
                ShopDataRecord.ProductRecords = ProductRecords;      
                var TotalCount = Database.Database.SqlQuery<int>("select count(i.ID) as Total" + joinQuery).FirstOrDefault();
                if (TotalCount > 0)
                {
                    int pageCount = (int)Math.Ceiling((decimal)TotalCount / limit);
                    if (pageCount > 1)
                    {
                        ShopDataRecord.PaginationCount = pageCount;
                    }
                }
            }
            ShopDataRecord.CurrentPageNumber = pageNumber;
            return ShopDataRecord;
        }

        public static string GetViewName(string _viewName)
        {
            return "~/Views/" + _viewName + ".cshtml";
        }
        public static DateTime GetDateTime()
        {
            TimeZoneInfo time_zone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, time_zone);
        }
        public static DateTime GetUtcDateTime()
        {
            return DateTime.UtcNow;
        }
        public static string TextToSlug(string _value)
        {
            Regex rgx = new Regex(@"[^-a-zA-Z0-9\d\s]");
            // Replace Special Charater and space with emptystring 
            string finalOutput = rgx.Replace(_value, "");
            Regex rgx1 = new Regex("\\s+");
            // Replace space with underscore 
            finalOutput = rgx1.Replace(finalOutput, "-");
            if (finalOutput.StartsWith("-") || finalOutput.StartsWith(" "))
            {
                finalOutput = finalOutput.TrimStart(finalOutput[0]);
            }
            if (finalOutput.EndsWith("-") || finalOutput.EndsWith(" "))
            {
                finalOutput = finalOutput.TrimEnd(finalOutput[finalOutput.Length - 1]);
            }
            return finalOutput.ToLower();
        }
        public static string TrimCharacters(string _value, int _length = 1)
        {
            if (string.IsNullOrWhiteSpace(_value))
            {
                return _value;
            }
            else
            {
                return _value.TrimEnd(_value[_value.Length - _length]);
            }
        }
        public static string StripHtmlTags(string _value)
        {
            return Regex.Replace(_value, "<.*?>|&.*?;", string.Empty);
        }
        public static string UpperCaseFirstLetter(string _value)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(_value))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(_value[0]) + _value.Substring(1);
        }
        public static string UpperCaseWords(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }
        public static int CountStringOccurrences(string _value, string _pattern)
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ((i = _value.IndexOf(_pattern, i)) != -1)
            {
                i += _pattern.Length;
                count++;
            }
            return count;
        }
        public static string RenderPartialToString(Controller _controller, string _partialViewName, object _model, ViewDataDictionary _viewData, TempDataDictionary _tempData)
        {
            ViewEngineResult result = ViewEngines.Engines.FindPartialView(_controller.ControllerContext, _partialViewName);
            if (result.View != null)
            {
                _controller.ViewData.Model = _model;              
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    using (HtmlTextWriter output = new HtmlTextWriter(sw))
                    {
                        ViewContext viewContext = new ViewContext(_controller.ControllerContext, result.View, _viewData, _tempData, output);
                        result.View.Render(viewContext, output);
                    }
                }
                return sb.ToString();
            }
            return string.Empty;
        }
        public static double Round(double d)
        {
            var absoluteValue = Math.Abs(d);
            var integralPart = (long)absoluteValue;
            var decimalPart = absoluteValue - integralPart;
            var sign = Math.Sign(d);

            double roundedNumber;

            if (decimalPart > 0.5)
            {
                roundedNumber = integralPart + 1;
            }
            else if (decimalPart == 0)
            {
                roundedNumber = absoluteValue;
            }
            else
            {
                roundedNumber = integralPart + 0.5;
            }

            return sign * roundedNumber;
        }
        public static string RemoveHTMLTags(string value)
        {
            Regex regex = new Regex("\\<[^\\>]*\\>");
            value = regex.Replace(value, String.Empty);
            return value;
        }
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        #endregion
        #region "Mail Helper"
        public class MailObject
        {
            public string MailTo { get; set; }
            public string MailFrom { get; set; }
            public string Subject { get; set; }
            public string Message { get; set; }
        }
        public static void SendEmail(MailObject mailer)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(mailer.MailFrom) && !string.IsNullOrWhiteSpace(mailer.MailTo) && !string.IsNullOrWhiteSpace(mailer.Subject) && !string.IsNullOrWhiteSpace(mailer.Message))
                {
                    if (IsEmailAddressValid(mailer.MailFrom))
                    {
                        mailer.Message = mailer.Message.Replace("{Current Year}", GetUtcDateTime().Year.ToString());
                        string[] mail_to_array = mailer.MailTo.Split(',');
                        foreach (string mail_to in mail_to_array)
                        {
                            var emailTo = mail_to.Trim();
                            if (!string.IsNullOrWhiteSpace(emailTo))
                            {
                                if (IsEmailAddressValid(emailTo))
                                {
                                    MailMessage mm = new MailMessage();
                                    mm.From = new MailAddress(mailer.MailFrom);
                                    mm.To.Add(new MailAddress(emailTo));
                                    mm.Subject = mailer.Subject;
                                    mm.Body = mailer.Message;
                                    mm.IsBodyHtml = true;

                                    using (SmtpClient client = new SmtpClient())
                                    {
                                        client.Host = "smtp.gmail.com";
                                        client.EnableSsl = true;
                                        NetworkCredential NetworkCred = new NetworkCredential("craftssamia@gmail.com", "s@m!@cr@ft$");
                                        client.UseDefaultCredentials = false;
                                        client.Credentials = NetworkCred;
                                        client.Port = 587;
                                        client.Send(mm);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string logfile = HttpContext.Current.Server.MapPath("~/Logs/logs.txt");
                StreamWriter fileStreamWriter = new StreamWriter(logfile, false);
                if (fileStreamWriter != null)
                {
                    fileStreamWriter.Write(ex);
                    fileStreamWriter.Close();
                }
            }
        }
        #endregion
        #region "Limit Helper"
        public static string LimitLength(string paragraph, int maximumLenght)
        {
            //null check
            if (paragraph == null) return null;
            //less than maximum length, return as it is
            if (paragraph.Length <= maximumLenght) return paragraph;
            //split the paragraph into indvidual words 
            string[] words = paragraph.Split(' ');
            //initialize return variable 
            string paragraphToReturn = string.Empty;
            //construct the return word 
            foreach (string word in words)
            {
                //check if adding 3 to current length and next word is more than maximum length. 
                if ((paragraphToReturn.Length + word.Length + 3) > maximumLenght)
                {
                    //append "..."
                    paragraphToReturn = paragraphToReturn.Trim() + "...";
                    //exit foreach loop
                    break;
                }
                //add next word and continue
                paragraphToReturn += word + " ";
            }
            return paragraphToReturn;
        }
        public static string LimitCharacterLength(string paragraph, int maximumLenght)
        {
            //null check
            if (paragraph == null) return null;
            //less than maximum length, return as it is
            if (paragraph.Length <= maximumLenght) return paragraph;
            return paragraph.Substring(0, maximumLenght);
        }
        public static string LimitWordLength(string paragraph, int maximumLenght)
        {
            //null check
            if (paragraph == null) return null;
            //split the paragraph into indvidual words 
            string[] words = paragraph.Split(' ');
            //initialize return variable 
            string paragraphToReturn = string.Empty;
            if (words.Length <= maximumLenght)
            {
                return paragraph;
            }
            int w_counter = 1;
            //construct the return word 
            foreach (string word in words)
            {
                //check if adding 3 to current length and next word is more than maximum length. 
                if (w_counter == maximumLenght)
                {
                    //append "..."
                    paragraphToReturn = paragraphToReturn.Trim() + "...";
                    //exit foreach loop
                    break;
                }
                //add next word and continue
                paragraphToReturn += word + " ";
                w_counter += 1;
            }
            return paragraphToReturn;
        }
        #endregion    
        #region "Default Values Helper"
        public static int ParseInt(object value)
        {
            int parseVal;
            return ((value == null) || (value == DBNull.Value)) ? 0 : int.TryParse(value.ToString(), out parseVal) ? parseVal : 0;
        }
        public static decimal ParseDecimal(object value)
        {
            decimal parseVal;
            return ((value == null) || (value == DBNull.Value)) ? 0 : decimal.TryParse(value.ToString(), out parseVal) ? parseVal : 0;
        }
        public static double ParseDouble(object value)
        {
            double parseVal;
            return ((value == null) || (value == DBNull.Value)) ? 0 : double.TryParse(value.ToString(), out parseVal) ? parseVal : 0;
        }
        public static DateTime ParseDateTime(object value)
        {
            DateTime parseVal;
            //CultureInfo ci = CultureInfo.CreateSpecificCulture("en-GB");
            return ((value == null) || (value == DBNull.Value)) ? new DateTime(1900, 1, 1) : DateTime.TryParse(value.ToString(), out parseVal) ? parseVal : new DateTime(1900, 1, 1);
        }
        public static DateTime ParseExactDateTime(object value)
        {
            DateTime parseVal;
            CultureInfo ci = CultureInfo.CreateSpecificCulture("en-GB");
            // return DateTime.Parse(value.ToString(), ci.DateTimeFormat);
            return ((value == null) || (value == DBNull.Value)) ? new DateTime(1900, 1, 1) : DateTime.TryParseExact(value.ToString(), Website_Date_Format, ci, DateTimeStyles.None, out parseVal) ? parseVal : new DateTime(1900, 1, 1);
        }
        public static DateTime ParsePickerDateTime(object value)
        {
            DateTime parseVal;
            CultureInfo ci = CultureInfo.CreateSpecificCulture("en-GB");
            return ((value == null) || (value == DBNull.Value)) ? new DateTime(1900, 1, 1) : DateTime.TryParse(value.ToString(), ci.DateTimeFormat, DateTimeStyles.None, out parseVal) ? parseVal : new DateTime(1900, 1, 1);
        }
        public static string ParseString(object value)
        {
            return ((value == null) || (value == DBNull.Value)) ? string.Empty : value.ToString();
        }
        public static bool ParseBoolean(object value)
        {
            bool parseVal;
            return ((value == null) || (value == DBNull.Value)) ? false : bool.TryParse(value.ToString(), out parseVal) ? parseVal : false;
        }
        public static bool IsEmailAddressValid(string EmailAddress)
        {
            string pattern = @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(EmailAddress);
        }
        #endregion
        #region "Cache Helper"
        public static bool isCacheContextAvailable()
        {
            bool ReturnValue = true;
            if (HttpContext.Current.Cache == null)
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }
        public static void AddItemToCache(string _key, object _value)
        {
            if (isCacheContextAvailable() == true)
            {
                HttpContext.Current.Cache.Add(_key, _value, null, DateTime.Now.AddDays(30), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
        }
        public static object GetItemFromCache(string _key)
        {
            object ReturnObject = string.Empty;
            if (isCacheContextAvailable() == true)
            {
                if (HttpContext.Current.Cache.Get(_key) != null)
                {
                    ReturnObject = HttpContext.Current.Cache.Get(_key);
                }
            }
            return ReturnObject;
        }
        public static void RemoveItemFromCache(string _key)
        {
            if (isCacheContextAvailable() == true)
            {
                if (HttpContext.Current.Cache.Get(_key) != null)
                {
                    HttpContext.Current.Cache.Remove(_key);
                }
            }
        }
        public static void ClearAllCache()
        {
            if (isCacheContextAvailable() == true)
            {
                var CacheEnum = HttpContext.Current.Cache.GetEnumerator();
                while (CacheEnum.MoveNext())
                {
                    HttpContext.Current.Cache.Remove(CacheEnum.Key.ToString());
                }
            }
        }
        #endregion
        #region "Cookie Helper"
        public static void AddCookie(string _key, string _value, int _numberOfHourAdd = 0)
        {
            System.Web.HttpCookie CookieObject = new System.Web.HttpCookie(_key);
            CookieObject.Value = _value;
            if (_numberOfHourAdd > 0)
            {
                CookieObject.Expires = GetUtcDateTime().AddHours(_numberOfHourAdd);
            }
            HttpContext.Current.Response.Cookies.Add(CookieObject);
        }
        public static string GetCookie(string _key)
        {
            string ReturnValue = string.Empty;
            System.Web.HttpCookie CookieObject = HttpContext.Current.Request.Cookies[_key];
            if (CookieObject != null)
            {
                ReturnValue = CookieObject.Value;
            }
            return ReturnValue;
        }
        public static void RemoveCookie(string _key)
        {
            System.Web.HttpCookie CookieObject = HttpContext.Current.Request.Cookies[_key];
            if (CookieObject != null)
            {
                CookieObject.Expires = GetUtcDateTime().AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(CookieObject);
            }
        }
        #endregion
        #region "Session Helper"
        public static void AddSession(string _key, object _value)
        {
            HttpContext.Current.Session.Add(_key, _value);
        }
        public static object GetSession(string _key)
        {
            object ReturnObject = null;
            var SessionObject = HttpContext.Current.Session[_key];
            if (SessionObject != null)
            {
                ReturnObject = SessionObject;
            }
            return ReturnObject;
        }
        public static void RemoveSession(string _key)
        {
            var SessionObject = HttpContext.Current.Session[_key];
            if (SessionObject != null)
            {
                HttpContext.Current.Session.Remove(_key);
            }
        }
        public static bool IsCustomerLogin()
        {
            bool ReturnValue = false;
            if (GetSession(Session_Customer_Login) != null)
            {
                ReturnValue = true;
            }
            return ReturnValue;
        }
        public static Customer GetCustomerData()
        {
            Customer CustomerRecord = null;
            if (IsCustomerLogin())
            {
                CustomerRecord = (Customer)GetSession(Session_Customer_Login);
            }
            return CustomerRecord;
        }
        public static bool IsWebsiteDataExist()
        {
            bool ReturnValue = false;
            if (GetSession(Session_Website_Data) != null)
            {
                ReturnValue = true;
            }
            return ReturnValue;
        }
        #endregion
        #region "Session Objects Helper"
        public static string GetCaptchaTextFromSession(string _type)
        {
            return (string)GetSession(_type + "_captcha_string");
        }
        #endregion
    }
}