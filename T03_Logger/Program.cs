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

            Pathfinder consoleLog = new Pathfinder(consoleLogWritter);
            Pathfinder secureConsoleLog = new Pathfinder(secureConsoleLogWritter);
            Pathfinder fileLog = new Pathfinder(fileLogWritter);
            Pathfinder secureFileLog = new Pathfinder(secureFileLogWritter);
            Pathfinder specialLog = new Pathfinder(specialLogWritter);

            consoleLog.Find($"Вывод сообщения в консоль");
            secureConsoleLog.Find($"Вывод сообщения в консоль по пятницам");
            fileLog.Find($"Запись сообщения в файл");
            secureFileLog.Find($"Запись сообщения в файл по пятницам");
            specialLog.Find($"Вывод сообщения в консоль и запись в файл по пятницам");
        }
    }

    public class Pathfinder
    {
        private ILogger _logWritter;

        public Pathfinder(ILogger logWritter)
        {
            ArgumentNullException.ThrowIfNull(logWritter);

            _logWritter = logWritter;
        }

        public void Find(string message)
        {            
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

            _logWritter.WriteError(message);
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
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                base.WriteError(message);
            }
        }
    }

    public class SecureConsoleLogWritter : ConsoleLogWritter
    {
        public override void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                base.WriteError(message);
            }
        }
    }

    public class SpecialLogWritter : ILogger
    {
        private ConsoleLogWritter _consoleLogWritter;
        private SecureFileLogWritter _secureFileLogWritter;

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