using System;
using System.Collections.Generic;

namespace TextBasedRPG
{
    // Interface for objects that can take damage
    interface IDamageable
    {
        void TakeDamage(int damage);
    }

    class Item
    {
        public string Name { get; }
        public int EffectValue { get; }
        public Action<Player> Effect { get; }

        public Item(string name, int effectValue, Action<Player> effect)
        {
            Name = name;
            EffectValue = effectValue;
            Effect = effect;
        }

        // Copy constructor
        public Item(Item other)
        {
            Name = other.Name;
            EffectValue = other.EffectValue;
            Effect = other.Effect;
        }

        public void Apply(Player player)
        {
            Effect(player);
            Console.WriteLine($"{Name} applied!");
        }
    }

    class Inventory
    {
        private readonly int capacity;
        private readonly List<Item> items;

        public Inventory(int capacity)
        {
            this.capacity = capacity;
            items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            if (items.Count < capacity)
            {
                items.Add(item);
                Console.WriteLine($"{item.Name} added to inventory.");
            }
            else
            {
                Console.WriteLine("Inventory is full. Sell an item first!");
            }
        }

        public void RemoveItem(Item item)
        {
            if (items.Remove(item))
            {
                Console.WriteLine($"{item.Name} removed from inventory.");
            }
            else
            {
                Console.WriteLine("Item not found in inventory.");
            }
        }

        public void DisplayItems()
        {
            if (items.Count == 0)
            {
                Console.WriteLine("No items in inventory.");
            }
            else
            {
                Console.WriteLine("Inventory:");
                for (int i = 0; i < items.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {items[i].Name}");
                }
            }
        }

        public bool IsFull => items.Count >= capacity;
        public bool IsEmpty => items.Count == 0;
        public List<Item> Items => items;
    }

    abstract class Player : IDamageable
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Energy { get; set; }
        public int XP { get; set; }
        public string Rank { get; set; }
        public Inventory Inventory { get; set; }

        public Player(string name, int health, int energy)
        {
            Name = name;
            Health = health;
            Energy = energy;
            XP = 0;
            Rank = "Novice";
            Inventory = new Inventory(2); // Limit inventory to 2 items
        }

        // Copy constructor
        public Player(Player other)
        {
            Name = other.Name;
            Health = other.Health;
            Energy = other.Energy;
            XP = other.XP;
            Rank = other.Rank;
            Inventory = new Inventory(2);  // Can be extended to clone inventory as well
        }

        public abstract void Attack(Enemy enemy);

        public virtual void GainXP(int amount)
        {
            XP += amount;
            if (XP >= 50) Rank = "Apprentice";
        }

        public void UseEnergy(int amount)
        {
            if (Energy >= amount)
            {
                Energy -= amount;
            }
            else
            {
                Console.WriteLine("No energy left to attack!");
            }
        }

        public void ClampHealth()
        {
            if (Health < 0)
            {
                Health = 0;
            }
        }

        // IDamageable Interface method implementation
        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }

        // Operator overloading for comparing player health
        public static bool operator >(Player p1, Player p2)
        {
            return p1.Health > p2.Health;
        }

        public static bool operator <(Player p1, Player p2)
        {
            return p1.Health < p2.Health;
        }
    }

    class Warrior : Player
    {
        public Warrior(string name) : base(name, 100, 50) { }

        public Warrior(Warrior other) : base(other) { }

        public override void Attack(Enemy enemy)
        {
            if (Energy >= 10)
            {
                Console.WriteLine($"{Name} slashes at the enemy!");
                enemy.TakeDamage(20);
                UseEnergy(10);
            }
            else
            {
                Console.WriteLine("Not enough energy to attack!");
            }
        }
    }

    class Mage : Player
    {
        public Mage(string name) : base(name, 200, 100) { }

        public Mage(Mage other) : base(other) { }

        public override void Attack(Enemy enemy)
        {
            if (Energy >= 15)
            {
                Console.WriteLine($"{Name} casts a spell on the enemy!");
                enemy.TakeDamage(40);
                UseEnergy(15);
            }
            else
            {
                Console.WriteLine("Not enough energy to attack!");
            }
        }
    }

    abstract class Enemy : IDamageable
    {
        public int Health { get; set; }
        public bool IsAlive => Health > 0;

        public Enemy(int health)
        {
            Health = health;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
            Console.WriteLine($"Enemy takes {damage} damage. Remaining health: {Health}");
        }
    }

    class Minion : Enemy
    {
        public Minion() : base(50) { }
    }

    class Boss : Enemy
    {
        public Boss() : base(120) { }
    }

    class NPC
    {
        public void OfferQuest()
        {
            Console.WriteLine("Defeat the minions to summon the boss!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose your character: 1. Warrior 2. Mage");
            int choice = int.Parse(Console.ReadLine());
            Player player = choice switch
            {
                1 => new Warrior("Warrior Hero"),
                2 => new Mage("Mage Hero"),
                _ => throw new InvalidOperationException("Invalid choice!")
            };

            NPC npc = new NPC();
            npc.OfferQuest();

            Item hpBooster = new Item("HP Booster", 20, p => p.Health += 20);
            Item attackBooster = new Item("Attack Booster", 10, p => p.Energy += 10);
            Item energyBooster = new Item("Energy Booster", 15, p => p.Energy += 15);

            Minion minion = new Minion();
            while (minion.IsAlive)
            {
                Console.WriteLine("1. Attack 2. Inventory");
                int action = int.Parse(Console.ReadLine());

                if (action == 1)
                {
                    player.Attack(minion);
                    if (minion.IsAlive)
                    {
                        Console.WriteLine("Minion attacks back!");
                        player.Health -= 10;
                        player.ClampHealth();  // Clamp health to 0 if it goes below 0
                        Console.WriteLine($"{player.Name} health: {player.Health}, Energy: {player.Energy}, XP: {player.XP}");
                    }
                    else
                    {
                        Console.WriteLine("Minion defeated!");
                        player.GainXP(50);
                        Console.WriteLine($"Rank updated to {player.Rank}");
                    }
                }
                else if (action == 2)
                {
                    InventoryMenu(player, hpBooster, attackBooster, energyBooster);
                }
            }

            Boss boss = new Boss();
            Console.WriteLine("The Boss has appeared!");

            while (boss.IsAlive && player.Health > 0)
            {
                Console.WriteLine("1. Attack 2. Inventory");
                int action = int.Parse(Console.ReadLine());

                if (action == 1)
                {
                    player.Attack(boss);
                    if (boss.IsAlive)
                    {
                        Console.WriteLine("Boss attacks back!");
                        player.Health -= 30;
                        player.ClampHealth();  // Clamp health to 0 if it goes below 0
                        Console.WriteLine($"{player.Name} health: {player.Health}, Energy: {player.Energy}, XP: {player.XP}");
                    }
                    else
                    {
                        Console.WriteLine("Boss defeated! You win!");
                        return;
                    }
                }
                else if (action == 2)
                {
                    InventoryMenu(player, hpBooster, attackBooster, energyBooster);
                }
            }

            if (player.Health <= 0)
            {
                Console.WriteLine("You have been defeated. Game over!");
            }
        }

        static void InventoryMenu(Player player, Item hpBooster, Item attackBooster, Item energyBooster)
        {
            if (player.Inventory.IsEmpty)
            {
                Console.WriteLine("No items in inventory.");
                Console.WriteLine("1. Buy");
                int action = int.Parse(Console.ReadLine());
                if (action == 1) BuyItem(player, hpBooster, attackBooster, energyBooster);
            }
            else
            {
                Console.WriteLine("1. Buy 2. Sell");
                int action = int.Parse(Console.ReadLine());
                if (action == 1) BuyItem(player, hpBooster, attackBooster, energyBooster);
                else if (action == 2) SellItem(player);
            }
        }

        static void BuyItem(Player player, Item hpBooster, Item attackBooster, Item energyBooster)
        {
            if (player.Inventory.IsFull)
            {
                Console.WriteLine("Inventory is full. Sell an item first!");
                return;
            }

            Console.WriteLine("Available items to buy:");
            Console.WriteLine("1. HP Booster");
            Console.WriteLine("2. Attack Booster");
            Console.WriteLine("3. Energy Booster");
            int choice = int.Parse(Console.ReadLine());

            Item selectedItem = choice switch
            {
                1 => hpBooster,
                2 => attackBooster,
                3 => energyBooster,
                _ => null
            };

            if (selectedItem != null)
            {
                player.Inventory.AddItem(selectedItem);
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        static void SellItem(Player player)
        {
            if (player.Inventory.IsEmpty)
            {
                Console.WriteLine("No items to sell.");
                return;
            }

            player.Inventory.DisplayItems();
            Console.WriteLine("Select an item to sell:");
            int choice = int.Parse(Console.ReadLine());

            if (choice > 0 && choice <= player.Inventory.Items.Count)
            {
                player.Inventory.RemoveItem(player.Inventory.Items[choice - 1]);
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }
    }
}
