using System.Data;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace T13_Code_Refactoring
{
    public class Program
    {
        public static void Main()
        {
            Console.Title = "ДЗ: 27. В функции можно использовать функции её уровня и на один ниже";
            CitezensData data = new CitezensData();
            Repository repository = new Repository(data);
            SHA256Hasher hasher = new SHA256Hasher();
            VotePresenter votePresenter = new VotePresenter(null, hasher, repository);
            View view = new View(votePresenter);

            view.OnButtonClick();
        }
    }

    public class VotePresenter
    {
        private IView _view;
        private IHasher _hasher;
        private IRepository _repository;

        public VotePresenter(IView view, IHasher hasher, IRepository repository)
        {
            ArgumentNullException.ThrowIfNull(view);
            ArgumentNullException.ThrowIfNull(hasher);
            ArgumentNullException.ThrowIfNull(repository);

            _view = view;
            _hasher = hasher;
            _repository = repository;
        }

        public void Run(string rawData)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(rawData);

            string hashData = _hasher.ComputeHash(rawData);
            Citizen citezen = _repository.GetCitezen(hashData);

            CanVote(citezen);
        }

        private void CanVote(Citizen citezen)
        {
            if (citezen == null)
                _view.TextResult.Text = "Паспорт «" + _view.PassortTextBox.Text + "» в списке участников дистанционного голосования НЕ НАЙДЕН";

            if (citezen.CanVote)
                _view.TextResult.Text = "По паспорту «" + _view.PassortTextBox.Text + "» доступ к бюллетеню на дистанционном электронном голосовании ПРЕДОСТАВЛЕН";
            else
                _view.TextResult.Text = "По паспорту «" + _view.PassortTextBox.Text + "» доступ к бюллетеню на дистанционном электронном голосовании НЕ ПРЕДОСТАВЛЯЛСЯ";
        }
    }

    public class View : IView
    {
        private VotePresenter _votePresenter;

        public View(VotePresenter votePresenter)
        {
            PassortTextBox = new TextBox();
            TextResult = new TextBox();

            _votePresenter = votePresenter;
        }

        public TextBox PassortTextBox { get; }
        public TextBox TextResult { get; }

        public void OnButtonClick()
        {
            string rawData = TryTakeValidData();
            _votePresenter.Run(rawData);
        }

        private string TryTakeValidData()
        {
            string validData = PassortTextBox.Text.Trim();
            int validLength = 10;
            string symbolToReplace = " ";

            if (string.IsNullOrEmpty(validData))
            {
                MessageBox.Show("Введите серию и номер паспорта");
            }
            if (validData.Replace(symbolToReplace, string.Empty).Length < validLength)
            {
                TextResult.Text = "Неверный формат серии или номера паспорта";
            }

            return validData;
        }
    }

    public interface IView
    {
        TextBox PassortTextBox { get; }
        TextBox TextResult { get; }
    }

    public class Repository : IRepository
    {
        private CitezensData _citezenData;

        public Repository(CitezensData citezenData)
        {
            _citezenData = citezenData;
        }

        public Citizen GetCitezen(string hashData)
        {
            string _commandText = string.Format($"select * from passports where num='{hashData}' limit 1;");
            string _connectionString = string.Format($"Data Source={Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\db.sqlite");

            return _citezenData.GetCitizen(_commandText, _connectionString);
        }
    }

    public class CitezensData
    {
        public Citizen GetCitizen(string commandText, string connectionString)
        {
            try
            {
                SQLiteConnection connection = new SQLiteConnection(connectionString);
                Citizen citizen;

                connection.Open();

                SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(new SQLiteCommand(commandText, connection));
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = dataTable1;
                sqLiteDataAdapter.Fill(dataTable2);

                if (dataTable1.Rows.Count > 0)
                {
                    citizen = new Citizen(new Passport(String.Empty));
                }
                else
                    citizen = null;

                connection.Close();
            }
            catch (SQLiteException ex)
            {
                if (ex.ErrorCode != 1)
                    return;

                MessageBox.Show("Файл db.sqlite не найден. Положите файл в папку вместе с exe.");
            }
        }
    }

    public interface IRepository
    {
        Citizen GetCitezen(string hashData);
    }

    public class Citizen
    {
        private readonly Passport _passport;

        public Citizen(Passport passport)
        {
            ArgumentNullException.ThrowIfNull(passport);

            _passport = passport;
        }

        public bool CanVote { get; }
    }

    public class Passport
    {
        public Passport(string id)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(id);
            Id = id;
        }

        public string Id { get; }
    }

    public class SHA256Hasher : IHasher
    {
        public string ComputeHash(string input)
        {
            using (SHA256 hasher = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = SHA256.HashData(inputBytes);

                return Convert.ToHexString(hashBytes).ToUpper();
            }
        }
    }

    public interface IHasher
    {
        string ComputeHash(string input);
    }

    #region Классы заглушки
    //класс заглушка
    public class SQLiteException : Exception
    {
        public SQLiteException() { }

        public SQLiteException(string? message) : base(message) { }

        public SQLiteException(string? message, Exception? innerException) : base(message, innerException) { }

        public int ErrorCode { get; private set; }
    }

    //класс заглушка
    public static class MessageBox
    {
        public static void Show(string message) { }
    }

    //класс заглушка
    public class TextBox
    {
        public string Text { get; set; }
    }

    //класс заглушка
    public class SQLiteDataAdapter
    {
        private SQLiteCommand _sQLiteCommand;

        public SQLiteDataAdapter(SQLiteCommand sQLiteCommand)
        {
            _sQLiteCommand = sQLiteCommand;
        }

        internal void Fill(DataTable dataTable2) { }
    }

    //класс заглушка
    public class SQLiteCommand
    {
        public SQLiteCommand(string commandText, SQLiteConnection connection)
        {
            CommandText = commandText;
            Connection = connection;
        }

        public string CommandText { get; }
        public SQLiteConnection Connection { get; }
    }

    //класс заглушка
    public class SQLiteConnection
    {
        private string _connectionString;

        public SQLiteConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Open() { }

        public void Close() { }
    }

    #endregion
}