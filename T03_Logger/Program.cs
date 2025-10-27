namespace T03_Logger
{
    public class Program
    {
        static void Main(string[] args)
        {
            DayOfWeek dayOfWeekToLogWritter = DayOfWeek.Friday;

            ILogger consoleLogWritter = new ConsoleLogWritter();
            ILogger secureConsoleLogWritter = new SecureLogWritter(consoleLogWritter, dayOfWeekToLogWritter);
            ILogger fileLogWritter = new FileLogWritter();
            ILogger secureFileLogWritter = new SecureLogWritter(fileLogWritter, dayOfWeekToLogWritter);
            List<ILogger> loggersCollection = new List<ILogger>() { consoleLogWritter, secureFileLogWritter };
            ILogger specialLogWritter = new SpecialLogWritter(loggersCollection);

            Pathfinder consoleLog = new Pathfinder(consoleLogWritter);
            Pathfinder secureConsoleLog = new Pathfinder(secureConsoleLogWritter);
            Pathfinder fileLog = new Pathfinder(fileLogWritter);
            Pathfinder secureFileLog = new Pathfinder(secureFileLogWritter);
            Pathfinder specialLog = new Pathfinder(specialLogWritter);

            consoleLog.Find($"Вывод сообщения в консоль");
            secureConsoleLog.Find($"Вывод сообщения в консоль по [{dayOfWeekToLogWritter}]");
            fileLog.Find($"Запись сообщения в файл");
            secureFileLog.Find($"Запись сообщения в файл по пятницам");
            specialLog.Find($"Вывод сообщения в консоль и запись в файл по [{dayOfWeekToLogWritter}]");
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

    public class SecureLogWritter : ILogger
    {
        private ILogger _loggerWritter;
        private DayOfWeek _dayOfWeek;

        public SecureLogWritter(ILogger loggerWritter, DayOfWeek dayOfWeek)
        {
            ArgumentNullException.ThrowIfNull(loggerWritter);
            ArgumentNullException.ThrowIfNull(dayOfWeek);

            _loggerWritter = loggerWritter;
            _dayOfWeek = dayOfWeek;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == _dayOfWeek)
            {
                _loggerWritter.WriteError(message);
            }
        }
    }

    public class SpecialLogWritter : ILogger
    {
        private readonly List<ILogger> _loggers;

        public SpecialLogWritter(List<ILogger> loggers)
        {
            ArgumentNullException.ThrowIfNull(loggers);

            _loggers = loggers;
        }

        public void WriteError(string message)
        {
            foreach (ILogger logWritter in _loggers)
            {
                logWritter.WriteError(message);
            }
        }
    }

    public interface ILogger
    {
        void WriteError(string message);
    }
}