﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Modals
{
    public class Category
    {
        [Key]
        public int Id {  get; set; }
        [Required]
        public string Title { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; } 

    }
}
