﻿using BusinessObject;
using System.ComponentModel.DataAnnotations;

namespace BE_TKDecor.Core.Dtos.Product
{
    public class ProductUpdateDto
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        //public int? Product3DModelId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, 9999999999)]
        public decimal Price { get; set; }

        public List<string> ProductImages { get; set; } = new List<string>();
    }
}