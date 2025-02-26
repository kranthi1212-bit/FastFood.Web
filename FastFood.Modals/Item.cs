﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Modals
{
    public class Item
    {
        public int Id {  get; set; }
        public string Title {  get; set; }  
        public string Description { get; set; }
        public string Image {  get; set; }
        public double Price {  get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; } 
        public int SubCategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public SubCategory SubCategory  { get; set; }
    }
}
