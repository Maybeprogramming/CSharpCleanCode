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
        public void FirstRunTest()
        {
            Assert.Pass();
        }

        [Test]
        public void TestPaylink()
        {
            List<Cell> cells = new List<Cell>();
            Cart cart1 = new Cart(new GoodsValidator(cells));

            string str = cart1.GetOrder().Paylink;
            string strStatic = "https://online-store.ru/Paylink/";

            Assert.That(str.Contains(strStatic));
        } 
    }
}
