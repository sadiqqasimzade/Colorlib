using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ColorLibBack.DAL;
using ColorLibBack.Models;
using ColorLibBack.Utilities;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace ColorLibBack.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Products.Include(p => p.Category);
            return View(await appDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                return View(product);
            }
            if (product.File == null)
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                return View(product);
            }
            if (!product.File.ContentType.Contains("image"))
            {
                ModelState.AddModelError("", "Wrong file type");
                ViewData["CategoryId"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                return View(product);
            }
            if (product.File.Length / 1024 > Consts.ProductImgMaxSizeKb)
            {
                ModelState.AddModelError("", "File size cant be more than:" + Consts.ProductImgMaxSizeKb + "KB");
                ViewData["CategoryId"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                return View(product);
            }

            string filename = Guid.NewGuid().ToString() + product.File.FileName;
            if (filename.Length > Consts.ProductImgNameMaxLength)
                filename.Substring(filename.Length - Consts.ProductImgNameMaxLength, Consts.ProductImgNameMaxLength);

            using (FileStream fs = new FileStream(Path.Combine(Consts.ProductImgPath, filename), FileMode.Create))
                await product.File.CopyToAsync(fs);

            product.Img = filename;
            product.Name = product.Name.Trim();
            product.Desc = product.Desc.Trim();


            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
                return NotFound();


            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            ViewData["CategoryId"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Product product)
        {
            if (ModelState.IsValid)
            {
                Product dbproduct = await _context.Products.FindAsync(product.Id);
                if (product == null) return View();
                dbproduct.Name = product.Name.Trim();
                dbproduct.Desc = product.Desc.Trim();
                dbproduct.CategoryId = product.CategoryId;
                dbproduct.IsDeleted = product.IsDeleted;
                dbproduct.Price = product.Price;

                if(product.File!=null&& product.File.ContentType.Contains("image")&& product.File.Length / 1024 < Consts.ProductImgMaxSizeKb)
                {
                    if (System.IO.File.Exists(Path.Combine(Consts.ProductImgPath, dbproduct.Img)))
                        System.IO.File.Delete(Path.Combine(Consts.ProductImgPath, dbproduct.Img));

                    string filename = Guid.NewGuid().ToString() + product.File.FileName;
                    if (filename.Length > Consts.ProductImgNameMaxLength)
                        filename.Substring(filename.Length - Consts.ProductImgNameMaxLength, Consts.ProductImgNameMaxLength);

                    using (FileStream fs = new FileStream(Path.Combine(Consts.ProductImgPath, filename), FileMode.Create))
                        await product.File.CopyToAsync(fs);
                    dbproduct.Img = filename;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (System.IO.File.Exists(Path.Combine(Consts.ProductImgPath, product.Img)))
                System.IO.File.Delete(Path.Combine(Consts.ProductImgPath, product.Img));
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
