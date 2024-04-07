﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models;

public class AppRating
{
    [Key]
    public Guid UserId { get; set; }
    public virtual User User { get; set; }

    [Required]
    public int Value { get; set; }
}