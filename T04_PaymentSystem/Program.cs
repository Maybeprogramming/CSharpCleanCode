using System.Security.Cryptography;
using System.Text;

namespace T04_PaymentSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ДЗ: Платёжные системы";

            int orderID = 100500;
            int amountByRub = 12000;

            Order order = new Order(orderID, amountByRub);

            IPaymentSystem webPaymentSystem = new PaymentSystem(new WebPaymentSystem());
            IPaymentSystem criptoPaymentSystem = new PaymentSystem(new CriptoPaymentSystem());
            IPaymentSystem onlinePaymentSystem = new PaymentSystem(new OnlinePaymentSystem());

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

        public Order(int id, int amount) => (Id, Amount) = (id, amount);
    }

    public class PaymentSystem : IPaymentSystem
    {
        private IPaymentSystem _paymentSystem;

        public PaymentSystem(IPaymentSystem paymentSystem)
        {
            ArgumentNullException.ThrowIfNull(paymentSystem, nameof(paymentSystem));

            _paymentSystem = paymentSystem;
        }

        public string GetPayingLink(Order order)
        {
            return _paymentSystem.GetPayingLink(order);
        }
    }

    public class WebPaymentSystem : IPaymentSystem
    {
        public string GetPayingLink(Order order)
        {
            string hashMD5 = Criptografer.ComputeMD5(order.Id.ToString());

            return $"pay.system1.ru/order?amount={order.Amount}RUB&hash={{{hashMD5}}}";
        }
    }

    public class CriptoPaymentSystem : IPaymentSystem
    {
        public string GetPayingLink(Order order)
        {
            string hashMD5 = Criptografer.ComputeMD5($"{order.Id}{order.Amount}");

            return $"order.system2.ru/pay?hash={{{hashMD5}}}";
        }
    }

    public class OnlinePaymentSystem : IPaymentSystem
    {
        public string GetPayingLink(Order order)
        {
            string secretKey = "QWERTY-SECRET-KEY";
            string hashSHA1 = Criptografer.ComputeSHA1($"{order.Id}{order.Amount}{secretKey}");

            return $"system3.com/pay?amount={order.Amount}&curency=RUB&hash={{{hashSHA1}}}";
        }
    }

    public interface IPaymentSystem
    {
        public string GetPayingLink(Order order);
    }

    public static class Criptografer
    {
        public static string ComputeMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes).ToUpper();
            }
        }

        public static string ComputeSHA1(string input) 
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes).ToUpper();
            }
        }
    }
}