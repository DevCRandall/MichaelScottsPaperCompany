using System;
using System.Collections.Generic;

namespace MichaelScottsPaperCompany.DATA.EF.Models
{
    public partial class Item
    {
        public Item()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public string? ItemDescription { get; set; }
        public int ItemsInStock { get; set; }
        public int? CategoryId { get; set; }
        public int? ManufacturerId { get; set; }
        public bool? InStock { get; set; }
        public DateTime? LastOrderedDate { get; set; }
        public string? ItemImage { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Manufacturer? Manufacturer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
