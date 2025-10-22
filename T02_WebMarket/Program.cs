namespace T02_WebMarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region TEST
            Cart cart1 = new Cart();
            Console.WriteLine($"{cart1.Order().Paylink}");
            Console.ReadKey();
            #endregion

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

    public class Cart : IOrderable
    {
        private string _storeAdressURL;

        private readonly List<Cell> _cells;

        public Cart()
        {
            _storeAdressURL = "https://online-store.ru/Paylink?"; ;
            _cells = new List<Cell>();
        }

        public string Paylink => CreatePaylink(_storeAdressURL);

        public void Add(Good good, int amount)
        {
            Console.WriteLine($"{nameof(Add)}  - Метод не реализован!");
        }

        public IOrderable Order()
        {
            return this;
        }

        public void ShowGoods()
        {
            int index = 0;

            Console.WriteLine($"На складе имеются следующие товары:");

            foreach (var cell in _cells)
            {
                Console.WriteLine($"{++index}: {cell.Good.Name} - [{cell.Amount}] шт.");
            }
        }

        private string CreatePaylink(string storeAdressURL)
        {
            Random random = new Random();
            int randomValue;
            int minValue = 0;
            int maxValue = 26;
            int staticValue = 65;
            int payStringLenth = 20;
            string generatedString = String.Empty;
            char letter;

            for (int i = 0; i < payStringLenth; i++)
            {
                randomValue = random.Next(minValue, maxValue);
                letter = Convert.ToChar(randomValue + staticValue);

                generatedString += letter;
            }

            return storeAdressURL + generatedString;
        }
    }

    public interface IOrderable
    {
        public string Paylink { get; }
    }

    public class Cell
    {
        public Cell(Good good, int amount)
        {
            Good = good;
            Amount = amount;
        }

        public Good Good { get; private set; }

        public int Amount { get; private set; }
    }

    public class Shop
    {
        private Warehouse _warehouse;
        private Cart _cart;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public Cart Cart()
        {
            _cart = new Cart();
            return _cart;
        }
    }

    public class Warehouse
    {
        private readonly List<Cell> _cells;

        public Warehouse()
        {
            _cells = new();
        }

        public void Delive(Good good, int amount)
        {
            Cell cell = new Cell(good, amount);

            if (IsCellContainsGood(cell))
            {
                MergeCell(cell);
            }
            else
            {
                _cells.Add(cell);
            }
        }

        private void MergeCell(Cell newCell)
        {
            Cell cell = _cells.First(cell => cell.Good.Name == newCell.Good.Name);
            cell = new Cell(cell.Good, cell.Amount + newCell.Amount);
        }

        private bool IsCellContainsGood(Cell validateCell)
        {
            return _cells.Where(cell => cell.Good.Name == validateCell.Good.Name).Count() > 0;
        }

        public void ShowAll()
        {
            int index = 0;

            Console.WriteLine($"На складе имеются следующие товары:");

            foreach (var cell in _cells)
            {
                Console.WriteLine($"{++index}: {cell.Good.Name} - [{cell.Amount}] шт.");
            }
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