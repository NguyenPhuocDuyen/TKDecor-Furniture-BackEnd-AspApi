﻿using System.ComponentModel.DataAnnotations;

namespace BE_TKDecor.Core.Dtos.User
{
    public class UserSetRoleDto
    {
        public Guid UserId { get; set; }

        [RegularExpression($"^(Customer|Admin)$")]
        public string Role { get; set; } = null!;
    }
}
