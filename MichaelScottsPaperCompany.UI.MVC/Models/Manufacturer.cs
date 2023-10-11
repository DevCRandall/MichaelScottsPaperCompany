using System;
using System.Collections.Generic;

namespace MichaelScottsPaperCompany.UI.MVC.Models
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Items = new HashSet<Item>();
        }

        public int ManufacturerId { get; set; }
        public string? ManufacturerName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
