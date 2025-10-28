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

            Order order = new Order(orderID, amountByRub);

            IPaymentSystem webPaymentSystem = new PaymentSystem(new WebPaymentSystem(new CriptoSystem(new MD5Hasher())));
            IPaymentSystem criptoPaymentSystem = new PaymentSystem(new CriptoPaymentSystem(new CriptoSystem(new MD5Hasher())));
            IPaymentSystem onlinePaymentSystem = new PaymentSystem(new OnlinePaymentSystem(new CriptoSystem(new SHA1Hasher())));

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
        private readonly IPaymentSystem _paymentSystem;

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

    public abstract class BasePaymentSystem: IPaymentSystem
    {
        private protected readonly IPrivacyPolicy _privacyPolicy;

        public BasePaymentSystem(IPrivacyPolicy privacyPolicy)
        {
            ArgumentNullException.ThrowIfNull(privacyPolicy, nameof(privacyPolicy));

            _privacyPolicy = privacyPolicy;
        }

        public abstract string GetPayingLink(Order order);
    }

    public class WebPaymentSystem(IPrivacyPolicy privacyPolicy) : BasePaymentSystem(privacyPolicy)
    {
        public override string GetPayingLink(Order order)
        {
            string hashMD5 = _privacyPolicy.ComputeHash(order.Id.ToString());

            return $"pay.system1.ru/order?amount={order.Amount}RUB&hash={{{hashMD5}}}";
        }
    }

    public class CriptoPaymentSystem(IPrivacyPolicy privacyPolicy) : BasePaymentSystem(privacyPolicy)
    {
        public override string GetPayingLink(Order order)
        {
            string hashMD5 = _privacyPolicy.ComputeHash($"{order.Id}{order.Amount}");

            return $"order.system2.ru/pay?hash={{{hashMD5}}}";
        }
    }

    public class OnlinePaymentSystem(IPrivacyPolicy privacyPolicy) : BasePaymentSystem(privacyPolicy)
    {
        public override string GetPayingLink(Order order)
        {
            string secretKey = "QWERTY-SECRET-KEY";
            string hashSHA1 = _privacyPolicy.ComputeHash($"{order.Id}{order.Amount}{secretKey}");

            return $"system3.com/pay?amount={order.Amount}&curency=RUB&hash={{{hashSHA1}}}";
        }
    }

    public class CriptoSystem: IPrivacyPolicy
    {
        private IPrivacyPolicy _privacyPolicy;

        public CriptoSystem(IPrivacyPolicy privacyPolicy)
        {
            ArgumentNullException.ThrowIfNull(privacyPolicy);

            _privacyPolicy = privacyPolicy;
        }

        public string ComputeHash(string input)
        {
            return _privacyPolicy.ComputeHash(input);
        }
    }

    public class MD5Hasher : IPrivacyPolicy
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

    public class SHA1Hasher : IPrivacyPolicy
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

    public interface IPrivacyPolicy
    {
        public string ComputeHash(string input);
    }
}