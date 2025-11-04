using System.Xml.Linq;

namespace T14_Polimorfism
{
    public class Program
    {
        static void Main()
        {
            Console.Title = "ДЗ: Замена условной логики полиморфизмом";

            var orderForm = new OrderForm();
            var paymentHandler = new PaymentHandler();
            var paymentSystemFactory = new PaymentSystemFactory();

            var systemId = orderForm.ShowForm(paymentSystemFactory.GetPaymentsSystemNames());
            var systemPayment = paymentSystemFactory.Create(systemId);

            Console.WriteLine($"{systemPayment.CallAPI()}");

            paymentHandler.ShowPaymentResult(systemPayment);
        }
    }

    public class OrderForm
    {
        public string ShowForm(string[] paymentSystemNames)
        {
            Console.WriteLine($"Мы принимаем: {GetContactString(paymentSystemNames)}");

            //симуляция веб интерфейса
            Console.WriteLine("Какое системой вы хотите совершить оплату?");
            return Console.ReadLine();
        }

        private string GetContactString(string[] paymentSystemNames)
        {
            string result = String.Empty;
            string splitSymbols = ", ";

            for (int i = 0; i < paymentSystemNames.Length - 1; i++)
            {
                result += paymentSystemNames[i] + splitSymbols;
            }

            return result += paymentSystemNames[paymentSystemNames.Length - 1];
        }
    }

    public class PaymentHandler
    {
        public void ShowPaymentResult(IPaymentSystem paymentSystem)
        {
            Console.WriteLine($"Вы оплатили с помощью {paymentSystem.SystemId}");
            Console.WriteLine($"Проверка платежа через {paymentSystem.SystemId}...");
            Console.WriteLine("Оплата прошла успешно!");
        }
    }

    public class PaymentSystemFactory
    {
        private readonly Dictionary<string, PaymentFactory> _paymentSystemsFactory;

        public PaymentSystemFactory()
        {
            _paymentSystemsFactory = new Dictionary<string, PaymentFactory>()
            {
                {"QIWI", new QIWIFactory() },
                {"WebMoney", new WebMoneyFactory() },
                {"Card", new CardFactory() }
            };
        }

        public IPaymentSystem Create(string systemId) =>
            _paymentSystemsFactory.Where(paymentSystemId => paymentSystemId.Key.ToLower() == systemId.ToLower()).First().Value.Create() ?? throw new InvalidOperationException(nameof(Create));

        public string[] GetPaymentsSystemNames() =>
            _paymentSystemsFactory.Select(name => name.Key).ToArray();
    }

    public abstract class PaymentFactory
    {
        public abstract IPaymentSystem Create();
    }

    public class QIWIFactory : PaymentFactory
    {
        public override IPaymentSystem Create() =>
            new QIWI();
    }

    public class WebMoneyFactory : PaymentFactory
    {
        public override IPaymentSystem Create() =>
            new WebMoney();
    }

    public class CardFactory : PaymentFactory
    {
        public override IPaymentSystem Create() =>
            new Card();
    }

    public class QIWI : IPaymentSystem
    {
        public QIWI() =>
            SystemId = nameof(QIWI);

        public string SystemId { get; }

        public string CallAPI() =>
            $"Перевод на страницу {SystemId}...";
    }

    public class WebMoney : IPaymentSystem
    {
        public WebMoney() =>
            SystemId = nameof(WebMoney);

        public string SystemId { get; }

        public string CallAPI() =>
            $"Вызов API {SystemId}...";
    }

    public class Card : IPaymentSystem
    {
        public Card() =>
            SystemId = nameof(Card);

        public string SystemId { get; }

        public string CallAPI() =>
            $"Вызов API банка эмитера карты {SystemId}...";
    }

    public interface IPaymentSystem
    {
        string SystemId { get; }
        string CallAPI();
    }
}