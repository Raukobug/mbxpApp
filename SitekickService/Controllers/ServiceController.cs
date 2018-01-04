using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Classic.PdfCreator;
using Serilog;
using System.IO;
using System.Globalization;
using System.Threading;

namespace SitekickService.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ServiceController : Controller
    {
        [HttpPost]
        public Product CheckCode([FromBody] Image image)
        {
            Log.Information("Someone is useing the api!");
            var product = new Product();
            if (image != null)
            {
                Barcode barcode = new Barcode(image.Bytes);
                Log.Information("Trying to access sitekick");
                try
                {
                    using (var sitekick = new SitekickDataContext("Connection information removed for security sake"))
                    {
                        var sitekickProduct = sitekick.Products.Where(x => barcode.Code.Substring(0, x.Prefix.Length) == x.Prefix).FirstOrDefault();
                        if (sitekickProduct != null)
                        {
                            Random ran = new Random();
                            DateTime now = DateTime.Now.AddDays(ran.Next(-10,700));
                            CultureInfo culture = new CultureInfo("da");
                            Thread.CurrentThread.CurrentCulture = culture;

                            Log.Information("Product found: " + sitekickProduct.ProductBase.Presentation.Name);
                            product.Code = barcode.Code;
                            product.Name = sitekickProduct.ProductBase.Presentation.Name;
                            product.Description = sitekickProduct.ProductBase.Presentation.Description;
                            product.ImageInBytes = barcode.CreateBarcode();
                            product.Price = sitekickProduct.Price.ToString();
                            product.ExpirationDate = now.ToString("dd-MM-yyyy");
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Error: " + e.ToString());
                }

            }
            return product;
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExpirationDate { get; set; }
        public string Price { get; set; }
        public string Code { get; set; }
        public byte[] ImageInBytes { get; set; }
    }

    public class Image
    {
        public string Bytes { get; set; }
    }
}
