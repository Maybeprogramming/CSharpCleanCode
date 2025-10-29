namespace T15_ArgumentsFlagsAreBad
{
    public class Program
    {
        static void Main()
        {
            Console.Title = "ДЗ: 30. Аргументы-флаги - это плохо";
        }

        public void SetEnable(bool enable)
        {
            _enable = enable;

            if (_enable)
            {
                _effects.StartEnableAnimation();
            }
            else
            {
                _pool.Free(this);
            }
        }
    }
}