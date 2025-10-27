namespace T03_Logger
{
    public class Program
    {
        static void Main(string[] args)
        {
            ILogger consoleLogWritter = new ConsoleLogWritter();
            ILogger secureConsoleLogWritter = new SecureConsoleLogWritter();
            ILogger fileLogWritter = new FileLogWritter();
            ILogger secureFileLogWritter = new SecureFileLogWritter();
            ILogger specialLogWritter = new SpecialLogWritter(new ConsoleLogWritter(), new SecureFileLogWritter());

            Pathfinder pathfinder = new Pathfinder();

            pathfinder.Find(consoleLogWritter, $"Вывод сообщения в консоль");
            pathfinder.Find(secureConsoleLogWritter, $"Вывод сообщения в консоль по пятницам");
            pathfinder.Find(fileLogWritter, $"Вывод сообщения в файл");
            pathfinder.Find(secureFileLogWritter, $"Вывод сообщения в файл по пятницам");
            pathfinder.Find(specialLogWritter, $"Вывод сообщения в консоль ежедневно и в файл по пятницам");
        }
    }

    public class Pathfinder
    {
        public void Find(ILogger logger, string message)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

            logger.WriteError(message);
        }
    }

    public class ConsoleLogWritter : ILogger
    {
        public virtual void WriteError(string message)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

            Console.WriteLine(message);
        }
    }

    public class FileLogWritter : ILogger
    {
        private readonly string _path = "log.txt";

        public virtual void WriteError(string message)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

            string logText = String.Empty;

            if (File.Exists(_path))
            {
                logText = File.ReadAllText(_path);
            }

            logText += "\n" + message;

            File.WriteAllText(_path, logText);
        }
    }

    public class SecureFileLogWritter : FileLogWritter
    {
        public override void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                base.WriteError(message);
            }
        }
    }

    public class SecureConsoleLogWritter : ConsoleLogWritter
    {
        public override void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                base.WriteError(message);
            }
        }
    }

    public class SpecialLogWritter : ILogger
    {
        ConsoleLogWritter _consoleLogWritter;
        SecureFileLogWritter _secureFileLogWritter;

        public SpecialLogWritter(ConsoleLogWritter consoleLogWritter, SecureFileLogWritter secureFileLogWritter)
        {
            ArgumentNullException.ThrowIfNull(consoleLogWritter);
            ArgumentNullException.ThrowIfNull(secureFileLogWritter);

            _consoleLogWritter = consoleLogWritter;
            _secureFileLogWritter = secureFileLogWritter;
        }

        public void WriteError(string message)
        {
            _consoleLogWritter.WriteError(message);
            _secureFileLogWritter.WriteError(message);
        }
    }
    public interface ILogger
    {
        void WriteError(string message);
    }
}