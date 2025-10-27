namespace T02_WebMarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(80, 25);  // Стандартный размер
            Console.Title = "Напильник. ДЗ: Интернет магазин";

            Product iPhone12 = new Product("IPhone 12");
            Product iPhone11 = new Product("IPhone 11");

            Warehouse warehouse = new Warehouse();
            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            //Вывод всех товаров на складе с их остатком
            warehouse.ShowProducts();

            Cart cart = shop.GetCart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            //Вывод всех товаров в корзине
            cart.ShowProducts();

            Console.WriteLine(cart.GetOrder().Paylink);

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары

            Console.ReadKey();
        }
    }

    public class Cart
    {
        private readonly List<Cell> _cells;

        private IWarehoseReserveableProducts _warehouse;

        public Cart(IWarehoseReserveableProducts warehouse)
        {
            ArgumentNullException.ThrowIfNull(warehouse);

            _cells = new List<Cell>();
            _warehouse = warehouse;
        }

        public void Add(Product product, int amount)
        {
            Cell cell = new Cell(product, amount);

            if (_warehouse.IsProductAvaiableByAmount(cell) == false)
            {
                throw new Exception($"Ошибка - нет {product.Name} в количестве: [{amount}] единиц");
            }

            if (IsCellContainsProduct(product))
            {
                MergeCell(product, amount);
            }
            else
            {
                _cells.Add(cell);
            }
        }

        public IOrderable GetOrder()
        {
            _warehouse.TryRemoveOrderedProducts(_cells);

            return new Order();
        }

        public void ShowProducts() =>
            Utils.PrintCollection(_cells, $"В корзине следующие товары:");

        private void MergeCell(Product newProduct, int amount) =>
            _cells.First(cell => cell.Products.Name == newProduct.Name).Add(amount);

        private bool IsCellContainsProduct(Product productToValidate) =>
             _cells.Where(cell => cell.Products.Name == productToValidate.Name).Count() > 0;
    }

    public class Cell : IReadOnlyCell
    {
        public Cell(Product product, int amount)
        {
            ArgumentNullException.ThrowIfNull(product);
            ArgumentOutOfRangeException.ThrowIfNegative(amount);

            Products = product;
            Amount = amount;
        }

        public Product Products { get; }
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
        private IWarehoseReserveableProducts _warehouse;

        public Shop(Warehouse warehouse)
        {
            ArgumentNullException.ThrowIfNull(warehouse);

            _warehouse = warehouse;
        }

        public Cart GetCart() =>
            new Cart(_warehouse);
    }

    public class Warehouse : IWarehoseReserveableProducts
    {
        private readonly List<Cell> _cells;

        public Warehouse()
        {
            _cells = new();
        }

        public void Delive(Product product, int amount)
        {
            if (IsCellContainsProduct(product))
            {
                MergeCell(product, amount);
            }
            else
            {
                _cells.Add(new Cell(product, amount));
            }
        }

        public void ShowProducts() =>
            Utils.PrintCollection(_cells, $"На складе имеются следующие товары:");

        public bool IsProductAvaiableByAmount(IReadOnlyCell cellToVerify)
        {
            IReadOnlyCell? cellInStock = GetProductByName(cellToVerify);

            if (cellInStock != null && cellInStock.Amount >= cellToVerify.Amount)
            {
                return true;
            }

            return false;
        }

        public void TryRemoveOrderedProducts(IEnumerable<IReadOnlyCell> cellsOrdered)
        {
            foreach (Cell cellOrdered in cellsOrdered)
            {
                Cell? cell = GetProductByName(cellOrdered);

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

        private void MergeCell(Product newProduct, int amount) =>
            _cells.First(cell => cell.Products.Name == newProduct.Name).Add(amount);

        private bool IsCellContainsProduct(Product productToValidate) =>
             _cells.Where(cell => cell.Products.Name == productToValidate.Name).Count() > 0;

        private Cell? GetProductByName(IReadOnlyCell cellToVerify) =>
            _cells.First(cell => cell.Products.Name == cellToVerify.Products.Name);
    }

    public class Product
    {
        public Product(string name)
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

    public interface IWarehoseReserveableProducts
    {
        bool IsProductAvaiableByAmount(IReadOnlyCell verifyCell);
        void TryRemoveOrderedProducts(IEnumerable<IReadOnlyCell> cells);
    }

    public interface IReadOnlyCell
    {
        public Product Products { get; }
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
            ArgumentNullException.ThrowIfNullOrWhiteSpace(text);

            Console.WriteLine($"{text}");
        }

        public static void PrintCollection(IEnumerable<Cell> cells, string message)
        {
            ArgumentNullException.ThrowIfNull(cells);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

            int index = 0;

            Print(message);

            foreach (var cell in cells)
            {
                Print($"{++index}: {cell.Products.Name} - [{cell.Amount}] шт.");
            }
        }

        public static string GetRandomString(string storeAdressURL)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(storeAdressURL);

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