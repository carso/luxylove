﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace LLO.BookingLib
{
    public partial class ProductAttribute
    {
        public ProductAttribute()
        {
            ProductProductAttributeMappings = new HashSet<ProductProductAttributeMapping>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ProductProductAttributeMapping> ProductProductAttributeMappings { get; set; }
    }
}