using Microsoft.AspNetCore.Mvc;
using MichaelScottsPaperCompany.DATA.EF.Models; // Access to the Context
using Microsoft.AspNetCore.Identity; // Access to the UserManager class
using MichaelScottsPaperCompany.UI.MVC.Models; // Access to the CartItemViewModel
using Newtonsoft.Json; // Easier management of the Shopping Cart

namespace MichaelScottsPaperCompany.UI.MVC.Controllers
{
    #region Steps to Implement Session Based Shopping Cart
    /*
     * 1) Register Session in program.cs (builder.Services.AddSession... && app.UseSession())
     * 2) Create the CartItemViewModel class in [ProjName].UI.MVC/Models folder
     * 3) Add the 'Add To Cart' button in the Index and/or Details view of your Products
     * 4) Create the ShoppingCartController (empty controller -> named ShoppingCartController)
     *      - add using statements
     *          - using GadgetStore.DATA.EF.Models;
     *          - using Microsoft.AspNetCore.Identity;
     *          - using GadgetStore.UI.MVC.Models;
     *          - using Newtonsoft.Json;
     *      - Add props for the GadgetStoreContext && UserManager
     *      - Create a constructor for the controller - assign values to context && usermanager
     *      - Code the AddToCart() action
     *      - Code the Index() action
     *      - Code the Index View
     *          - Start with the basic table structure
     *          - Show the items that are easily accessible (like the properties from the model)
     *          - Calculate/show the lineTotal
     *          - Add the RemoveFromCart <a>
     *      - Code the RemoveFromCart() action
     *          - verify the button for RemoveFromCart in the Index view is coded with the controller/action/id
     *      - Add UpdateCart <form> to the Index View
     *      - Code the UpdateCart() action
     *      - Add Submit Order button to Index View
     *      - Code SubmitOrder() action
     * */
    #endregion

    public class ShoppingCartController : Controller
    {
        //FIELDS
        private readonly MichaelScottsPaperCompanyContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        //Constructor
        public ShoppingCartController
            (MichaelScottsPaperCompanyContext context,
            UserManager<IdentityUser> userManager)// Dependency Injection (DI)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            // Retrieve the contents of the Session Cart

            // Get the string stored in the Session:
            var sessionCart = HttpContext.Session.GetString("cart");

            // Create the Local Cart:
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            if (sessionCart == null || sessionCart.Count() == 0)
            {
                // If the Session Cart is empty:
                //Create an empty cart
                shoppingCart = new Dictionary<int, CartItemViewModel>();

                //Notify the user:
                ViewBag.Message = "There are no items in your cart.";
            }
            else
            {
                // If the Session Cart has items:
                // Reset the Message
                ViewBag.Message = null;

                // Deserialize the Session Cart
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            return View(shoppingCart);
        }

        public IActionResult AddToCart(int id)
        {
            // Create an empty shell for a LOCAL Shopping Cart object.
            // Our shopping cart is a dictionary <TKey, TValue>
            // Where the Key is the Product ID (int)
            // and the Value is the CartItemViewModel (Qty & Product)
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            #region Session Notes
            /*
             * Session is a tool available on the server-side that can store information for a user while they are actively using your site.
             * Typically the session lasts for 20 minutes (this can be adjusted in Program.cs).
             * Once the 20 minutes is up, the session variable is disposed.
             * 
             * Values that we can store in Session are limited to: string, int
             * - Because of this we have to get creative when trying to store complex objects (like CartItemViewModel).
             * To keep the info separated into properties we will convert the C# object to a JSON string.
             * */
            #endregion

            var sessionCart = HttpContext.Session.GetString("cart");

            // Check to see if the session object exists
            // If not, instantiate a new collection.
            if (String.IsNullOrEmpty(sessionCart))
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                // If the Cart already exists:
                // Transfer the existing Cart items from the session to our local shopping cart
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            // Add the newly selected Product(s) to the Cart

            // Retrieve the Product details from the Database:
            Item item = _context.Items.Find(id);

            // Instantiate the object to add to the Cart:
            var cartItem = new CartItemViewModel(1, item);

            if (shoppingCart.ContainsKey(item.ItemId))
            {
                // If the Product is already in the cart:
                // Add one to the Quantity.
                shoppingCart[item.ItemId].Qty++;
            }
            else
            {
                // If the Product is NOT in the Cart:
                // Add the Product.
                shoppingCart.Add(item.ItemId, cartItem);
            }

            // Update the Session Cart: The above Cart is the LOCAL Cart

            // Stringify the Local Cart
            string jsonLocalCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonLocalCart);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var sessionCart = HttpContext.Session.GetString("cart");

            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            shoppingCart.Remove(id);

            if (shoppingCart.Count == 0)
            {
                // If the are no more items in the cart:
                // Remove it from the session
                HttpContext.Session.Remove("cart");
            }
            else
            {
                // Otherwise:
                // Update the session
                var jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult UpdateCart(int productId, int qty)
        {
            var sessionCart = HttpContext.Session.GetString("cart");

            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            shoppingCart[productId].Qty = qty;

            var jsonCart = JsonConvert.SerializeObject(shoppingCart);

            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SubmitOrder()
        {
            #region Planning out Order Submission
            //BIG PICTURE PLAN
            //Create Order object -> then save to the DB
            //  - OrderDate
            //  - UserId
            //  - ShipToName, ShipCity, ShipState, ShipZip --> this info needs to be pulled from the UserDetails record
            //Add the record to _context
            //Save DB changes

            //Create OrderProducts object for each item in the cart
            //  - ProductId -> available from cart
            //  - OrderId -> from Order object
            //  - Qty -> available from cart
            //  - ProductPrice -> available from cart
            //Add the record to _context
            //Save DB changes
            #endregion

            var userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

            var userDetails = _context.UserDetails.Find(userId);

            var order = new Sale()
            {
                SaleDate = DateTime.Now,
                UserId = userId,
                ShipCity = userDetails.City,
                ShipToName = userDetails.FirstName + " " + userDetails.LastName,
                ShipState = userDetails.State,
                ShipZip = userDetails.Zip
            };

            _context.Sales.Add(order);

            var sessionCart = HttpContext.Session.GetString("cart");
            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            foreach (var item in shoppingCart)
            {
                OrderItem orderItem = new OrderItem()
                {
                    SaleId = order.SaleId,
                    ItemId = item.Key,
                    ItemPrice = item.Value.Item.ItemPrice,
                    Quantity = (short)item.Value.Qty
                };

                order.OrderItems.Add(orderItem);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Sales");
        }
    }
}
