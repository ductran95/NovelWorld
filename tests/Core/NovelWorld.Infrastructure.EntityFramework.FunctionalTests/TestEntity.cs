using NovelWorld.Data.Entities;

namespace NovelWorld.Infrastructure.EntityFramework.FunctionalTests
{
    public class TestEntity: Entity
    {
        public string Name { get; set; }
        public TestInnerEntity Inner { get; set; }
        public TestOtherInnerEntity Other { get; set; }
    }
    
    public class TestInnerEntity: Entity
    {
        public string Description { get; set; }
        public TestInnerLevel2Entity Child { get; set; }
    }
    
    public class TestOtherInnerEntity: Entity
    {
        public string Description { get; set; }
    }
    
    public class TestInnerLevel2Entity: Entity
    {
        public string Name { get; set; }
    }
}