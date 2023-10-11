using System;
using System.Collections.Generic;

namespace MichaelScottsPaperCompany.UI.MVC.Models
{
    public partial class OrderItem
    {
        public int OrderItemId { get; set; }
        public int SaleId { get; set; }
        public int ItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal? ItemPrice { get; set; }

        public virtual Item Item { get; set; } = null!;
        public virtual Sale Sale { get; set; } = null!;
    }
}
