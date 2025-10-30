namespace T06_RenameMethodsBySimpleEnglish
{
    public class Program
    {
        static void Main()
        {
            Console.Title = "ДЗ: \"7. При именовании имеет смысл использовать упрощенный английский\"";
        }

        public static int GetIndex(int[] array, int element)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i] == element)
                    return i;

            return -1;
        }
    }
}