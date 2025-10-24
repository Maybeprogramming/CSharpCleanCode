namespace T11_MethodsSetter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }

        public static void SetNewObject()
        {
            //Создание объекта на карте
        }

        public static void SetChance()
        {
            _chance = Random.Range(0, 100);
        }

        public static int SetSalary(int hoursWorked)
        {
            return _hourlyRate * hoursWorked;
        }
    }
}
