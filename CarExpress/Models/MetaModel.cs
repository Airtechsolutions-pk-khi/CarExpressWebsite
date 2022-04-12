using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace CarExpress.Models
{
    public class ShopDataModel
    {
        public List<ProductModel> ProductRecords { get; set; }
        public int PaginationCount { get; set; }
        public int CurrentPageNumber { get; set; }
    }
    public class CheckoutModel
    {
        [Required(ErrorMessage = "Required")]
        public string SenderName { get; set; }
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string SenderEmailAddress { get; set; }
        [Required(ErrorMessage = "Required")]
        public string SenderMobileNumber { get; set; }
        [Required(ErrorMessage = "Required")]
        public string RecipientName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Required")]
        public string RecipientMobileNumber { get; set; }
        [Required(ErrorMessage = "Required")]
        public string DeliveryDate { get; set; }
        [Required(ErrorMessage = "Required")]
        public string DeliveryTime { get; set; }
        [Required(ErrorMessage = "Required")]
        public string RecipientAddress { get; set; }
        [Required(ErrorMessage = "Required")]
        public string PlaceType { get; set; }
        public string NearestPlace { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Notes { get; set; }
    }
    public class CouponModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public decimal? Amount { get; set; }
    }
    public class WebsiteModel
    {
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public string WebsiteURL { get; set; }
        public string WebsiteImageURL { get; set; }
        public string CurrencySymbol { get; set; }
        public int StatusID { get; set; }
        public decimal? TaxPercentage { get; set; }
        public string Currency { get; set; }
        public double DeliveryCharges { get; set; }
        public double MinimumOrderAmount { get; set; }
    }
    public class CategoryModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
    public class ProductReviewModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Contact { get; set; }
        public int? Star { get; set; }
    }
    public class ProductMaxPriceModel
    {
        public double? Price { get; set; }
    }
 
    public class ProductModel
    {
        public int ID { get; set; }
        public string SubCategory { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public int? DisplayOrder { get; set; }
        public double? Price { get; set; }
        public double? OriginalPrice { get; set; }
        public double? Cost { get; set; }
        public string ItemType { get; set; }
        public double? StarRating { get; set; }
        public List<ProductImageModel> ItemImages { get; set; }
        public List<ProductReviewModel> Reviews { get; set; }
    }
    public class SubscribeModel
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }
    }
    public class ContactModel
    {
        [Required(ErrorMessage = "Required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Message { get; set; }
    }
    public class GiftModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public int? DisplayOrder { get; set; }
        public decimal? Price { get; set; }
        public string Type { get; set; }
    }
    public class ProductImageModel
    {
        public int ID { get; set; }
        public string Image { get; set; }
    }
    public class ShopCategoryModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class BreadCrumbMenu
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string AccessURL { get; set; }
    }
    public class LoginModel
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
    }
    public class RegisterModel
    {
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Address { get; set; }
    }
    public class ForgotModel
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }
    }
}