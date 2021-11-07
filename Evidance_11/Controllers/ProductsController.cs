using Evidance_11.Models;
using Evidance_11.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Evidance_11.Controllers
{
    public class ProductsController : Controller
    {
        readonly ProductDbContext db = null;
        private readonly IWebHostEnvironment env;
        public ProductsController(ProductDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public IActionResult Index()
        {
            return View(db.Products.Include(x=> x.Customer).ToList());
        }
        public IActionResult Create()
        {
            ViewBag.Customers = db.Customers.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductCreateModel p)
        {
            if (ModelState.IsValid)
            {
                var ProNew  = new Product
                {
                    Picture = "on2.jpg",
                    ProductName = p.ProductName,
                    Price = p.Price,
                    SalesDate = p.SalesDate,
                    Discontinued = p.Discontinued,
                    CustomerId = p.CustomerId
                    
                };
                if (p.Picture != null && p.Picture.Length > 0)
                {
                    string dir = Path.Combine(env.WebRootPath, "Uploads");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string fileName = Guid.NewGuid() + Path.GetExtension(p.Picture.FileName);
                    string fullPath = Path.Combine(dir, fileName);
                    FileStream fs = new FileStream(fullPath, FileMode.Create);
                    p.Picture.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                    ProNew.Picture = fileName;
                }
                db.Products.Add(ProNew);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Customers = db.Customers.ToList();
            return View(p);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Customers = db.Customers.ToList();
            var pro = db.Products.First(p => p.ProductId == id);
            ViewBag.CurrentPic = pro.Picture;
            return View(new ProductEditModel
            {
                ProductId = pro.ProductId,
                ProductName = pro.ProductName,
                Price = pro.Price,
                SalesDate = pro.SalesDate,
                Discontinued = pro.Discontinued,
                CustomerId = pro.CustomerId
            });
        }
        [HttpPost]
        public IActionResult Edit(ProductEditModel p)
        {
            var Pro = db.Products.First(p => p.ProductId == p.ProductId);
            if (ModelState.IsValid)
            {
                Pro.ProductName = p.ProductName;
                Pro.Price = p.Price;
                Pro.SalesDate = p.SalesDate;
                Pro.Discontinued = p.Discontinued;
                Pro.CustomerId = p.CustomerId;
                if(p.Picture !=null && Pro.Picture.Length > 0)
                {
                    string dir = Path.Combine(env.WebRootPath, "Uploads");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string fileName = Guid.NewGuid() + Path.GetExtension(p.Picture.FileName);
                    string fullPath = Path.Combine(dir, fileName);
                    FileStream fs = new FileStream(fullPath, FileMode.Create);
                    p.Picture.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                    Pro.Picture = fileName;
                }
                db.SaveChanges();
                return RedirectToAction("Index"); 
            }
            ViewBag.Customers = db.Customers.ToList();
            ViewBag.CurrentPic = Pro.Picture;
            return View(p);
        }
        public IActionResult Delete(int id)
        {
            return View(db.Products.Include(c => c.Customer).First(p => p.ProductId == id));
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DoDelete(int id)
        {
            var Product = new Product { ProductId = id };
            db.Entry(Product).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
