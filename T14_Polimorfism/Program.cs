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

            var systemId = orderForm.ShowForm();
            var systemPayment = paymentSystemFactory.Get(systemId);

            Console.WriteLine($"{systemPayment.CallAPI()}");

            paymentHandler.ShowPaymentResult(systemPayment);
        }
    }

    public class OrderForm
    {
        public string ShowForm()
        {
            Console.WriteLine("Мы принимаем: QIWI, WebMoney, Card");

            //симуляция веб интерфейса
            Console.WriteLine("Какое системой вы хотите совершить оплату?");
            return Console.ReadLine();
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
        private readonly Dictionary<string, IPaymentSystem> _paymentSystems;

        public PaymentSystemFactory()
        {
            _paymentSystems = new Dictionary<string, IPaymentSystem>()
            {
                {"QIWI", new QIWI() },
                {"WebMoney", new WebMoney() },
                {"Card", new Card() }
            };
        }

        public IPaymentSystem Get(string systemId)
        {
            return _paymentSystems.Where(paymentSystemId => paymentSystemId.Key.ToLower() == systemId.ToLower()).First().Value;
        }
    }

    public class QIWI : IPaymentSystem
    {
        public QIWI()
        {
            SystemId = nameof(QIWI);
        }

        public string SystemId { get; }

        public string CallAPI()
        {
            return $"Перевод на страницу {SystemId}...";
        }
    }

    public class WebMoney : IPaymentSystem
    {
        public WebMoney()
        {
            SystemId = nameof(WebMoney);
        }

        public string SystemId { get; }

        public string CallAPI()
        {
            return $"Вызов API {SystemId}...";
        }
    }

    public class Card : IPaymentSystem
    {
        public Card()
        {
            SystemId = nameof(Card);
        }

        public string SystemId { get; }

        public string CallAPI()
        {
            return $"Вызов API банка эмитера карты {SystemId}...";
        }
    }

    public interface IPaymentSystem
    {
        string SystemId { get; }
        string CallAPI();
    }
}