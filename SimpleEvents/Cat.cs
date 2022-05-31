namespace SimpleEvents
{
    using System;

    public class Cat
    {
        private int health;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Health
        {
            get => this.health;
            set
            {
                this.health = value;
                this.OnHealthChange?.Invoke(this, this.health); // we use null chechik instead of creating an if onHealthChange is null ...
                // this event notify all subscribers which cat health change and whats the new value
            }
        }

        public event EventHandler<int> OnHealthChange;
    }
}