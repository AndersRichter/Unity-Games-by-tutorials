using NUnit.Framework;
using Test;

namespace Editor.EditorTests
{
    public class TestModelTest
    {
        private TestModel _testModel;

        [SetUp]
        public void BeforeEach()
        {
            _testModel = new TestModel(2, 3);
        }
        
        [TearDown]
        public void AfterEach() {}

        [Test]
        public void TestIncreaseValue()
        {
            _testModel.Increase();
            Assert.AreEqual(2, _testModel.GetValue());
            
            _testModel.Increase();
            Assert.AreEqual(4, _testModel.GetValue());
        }
        
        [Test]
        public void TestDecreaseValue()
        {
            _testModel.Decrease();
            Assert.AreEqual(-3, _testModel.GetValue());
            
            _testModel.Decrease();
            Assert.AreEqual(-6, _testModel.GetValue());
        }
    }
}