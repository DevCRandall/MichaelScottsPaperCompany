using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MichaelScottsPaperCompany.DATA.EF.Models //Metadata
{
    //public class Metadata
    //{
    //}

    public class CategoryMetadata
    {
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        [StringLength(50, ErrorMessage = "Cannot exceed 50 characters.")]
        public string? CategoryName { get; set; }

        [Display(Name = "Description")]
        [StringLength(1000, ErrorMessage = "Cannot exceed 1000 characters.")]
        [DataType(DataType.MultilineText)]
        public string? CategoryDescription { get; set; }
    }

    public class ItemMetadata
    {
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Item Name Required")]
        [Display(Name = "Item Name")]
        [StringLength(150, ErrorMessage = "Cannot exceed 150 characters")]
        public string ItemName { get; set; } = null!;

        [Required(ErrorMessage = "Item Price Required")]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal ItemPrice { get; set; }

        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Cannot exceed 500 characters")]
        public string? ItemDescription { get; set; }

        [Required(ErrorMessage = "Number In Stock Required")]
        [Display(Name = "# in Stock")]
        public int ItemsInStock { get; set; }

        public int? CategoryId { get; set; }

        public int? ManufacturerId { get; set; }

        [Display(Name = "Is In Stock")]
        public bool? InStock { get; set; }

        [Display(Name = "Last Ordered")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? LastOrderedDate { get; set; }

        [Display(Name = "Image")]
        public string? ItemImage { get; set; }
    }

    public class ManufacturerMetadata
    {
        public int ManufacturerId { get; set; }

        [Display(Name = "Manufacturer")]
        [StringLength(200, ErrorMessage = "Cannot exceed 200 characters")]
        public string? ManufacturerName { get; set; }

        [Display(Name = "Address")]
        [StringLength(150, ErrorMessage = "Cannot exceed 150 characters")]
        public string? Address { get; set; }

        [Display(Name = "City")]
        [StringLength(50, ErrorMessage = "Cannot exceed 50 characters")]
        public string? City { get; set; }

        [Display(Name = "State")]
        [StringLength(2, ErrorMessage = "Cannot exceed 2 characters")]
        public string? State { get; set; }

        [Display(Name = "ZIP")]
        [StringLength(5)]
        [DataType(DataType.PostalCode)]
        public string? Zip { get; set; }
    }

    public class OrderItemMetadata
    {
        public int OrderItemId { get; set; }


        public int SaleId { get; set; }


        public int ItemId { get; set; }

        [Display(Name = "Quantity")]
        public int? Quantity { get; set; }

        [Display(Name = "Manufacturer")]
        [DataType(DataType.Currency)]
        public decimal? ItemPrice { get; set; }
    }

    public class SaleMetadata
    {
        public int SaleId { get; set; }

        [Required]
        [Display(Name = "Order Date")]
        public DateTime SaleDate { get; set; }

        [Required]
        [Display(Name = "Ship To Address")]
        [StringLength(50, ErrorMessage = "Cannot exceed 50 characters")]
        public string ShipToName { get; set; } = null!;

        [Required]
        [Display(Name = "City")]
        [StringLength(50, ErrorMessage = "Cannot exceed 50 characters")]
        public string ShipCity { get; set; } = null!;

        [Required]
        [Display(Name = "State")]
        [StringLength(2, ErrorMessage = "Cannot exceed 2 characters")]
        public string ShipState { get; set; } = null!;

        [Required]
        [Display(Name = "ZIP")]
        [DataType(DataType.PostalCode)]
        public string ShipZip { get; set; } = null!;


        public string UserId { get; set; } = null!;
    }

    public class UserDetailMetadata
    {
        public string UserId { get; set; } = null!;

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Cannot exceed 50 characters")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Cannot exceed 50 characters")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Address")]
        [StringLength(150, ErrorMessage = "Cannot exceed 150 characters")]
        public string? Address { get; set; }

        [Display(Name = "City")]
        [StringLength(50, ErrorMessage = "Cannot exceed 50 characters")]
        public string? City { get; set; }

        [Display(Name = "State")]
        [StringLength(2, ErrorMessage = "Cannot exceed 2 characters")]
        public string? State { get; set; }

        [Display(Name = "ZIP")]
        [DataType(DataType.PostalCode)]
        public string? Zip { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }


        public string? Phone { get; set; }
    }
}


