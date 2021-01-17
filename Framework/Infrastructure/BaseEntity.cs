using System;

namespace Infrastructure
{
    public abstract class BaseEntity
    {
        public int Key { get; set; }
        public Guid Id { get; set; }
    }

}
