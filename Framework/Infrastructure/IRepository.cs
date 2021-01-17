using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public interface IRepository<T> where T : BaseEntity
    {
        IUnitOfWork UnitOfWrok { get; }
    }
}
