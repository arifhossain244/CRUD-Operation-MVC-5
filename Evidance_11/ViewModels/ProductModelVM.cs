using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Evidance_11.ViewModels
{
    public class ProductCreateModel
    {
        public int ProductId { get; set; }
        [Required, StringLength(40), Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal Price { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Sales Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime SalesDate { get; set; }
        [Required]
        public IFormFile Picture { get; set; }
        public bool Discontinued { get; set; }
        [Required, ForeignKey("Customer")]
        public int CustomerId { get; set; }
    }
    public class ProductEditModel
    {
        public int ProductId { get; set; }
        [Required, StringLength(40), Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal Price { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Sales Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime SalesDate { get; set; }
        public IFormFile Picture { get; set; }
        public bool Discontinued { get; set; }
        [Required, ForeignKey("Customer")]
        public int CustomerId { get; set; }
    }
}
