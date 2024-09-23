using Api.src.Favorite.domain.entity;
using Api.src.Review.domain.entity;
using Api.src.Session.domain.entity;
using Api.src.User.domain.enums;
using backend.src.User.domain.enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace backend.src.User.domain.entity
{
    public class UserEntity
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters"), Column(TypeName = "VARCHAR"), Required]
        public string Name { get; set; }


        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(50, ErrorMessage = "Email cannot be longer than 50 characters"), Column(TypeName = "VARCHAR")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "UserName cannot be longer than 50 characters"), Column(TypeName = "VARCHAR"), Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public UserStatus Status { get; set; }

        [Required, Column(TypeName = "datetime2(7)")]
        public DateTime CreatedAt { get; set; }
        [Required]
        public SessionEntity Session { get; set; }
        public ICollection<CartEntity> Carts { get; set; }
        public ICollection<ReviewEntity> Reviews { get; set; }
        public ICollection<FavoriteEntity> Favorites { get; set; }

    }
}