using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MichaelScottsPaperCompany.DATA.EF.Models;
using MichaelScottsPaperCompany.Utilities;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;

namespace MichaelScottsPaperCompany.UI.MVC.Controllers
{
    public class ItemsController : Controller
    {
        private readonly MichaelScottsPaperCompanyContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemsController(MichaelScottsPaperCompanyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var michaelScottsPaperCompanyContext = _context.Items.Include(i => i.Category).Include(i => i.Manufacturer);
            return View(await michaelScottsPaperCompanyContext.ToListAsync());
        }

        public async Task<IActionResult> Tiled()
        {
            var gadgetStoreContext = _context.Items.Include(p => p.Category).Include(p => p.Manufacturer);
            return View(await gadgetStoreContext.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.Manufacturer)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerId");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,ItemName,ItemPrice,ItemDescription,ItemsInStock,CategoryId,ManufacturerId,InStock,LastOrderedDate,ItemImage, Image")] Item item)
        {
            if (ModelState.IsValid)
            {
                if (item.Image != null)
                {
                    //Find out what file extension this is... what is the file type
                    // Because we only want image types
                    string Ext = Path.GetExtension(item.Image.FileName);

                    // Create a list of the valid image extensions
                    // These are found in System.Drawing.Imaging.ImageFormat namespace
                    string[] ValidImageExtension = { ".jpeg", ".jpg", ".gif", ".png", ".tiff", ".bmp" };

                    // Verify the uploaded file has an allowed extension
                    // AND verify that it is in the .NET 4mb limit
                    if (ValidImageExtension.Contains(Ext.ToLower()) && item.Image.Length < 4_194_303)
                    {
                        // Generate a unique file name
                        // You could also use Path.GetRandomFileName() if you don't want GUIDs
                        item.ItemImage = Guid.NewGuid() + Ext;

                        // Save the file to the webserver (wwwroot/images)
                        // Suggestion: Make a different directory than the site Images
                        string WebRootPath = _webHostEnvironment.WebRootPath;
                        //string FullImagePath = WebRootPath + "/images/";


                        string FullImagePath = Path.Combine(WebRootPath, "assets/img/upload/");

                        //Create a MemoryStream to read the image into server memory
                        using (var MemStream = new MemoryStream())
                        {
                            // Transfer the file into the memory stream
                            await item.Image.CopyToAsync(MemStream);
                            using (var img = Image.FromStream(MemStream))
                            {
                                // Now send this image to the ImageUtility for resizing
                                // and thumbnail creation
                                // Parameters Needed:
                                // 1 (int) Max Image Size
                                // 2 (int) Max Thumbnail Size
                                // 3 (string) Full Destination File Path for Image
                                // 4 (Image) an image (we put it in the MemStream)
                                // 5 (string) FileName
                                int MaxImageSize = 500; // Pixels
                                int MaxThumbSize = 100;

                                ImageUtility.ResizeImage(FullImagePath, item.ItemImage, img, MaxImageSize, MaxThumbSize);

                            }
                        }
                    }
                }

                else
                {
                    item.ItemImage = "noimage.png";
                }

                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", item.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerId", item.ManufacturerId);
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", item.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerId", item.ManufacturerId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,ItemName,ItemPrice,ItemDescription,ItemsInStock,CategoryId,ManufacturerId,InStock,LastOrderedDate,ItemImage,Image")] Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                string oldImageName = item.ItemImage;

                // Check... Did user upload a file?
                if (item.Image != null)
                {
                    //Find out what file extension this is... what is the file type
                    // Because we only want image types
                    string Ext = Path.GetExtension(item.Image.FileName);

                    // Create a list of the valid image extensions
                    // These are found in System.Drawing.Imaging.ImageFormat namespace
                    string[] ValidImageExtension = { ".jpeg", ".jpg", ".gif", ".png", ".tiff", ".bmp" };

                    // Verify the uploaded file has an allowed extension
                    // AND verify that it is in the .NET 4mb limit
                    if (ValidImageExtension.Contains(Ext.ToLower()) && item.Image.Length < 4_194_303)
                    {
                        // Generate a unique file name
                        // You could also use Path.GetRandomFileName() if you don't want GUIDs
                        item.ItemImage = Guid.NewGuid() + Ext;

                        // Save the file to the webserver (wwwroot/images)
                        // Suggestion: Make a different directory than the site Images
                        string WebRootPath = _webHostEnvironment.WebRootPath;
                        //string FullImagePath = WebRootPath + "/images/";


                        string FullImagePath = Path.Combine(WebRootPath, "assets/img/upload/");

                        // If the old image was NOT NoImage.png... we don't want to delete the noImage.png
                        if (oldImageName != "noimage.png")
                        {
                            ImageUtility.Delete(FullImagePath, oldImageName);
                        }
                        //Create a MemoryStream to read the image into server memory
                        using (var MemStream = new MemoryStream())
                        {
                            // Transfer the file into the memory stream
                            await item.Image.CopyToAsync(MemStream);
                            using (var img = Image.FromStream(MemStream))
                            {
                                // Now send this image to the ImageUtility for resizing
                                // and thumbnail creation
                                // Parameters Needed:
                                // 1 (int) Max Image Size
                                // 2 (int) Max Thumbnail Size
                                // 3 (string) Full Destination File Path for Image
                                // 4 (Image) an image (we put it in the MemStream)
                                // 5 (string) FileName
                                int MaxImageSize = 500; // Pixels
                                int MaxThumbSize = 100;

                                ImageUtility.ResizeImage(FullImagePath, item.ItemImage, img, MaxImageSize, MaxThumbSize);

                            }
                        }
                    }
                }

                else
                {
                    //If no image was uploaded, assign a default filename
                    //Will also need to download a default image and name it 'noimage.png' -> copy it to the /images folder
                    item.ItemImage = "noimage.png";
                }

                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", item.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerId", item.ManufacturerId);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.Manufacturer)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'MichaelScottsPaperCompanyContext.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return (_context.Items?.Any(e => e.ItemId == id)).GetValueOrDefault();
        }
    }
}
