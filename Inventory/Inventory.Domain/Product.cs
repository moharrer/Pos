using Infrastructure;
using System;

namespace Inventory.Domain
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
