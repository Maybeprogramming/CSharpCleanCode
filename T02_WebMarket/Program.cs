
namespace T02_WebMarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            //Вывод всех товаров на складе с их остатком
            warehouse.ShowAll();

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            //Вывод всех товаров в корзине
            cart.ShowGoods();

            Console.WriteLine(cart.Order().Paylink);

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
        }
    }

    public class Cart
    {
        public string Paylink { get; private set; }

        internal void Add(Good good, int amount)
        {
            throw new NotImplementedException();
        }

        internal Cart Order()
        {
            throw new NotImplementedException();
        }

        internal void ShowGoods()
        {
            throw new NotImplementedException();
        }
    }

    public class Shop
    {
        private Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public Cart Cart()
        {
            throw new NotImplementedException();
        }
    }

    public class Warehouse
    {
        public void Delive(Good good, int amount)
        {
            throw new NotImplementedException();
        }

        internal void ShowAll()
        {
            throw new NotImplementedException();
        }
    }

    public class Good
    {
        public Good(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}