using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Infrastructure.EntityFrameworkCore.Extensions;
using Xunit;

namespace NovelWorld.Infrastructure.EntityFramework.FunctionalTests
{
    public class IQueryableTest
    {
        [Fact]
        public void TestGetInclude()
        {
            IQueryable<TestEntity> query = new EnumerableQuery<TestEntity>(new List<TestEntity>());
            Expression<Func<TestEntity, object>> includes;
            includes = x => x.Other;

            query = query.Includes(includes);

            query.Include(x => x.Inner)
                .ThenInclude(x => x.Child)
                .Include(x => x.Other);

            Assert.Equal(1,1);
        }
        
        [Fact]
        public void TestGetIncludeWithNew()
        {
            IQueryable<TestEntity> query = new EnumerableQuery<TestEntity>(new List<TestEntity>());
            Expression<Func<TestEntity, object>> includes;
            includes = x => new
            {
                x.Other,
                x.Inner.Child
            };

            query = query.Includes(includes);

            query.Include(x => x.Inner)
                .ThenInclude(x => x.Child)
                .Include(x => x.Other);

            Assert.Equal(1,1);
        }
    }
}