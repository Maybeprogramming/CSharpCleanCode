namespace T11_MethodsSetter
{
    public class Program
    {
        public static void Main()
        {
            Console.Title = "ДЗ: 17. Методы set должны устанавливать значение из параметра";
        }

        public static void CreateObject()
        {
            //Создание объекта на карте
        }

        public static void SetRandomChance()
        {
            _chance = Random.Range(0, 100);
        }

        public static int CalculateSalary(int hoursWorked)
        {
            return _hourlyRate * hoursWorked;
        }
    }
}