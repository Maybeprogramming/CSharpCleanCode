namespace T06_RenameMethodsBySimpleEnglish
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }

        public static int Rummage(int[] array, int element)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i] == element)
                    return i;

            return -1;
        }
    }
}