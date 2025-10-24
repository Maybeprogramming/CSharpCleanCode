namespace T05_RenameMethods
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
        }

        public static int MakeValidNumber(int a, int b, int c)
        {
            if (a < b)
                return b;
            else if (a > c)
                return c;
            else
                return a;
        }
    }
}
