﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string ProductName { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string ProductDescription { get; set; }
        [Required]
        [Range(1, 999999)]
        public decimal ProductPrice { get; set; }
        [Required]
        public byte[] ProductImageData { get; set; }
        [DefaultValue(true)]
        public bool ProductStatus { get; set; } = true;
        [Required]
        [Column(TypeName = "nvarchar(5)")]
        public string ProductPriceSembol { get; set; }
        [Required]
        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}