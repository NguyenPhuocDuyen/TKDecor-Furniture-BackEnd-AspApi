﻿using System.ComponentModel.DataAnnotations;

namespace BE_TKDecor.Core.Dtos.Category
{
    public class CategoryUpdateDto
    {
        public Guid CategoryId { get; set; }

        [MinLength(5)]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public string Thumbnail { get; set; } = null!;
    }
}
