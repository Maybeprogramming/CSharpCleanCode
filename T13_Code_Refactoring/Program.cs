﻿using System.Data;
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
        }

        private void checkButton_Click(object sender, EventArgs e)
        {
            if (this.passportTextbox.Text.Trim() == "")
            {
                MessageBox.Show("Введите серию и номер паспорта");
            }
            else
            {
                string rawData = this.passportTextbox.Text.Trim().Replace(" ", string.Empty);
                if (rawData.Length < 10)
                {
                    this.textResult.Text = "Неверный формат серии или номера паспорта";
                }
                else
                {
                    string commandText = string.Format($"select * from passports where num='{new SHA256Hasher().ComputeHash(rawData)}' limit 1;");
                    string connectionString = string.Format($"Data Source={Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\db.sqlite");

                    try
                    {
                        SQLiteConnection connection = new SQLiteConnection(connectionString);

                        connection.Open();

                        SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(new SQLiteCommand(commandText, connection));
                        DataTable dataTable1 = new DataTable();
                        DataTable dataTable2 = dataTable1;
                        sqLiteDataAdapter.Fill(dataTable2);

                        if (dataTable1.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(dataTable1.Rows[0].ItemArray[1]))
                                this.textResult.Text = "По паспорту «" + this.passportTextbox.Text + "» доступ к бюллетеню на дистанционном электронном голосовании ПРЕДОСТАВЛЕН";
                            else
                                this.textResult.Text = "По паспорту «" + this.passportTextbox.Text + "» доступ к бюллетеню на дистанционном электронном голосовании НЕ ПРЕДОСТАВЛЯЛСЯ";
                        }
                        else
                            this.textResult.Text = "Паспорт «" + this.passportTextbox.Text + "» в списке участников дистанционного голосования НЕ НАЙДЕН";

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
        }
    }

    #region Классы заглушки

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

    public class VotePresenter
    {
        internal void Validate(IView view)
        {
            throw new NotImplementedException();
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
            _votePresenter.Validate(this);
        }
    }

    public interface IView
    {

    }

    public class Repository
    {

    }

    public class Passport
    {
        public Passport(string idendificationNumber)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(idendificationNumber);
            IdendificationNumber = idendificationNumber;
        }

        public string IdendificationNumber { get; }
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
}