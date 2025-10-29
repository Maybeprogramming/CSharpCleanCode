namespace T10_ClassNameAreNouns
{
    public class Program
    {
        static void Main()
        {
            Console.Title = "ДЗ: 15. Имена классов и объектов должны предоставлять собой существительные";
        }
    }

    public class Unit{}
    public class PlayerInfo { }
    public class Weapon { }
    public class Movement { }
    public class Squad
    {
        public IReadOnlyCollection<Unit> Units { get; private set; }
    }
}