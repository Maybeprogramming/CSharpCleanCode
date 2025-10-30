namespace T05_RenameMethods
{
    public class Program
    {
        static void Main()
        {
            Console.Title = "ДЗ: \"2. Даже простой алгоритм можно угробить тупым названием метода\"";
        }

        public static int Clamp(int value, int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new Exception($"Минимальное значение не может быть больше максимального! min: {minValue} > max: {maxValue}");

            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            else
                return value;
        }
    }
}