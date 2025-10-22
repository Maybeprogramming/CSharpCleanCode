namespace T02_WebMarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(80, 25);  // Стандартный размер
            Console.SetBufferSize(80, 300); // Стандартный размер буфера

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

            Console.ReadKey();
        }
    }

    public class Cart : IOrderable
    {
        private string _storeAdressURL;

        private readonly List<Cell> _cells;

        public event Action Ordered;
        public event Action<Cell, Deliver> TryAdding;

        public Cart()
        {
            _storeAdressURL = "https://online-store.ru/Paylink/";
            _cells = new List<Cell>();
        }

        public string Paylink => Utils.GetRandomString(_storeAdressURL);

        public void Add(Good good, int amount)
        {
            Cell cell = new Cell(good, amount);
            Deliver deliver = new Deliver();

            TryAdding?.Invoke(cell, deliver);

            if (deliver.CanMove == false)
            {
                Utils.Print($"Ошибка - нет нужного количества товара на складе");
            }
            else
            {
                _cells.Add(cell);
            }
        }

        public IOrderable Order()
        {
            Ordered?.Invoke();
            return this;
        }

        public void ShowGoods()
        {
            Utils.PrintCollection(_cells, $"В корзине следующие товары:");
        }
    }

    public class Deliver
    {
        public bool CanMove { get; set; }
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

        public Good Good { get; }

        public int Amount { get; }
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
            _cart.TryAdding += OnTryAdding;
            _cart.Ordered += OnOrdered;

            return _cart;
        }

        private void OnOrdered()
        {
            _cart.TryAdding -= OnTryAdding;
            _cart.Ordered -= OnOrdered;

            //
        }

        private void OnTryAdding(Cell cellToCart, Deliver deliver)
        {
            deliver.CanMove = false;
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
            Utils.PrintCollection(_cells, $"На складе имеются следующие товары:");
        }
    }

    public class Good
    {
        public Good(string name) => Name = name;

        public string Name { get; }
    }

    public static class Utils
    {
        private static Random s_random = new Random();

        public static void Print(string text)
        {
            Console.WriteLine($"{text}");
        }

        public static void PrintCollection(IEnumerable<Cell> cells, string message)
        {
            int index = 0;

            Print(message);

            foreach (var cell in cells)
            {
                Print($"{++index}: {cell.Good.Name} - [{cell.Amount}] шт.");
            }
        }

        public static string GetRandomString(string storeAdressURL)
        {
            int randomValue;
            int minValue = 0;
            int maxValue = 26;
            int staticValue = 65;
            int payStringLenth = 20;
            string generatedString = String.Empty;
            char letter;

            for (int i = 0; i < payStringLenth; i++)
            {
                randomValue = s_random.Next(minValue, maxValue);
                letter = Convert.ToChar(randomValue + staticValue);

                generatedString += letter;
            }

            return storeAdressURL + generatedString;
        }
    }
}