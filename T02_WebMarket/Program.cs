namespace T02_WebMarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cart cart1 = new Cart();
            Console.WriteLine($"{cart1.Order().Paylink}");
            Console.ReadKey();

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
        private string _store = "https://online-store.ru/Paylink?";

        public string Paylink => _store + GetRandomString();

        public void Add(Good good, int amount)
        {
            throw new NotImplementedException();
        }

        public Cart Order()
        {
            Console.WriteLine($"{nameof(Order)}  - Метод не реализован!");
            return this;
        }

        public void ShowGoods()
        {
            Console.WriteLine($"{nameof(ShowGoods)}  - Метод не реализован!");
        }

        private string GetRandomString()
        {
            Random random = new Random();
            int randomNumber;
            int minNumber = 0;
            int maxNumber = 26;
            int staticNumber = 65;
            int stringLenth = 20;
            string generatedString = "";

            for (int i = 0; i < stringLenth; i++)
            {
                randomNumber = random.Next(minNumber, maxNumber);
                char letter = Convert.ToChar(randomNumber + staticNumber);

                generatedString += letter;
            }

            return generatedString;
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
            Console.WriteLine($"{nameof(Cart)}  - Метод не реализован!");
            return new Cart();
        }
    }

    public class Warehouse
    {
        public void Delive(Good good, int amount)
        {
            Console.WriteLine($"{nameof(Delive)} - Метод не реализован!");
        }

        public void ShowAll()
        {
            Console.WriteLine($"{nameof(ShowAll)}  - Метод не реализован!");
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