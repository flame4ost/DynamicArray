using System;

namespace DynamicArray
{
    class Program
    {
        public static void Main(string[] args)
        {
            var arr = new DynamicArray<int>(80, 30, 50, 40);
 
            Console.WriteLine("Введённый массив: " + string.Join(", ", arr));
 
            arr += 11;
            arr = 99 + arr;
 
            Console.WriteLine("Массив после добавлений: " + string.Join(", ", arr));
 
            arr--;
            arr -= 30;
 
            Console.WriteLine("Массив после удалений: " + string.Join(", ", arr));
        }
    }
}