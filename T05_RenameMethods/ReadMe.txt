2. Даже простой алгоритм можно угробить тупым названием метода
Переименуйте метод - https://gist.github.com/HolyMonkey/92e8c9c2f08471e31eceef30da7ea6ad

            Console.WriteLine($"{Clamp(0,0,10)}");
            Console.WriteLine($"{Clamp(-1,0,10)}");
            Console.WriteLine($"{Clamp(10,0,10)}");
            Console.WriteLine($"{Clamp(11,0,10)}");
            Console.WriteLine($"{Clamp(5,0,10)}");
            Console.WriteLine($"{Clamp(9,10,10)}");
            Console.WriteLine($"{Clamp(10,10,10)}");
            Console.WriteLine($"{Clamp(11,10,10)}");
            Console.ReadKey();
            Console.WriteLine($"{Clamp(5,11,10)}");

            
            int minValue = 0;
            int maxValue = 10;
            int value = 5;

            Console.WriteLine($"{Clamp(value,minValue,maxValue)}");
            value = -1;
            Console.WriteLine($"{Clamp(value,minValue,maxValue)}");
            value = 11;
            Console.WriteLine($"{Clamp(value,minValue,maxValue)}");
            Console.ReadKey();