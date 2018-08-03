using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ProductCatalog.Models
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "Code is required.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public virtual  byte[] Photo { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public decimal? Price { get; set; }

        public DateTime LastUpdated { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }

    public enum HTTPMODE
    {
        GET,
        PUT,
        POST,
        DELETE,
        GETALL
    }

}
