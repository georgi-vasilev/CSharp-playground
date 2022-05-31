
namespace ActionAndFunc
{
    using System;

    public class Cat
    {
        public string Name { get; set; }
        public void MakeSound()
        {
            Console.WriteLine("Meow");
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Action action = SomeVoid;
            action += () => Console.WriteLine("else");
            action();

            Action<int> actionWithParam = SomeVoidWithParameter;
            actionWithParam += x => Console.WriteLine(x);
            actionWithParam(10);

            Func<string,bool, int> func = SomeMethod;
            //func += () => 42;
            Console.WriteLine(func("text", true)); // cannot return multiple values from a function (returns the last result);
                                                   // however, both methods are called

            Func<int, int, int> sumFunc = (x, y) => x + y;
            Console.WriteLine(sumFunc(5, 10));


            Action<Cat> catAction = cat => cat.MakeSound();
            var cat = new Cat
            {
                Name = "Pesho",
            };
            catAction(cat);

            Func<Cat, string> catFunc = cat => cat.Name;
            Console.WriteLine(catFunc(cat));
        }

        public static int SomeMethod(string text, bool someBool)
        {
            Console.WriteLine($"Calling {text}...");
            return 5;
        }

        public static void SomeVoid()
        {
            Console.WriteLine("Test");
        }

        public static void SomeVoidWithParameter(int number) => Console.WriteLine(number);
    }
}
