namespace BasicDelegates
{
    using System;

    public class Program
    {
        public class Cat
        {
            public string Name { get; set; }

            public void SomeCatMethod(int cat)
            {

            }
        }

        public static void Main(string[] args)
        {
            var myDelegate = new MyVoidDelegate(PrintInteger);
            
            var myCoolDelegate = new MyCoolDelegate(SomeOtherMethod);
            var cat = new Cat();

            // instead of using 'var' and the 'new' keyword
            MyVoidDelegateWithString withStr = SomeMethodWithString;

            // adding methods, as long as having the same signature
            myDelegate += SomeMethod;
            myDelegate += PrintInteger;
            myDelegate -= SomeMethod; // removes the method

            myDelegate += x => Console.WriteLine("Inline func 1");
            myDelegate += x => Console.WriteLine("Inline func 2");
            myDelegate += x => Console.WriteLine("Inline func 3");
            // lambda expressions cannot be removed;

            myDelegate?.Invoke(10); // we can use null checking as well if the delegate is not instatiated;
            //myDelegate?.DynamicInvoke(10); // should be used when we are not familiar what delegate we are using;

            myDelegate(100);
            myDelegate += cat.SomeCatMethod;
            Console.WriteLine(myDelegate.Target?.GetType().Name); // the target will be cat, if it is static it will be null
            PassSomeDelegate(myDelegate);
        }

        public static void PassSomeDelegate(Delegate del)
        {
            del.DynamicInvoke(5); // slower
        }

        public static void PrintInteger(int number)
        {
            Console.WriteLine(number);
        }

        public static void SomeMethod(int number)
        {
            Console.WriteLine(number + 20);
        }

        public static void SomeMethodWithString(string text)
        {

        }

        public static string SomeOtherMethod(int x, int y)
        {
            return x + y.ToString();
        }
    }
}
