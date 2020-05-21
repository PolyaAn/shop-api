using System;
using ShopModels.Common;

namespace ShopModels.ProductsClasses
{
    public interface IProduct
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
    }
}