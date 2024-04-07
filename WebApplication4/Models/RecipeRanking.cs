﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models;

public class RecipeRanking
{
    [Key]
    public Guid RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }

    [Required]
    public int Points { get; set; }
}