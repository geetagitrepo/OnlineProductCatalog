using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductCatalog.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public byte[] Photo { get; set; } = Array.Empty<byte>();

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }        
        public DateTime LastUpdated { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }

}
