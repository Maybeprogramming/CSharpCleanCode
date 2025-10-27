namespace T02_WebMarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(80, 25);  // Стандартный размер
            Console.Title = "Напильник. ДЗ: Интернет магазин";

            Goods iPhone12 = new Goods("IPhone 12");
            Goods iPhone11 = new Goods("IPhone 11");

            Warehouse warehouse = new Warehouse();
            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            //Вывод всех товаров на складе с их остатком
            warehouse.ShowAll();

            Cart cart = shop.GetCart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            //Вывод всех товаров в корзине
            cart.ShowGoods();

            Console.WriteLine(cart.Order.Paylink);

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары

            Console.ReadKey();
        }
    }

    public class Cart
    {
        private readonly List<Cell> _cells;

        private GoodsValidator _validator;
        private Order _order;

        public event Action<IEnumerable<IReadOnlyCell>> Ordered;

        public Cart(GoodsValidator validator)
        {
            ArgumentNullException.ThrowIfNull(validator);

            _order = new Order();
            _cells = new List<Cell>();
            _validator = validator;
        }

        public IOrderable Order => _order;

        public void Add(Goods goods, int amount)
        {
            Cell cell = new Cell(goods, amount);

            if (_validator.IsGoodsAvaiableByAmount(cell) == false)
            {
                Utils.Print($"Ошибка - нет нужного количества товара на складе");
            }
            else
            {
                _cells.Add(cell);
            }
        }

        public void ShowGoods() =>
            Utils.PrintCollection(_cells, $"В корзине следующие товары:");
    }

    public class Cell : IReadOnlyCell
    {
        public Cell(Goods goods, int amount)
        {
            ArgumentNullException.ThrowIfNull(goods);
            ArgumentOutOfRangeException.ThrowIfNegative(amount);

            Goods = goods;
            Amount = amount;
        }

        public Goods Goods { get; }
        public int Amount { get; private set; }

        public void TryRemove(int amount)
        {
            if (amount < 0 && amount > Amount)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Amount -= amount;
        }

        public void Add(int amount)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(amount);

            Amount += amount;
        }
    }

    public class Shop
    {
        private Warehouse _warehouse;
        private Cart _cart;
        private GoodsValidator _validator;

        public Shop(Warehouse warehouse)
        {
            ArgumentNullException.ThrowIfNull(warehouse);

            _warehouse = warehouse;
            _validator = new GoodsValidator(_warehouse.Cells);
        }

        public Cart GetCart()
        {
            _cart = new Cart(_validator);
            _cart.Ordered += OnOrdered;

            return _cart;
        }

        private void OnOrdered(IEnumerable<IReadOnlyCell> cells)
        {
            _cart.Ordered -= OnOrdered;
            _warehouse.TryRemoveGoods(cells);
        }
    }

    public class Warehouse
    {
        private readonly List<Cell> _cells;

        public Warehouse()
        {
            _cells = new();
        }

        public IEnumerable<IReadOnlyCell> Cells => _cells;

        public void Delive(Goods goods, int amount)
        {
            if (IsCellContainsGoods(goods))
            {
                MergeCell(goods, amount);
            }
            else
            {
                _cells.Add(new Cell(goods, amount));
            }
        }

        public void ShowAll() =>
            Utils.PrintCollection(_cells, $"На складе имеются следующие товары:");

        public void TryRemoveGoods(IEnumerable<IReadOnlyCell> cellsOrdered)
        {
            foreach (Cell cellOrdered in cellsOrdered)
            {
                Cell cell = _cells.First(findedCell => findedCell.Goods.Name == cellOrdered.Goods.Name);

                if (cell == null)
                {
                    throw new InvalidOperationException(nameof(cell));
                }
                else
                {
                    cell.TryRemove(cellOrdered.Amount);
                }
            }
        }

        private void MergeCell(Goods newGoods, int amount)
        {
            _cells.First(cell => cell.Goods.Name == newGoods.Name).Add(amount);
        }

        private bool IsCellContainsGoods(Goods goodsToValidate)
        {
            return _cells.Where(cell => cell.Goods.Name == goodsToValidate.Name).Count() > 0;
        }
    }

    public class Goods
    {
        public Goods(string name)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name);

            Name = name;
        }

        public string Name { get; }
    }

    public class Order : IOrderable
    {
        private string _storeAdressURL;

        public Order()
        {
            _storeAdressURL = "https://online-store.ru/Paylink/";
        }

        public string Paylink => Utils.GetRandomString(_storeAdressURL);
    }

    public class GoodsValidator
    {
        private IEnumerable<IReadOnlyCell> _cells;

        public GoodsValidator(IEnumerable<IReadOnlyCell> cells)
        {
            ArgumentNullException.ThrowIfNull(cells);

            _cells = cells;
        }

        public bool IsGoodsAvaiableByAmount(IReadOnlyCell cellToVerify)
        {
            IReadOnlyCell? cellInStock = TryGetGoodsByName(cellToVerify);

            if (cellInStock != null && cellInStock.Amount >= cellToVerify.Amount)
            {
                return true;
            }

            return false;
        }

        private IReadOnlyCell? TryGetGoodsByName(IReadOnlyCell cellToVerify)
        {
            IReadOnlyCell cellInStock = _cells.First(cell => cell.Goods.Name == cellToVerify.Goods.Name);

            if (cellInStock != null)
            {
                return cellInStock;
            }

            return null;
        }
    }

    public interface IReadOnlyCell
    {
        public Goods Goods { get; }
        public int Amount { get; }
    }

    public interface IOrderable
    {
        public string Paylink { get; }
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
                Print($"{++index}: {cell.Goods.Name} - [{cell.Amount}] шт.");
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