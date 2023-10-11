using System;
using System.Collections.Generic;

namespace MichaelScottsPaperCompany.UI.MVC.Models
{
    public partial class Sale
    {
        public Sale()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public string ShipToName { get; set; } = null!;
        public string ShipCity { get; set; } = null!;
        public string ShipState { get; set; } = null!;
        public string ShipZip { get; set; } = null!;
        public string UserId { get; set; } = null!;

        public virtual UserDetail User { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
