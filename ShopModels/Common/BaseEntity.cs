using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopModels.Common
{
    public interface IBaseEntity
    {
        public Guid Id { get; set; }
    }
}
