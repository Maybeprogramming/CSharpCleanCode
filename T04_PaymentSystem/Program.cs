using System.Security.Cryptography;
using System.Text;

namespace T04_PaymentSystem
{
    class Program
    {
        static void Main()
        {
            Console.Title = "ДЗ: Платёжные системы";

            int orderID = 100500;
            int amountByRub = 12000;
            string secretKey = "QWERTY-SECRET-KEY";

            Order order = new Order(orderID, amountByRub);

            IPaymentSystem webPaymentSystem = new WebPaymentSystem(new MD5Hasher());
            IPaymentSystem criptoPaymentSystem = new CriptoPaymentSystem(new MD5Hasher());
            IPaymentSystem onlinePaymentSystem = new OnlinePaymentSystem(new SHA1Hasher(), secretKey);

            Console.WriteLine($"{webPaymentSystem.GetPayingLink(order)}");
            Console.WriteLine($"{criptoPaymentSystem.GetPayingLink(order)}");
            Console.WriteLine($"{onlinePaymentSystem.GetPayingLink(order)}");

            Console.ReadKey();
        }
    }

    public class Order
    {
        public readonly int Id;
        public readonly int Amount;

        public Order(int id, int amount)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

            Id = id;
            Amount = amount;
        }
    }

    public class WebPaymentSystem : IPaymentSystem
    {
        private readonly IHasher _hasher;

        public WebPaymentSystem(IHasher hasher)
        {
            ArgumentNullException.ThrowIfNull(hasher, nameof(hasher));

            _hasher = hasher;
        }

        public string GetPayingLink(Order order)
        {
            string hash = _hasher.ComputeHash(order.Id.ToString());

            return $"pay.system1.ru/order?amount={order.Amount}RUB&hash={hash}";
        }
    }

    public class CriptoPaymentSystem : IPaymentSystem
    {
        private readonly IHasher _hasher;

        public CriptoPaymentSystem(IHasher hasher)
        {
            ArgumentNullException.ThrowIfNull(hasher, nameof(hasher));

            _hasher = hasher;
        }

        public string GetPayingLink(Order order)
        {
            string hash = _hasher.ComputeHash($"{order.Id}{order.Amount}");

            return $"order.system2.ru/pay?hash={hash}";
        }
    }

    public class OnlinePaymentSystem : IPaymentSystem
    {
        private readonly IHasher _hasher;
        private readonly string _secretKey;

        public OnlinePaymentSystem(IHasher hasher, string secretKey)
        {
            ArgumentNullException.ThrowIfNull(hasher, nameof(hasher));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(secretKey, nameof(secretKey));

            _hasher = hasher;
            _secretKey = secretKey;
        }

        public string GetPayingLink(Order order)
        {
            string hash = _hasher.ComputeHash($"{order.Id}{order.Amount}{_secretKey}");

            return $"system3.com/pay?amount={order.Amount}&curency=RUB&hash={hash}";
        }
    }

    public class MD5Hasher : IHasher
    {
        public string ComputeHash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes).ToUpper();
            }
        }
    }

    public class SHA1Hasher : IHasher
    {
        public string ComputeHash(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes).ToUpper();
            }
        }
    }
    public interface IPaymentSystem
    {
        public string GetPayingLink(Order order);
    }

    public interface IHasher
    {
        public string ComputeHash(string input);
    }
}