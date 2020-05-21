using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ShopModels.Common;
using ShopModels.CustomerClasses;

namespace ShopModels.ProductsClasses
{
    [Table("Products")]
    public class Product : IProduct, IBaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public Product() { }

    }
}
