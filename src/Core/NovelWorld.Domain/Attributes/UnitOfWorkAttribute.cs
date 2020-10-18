using System;

namespace NovelWorld.Domain.Attributes
{
    public class UnitOfWorkAttribute: Attribute
    {
        public bool IsTransaction { get; set; }

        public UnitOfWorkAttribute()
        {
            IsTransaction = true;
        }

        public UnitOfWorkAttribute(bool isTransaction)
        {
            IsTransaction = isTransaction;
        }
    }
}
