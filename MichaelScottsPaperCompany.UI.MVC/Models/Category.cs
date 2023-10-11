using System;
using System.Collections.Generic;

namespace MichaelScottsPaperCompany.UI.MVC.Models
{
    public partial class Category
    {
        public Category()
        {
            Items = new HashSet<Item>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
