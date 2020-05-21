using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ShopModels.Common;
using ShopModels.ProductsClasses;

namespace ShopModels.CustomerClasses
{
    [Table("Customers")]
    public class Customer : IBaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Guid Id { get; set; }
        public string Login { get; set; }
        public int OrderPrice { get; set; }
        public virtual  ICollection<Product> Products { get; set; }

        public Customer() { }
    }
}
