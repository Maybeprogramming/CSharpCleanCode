namespace T15_ArgumentsFlagsAreBad
{
    public class Program
    {
        static void Main()
        {
            Console.Title = "ДЗ: 30. Аргументы-флаги - это плохо";
        }

        public void Activate()
        {
            _effects.StartEnableAnimation();
        }

        public void Deactivate() 
        {
            _pool.Free(this);
        }
    }
}