namespace SimpleEvents
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            var cat = new Cat()
            {
                Id = 1,
                Name = "Test",
                Health = 100,
            };

            cat.OnHealthChange += CatOnHealthChange;
            cat.OnHealthChange += CatOnDead;

            cat.Health = 100;

            for (int i = 0; i < 10; i++)
            {
                cat.Health -= 10;
            }
        }

        private static void CatOnDead(object sender, int health)
        {
            var cat = sender as Cat;
            if (cat.Health <= 0)
            {
                Console.WriteLine($"{cat.Name} is no longer alive");
            }
        }

        private static void CatOnHealthChange(object sender, int health)
        {
            var cat = sender as Cat;
            Console.WriteLine($"{cat.Name} has new health {cat.Health}");


        }
    }
}
