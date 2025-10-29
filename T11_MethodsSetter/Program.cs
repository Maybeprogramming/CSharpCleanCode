namespace T11_MethodsSetter
{
    public class Program
    {
        static void Main()
        {
            Console.Title = "ДЗ: 17. Методы set должны устанавливать значение из параметра";

        public static void CreateObject()
        {
            //Создание объекта на карте
        }

        public static void GenerateChance()
        {
            _chance = Random.Range(0, 100);
        }

        public static int GetSalary(int hoursWorked)
        {
            return _hourlyRate * hoursWorked;
        }
    }
}