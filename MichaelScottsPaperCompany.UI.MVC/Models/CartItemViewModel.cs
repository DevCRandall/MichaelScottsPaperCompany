using MichaelScottsPaperCompany.DATA.EF.Models;

namespace MichaelScottsPaperCompany.UI.MVC.Models
{
    public class CartItemViewModel
    {
        public int Qty { get; set; }

        public Item Item { get; set; }

        // The above is an example of a concept called "Containment":
        // We are using a complex datatype as a field/property in a class.
        // A complex datatype is any class with multiple properties. DateTime, Product
        // A primitive datatype stores a single value. example: bool, int, short

        // CTRL + . => Generate constructor ... => Select props => OK
        public CartItemViewModel(int qty, Item item)
        {
            Qty = qty;
            Item = item;
        }

    }
}

