using T02_WebMarket;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using NUnit.Framework.Legacy;

namespace T02_Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            Cart cart1 = new Cart();

            string str = cart1.Order().Paylink;

            Assert.That(str, Is.Not.Empty);
        } 
    }
}
