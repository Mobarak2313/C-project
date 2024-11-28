using System;

// ICombat Interface for Combat Behavior
interface ICombat
{
    void Attack(Enemy enemy);
    void TakeDamage(int damage);
}

// Base Player Class with Virtual Methods
abstract class Player : ICombat
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Energy { get; set; }
    public int XP { get; set; }
    public string Rank { get; set; }
    public static int PlayerCount { get; set; } // Static Field

    public Player(string name)
    {
        Name = name;
        Health = 100;
        Energy = 100;
        XP = 0;
        Rank = "Novice";
        PlayerCount++; // Increment player count on creation
    }

    public Player(string name, int health, int energy, int xp)
    {
        Name = name;
        Health = health;
        Energy = energy;
        XP = xp;
        Rank = "Novice";
        PlayerCount++; // Increment player count on creation
    }

    public abstract void Attack(Enemy enemy);
    
    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Console.WriteLine($"{Name} has been defeated.");
        }
    }

    public void UseEnergy(int amount)
    {
        if (Energy >= amount)
        {
            Energy -= amount;
        }
    }

    public void UpdateRank()
    {
        Rank = "Apprentice";
        Console.WriteLine($"{Name} has been promoted to {Rank}!");
    }

    public static void ShowGameInfo() // Static Method
    {
        Console.WriteLine("Welcome to the RPG Game!");
    }

    // Copy Constructor
    public Player(Player other)
    {
        Name = other.Name;
        Health = other.Health;
        Energy = other.Energy;
        XP = other.XP;
        Rank = other.Rank;
    }

    public override string ToString() => $"{Name} - {Rank} - Health: {Health}, Energy: {Energy}, XP: {XP}";
}

// Warrior Class - Inherits Player
class Warrior : Player
{
    public Warrior(string name) : base(name) { }

    // Method Overriding
    public override void Attack(Enemy enemy)
    {
        if (Energy >= 20)
        {
            UseEnergy(20);
            Console.WriteLine($"{Name} strikes {enemy.Name} with a sword!");
            enemy.TakeDamage(15);
        }
        else
        {
            Console.WriteLine($"{Name} does not have enough energy to attack!");
        }
    }

    // Overloaded Attack Method
    public void Attack(Enemy enemy, int extraDamage)
    {
        if (Energy >= 20)
        {
            UseEnergy(20);
            Console.WriteLine($"{Name} strikes {enemy.Name} with a powerful sword strike!");
            enemy.TakeDamage(15 + extraDamage); // Extra damage
        }
        else
        {
            Console.WriteLine($"{Name} does not have enough energy to attack!");
        }
    }
}

// Mage Class - Inherits Player
class Mage : Player
{
    public Mage(string name) : base(name) { }

    // Method Overriding
    public override void Attack(Enemy enemy)
    {
        if (Energy >= 10)
        {
            UseEnergy(10);
            Console.WriteLine($"{Name} casts a fireball at {enemy.Name}!");
            enemy.TakeDamage(25);
        }
        else
        {
            Console.WriteLine($"{Name} does not have enough energy to attack!");
        }
    }

    // Overloaded Attack Method
    public void Attack(Enemy enemy, int extraDamage)
    {
        if (Energy >= 25)
        {
            UseEnergy(25);
            Console.WriteLine($"{Name} casts an enhanced fireball at {enemy.Name}!");
            enemy.TakeDamage(25 + extraDamage); // Extra damage
        }
        else
        {
            Console.WriteLine($"{Name} does not have enough energy to attack!");
        }
    }
}

// Enemy Class
abstract class Enemy
{
    public string Name { get; set; }
    public int Health { get; set; }

    protected Enemy(string name, int health)
    {
        Name = name;
        Health = health;
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
       /* if (Health <= 0)
        {
            Console.WriteLine($"{Name} has been defeated!");
        } */
    }
}

// Minion Class - Inherits Enemy
class Minion : Enemy
{
    public Minion() : base("Minion", 30) { }
}

// Boss Class - Inherits Enemy
class Boss : Enemy
{
    public Boss() : base("Boss", 120) { }

    // Overriding TakeDamage method
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (Health <= 0)
        {
            Console.WriteLine("The Boss has been defeated!");
        }
    }

    public void Attack(Player player)
    {
        Console.WriteLine($"{Name} attacks {player.Name}!");
        player.TakeDamage(20);
    }
}

// NPC Class
class NPC
{
    public string Name { get; set; }

    public NPC(string name)
    {
        Name = name;
    }

    public void OfferQuest(Player player)
    {
        Console.WriteLine($"{Name} offers you a quest: Defeat the Minion before facing the Boss!");
    }
}

// Main Program
class Program
{
    static void Main(string[] args)
    {
        // Show Game Info (Static Method)
        Player.ShowGameInfo();

        Console.WriteLine("Choose your character: 1. Warrior  2. Mage");
        int choice = int.Parse(Console.ReadLine());
        Player player = null;

        if (choice == 1)
        {
            player = new Warrior("Warrior Hero");
            Console.WriteLine("You have chosen Warrior. Complete the mission to proceed!");
        }
        else if (choice == 2)
        {
            player = new Mage("Mage Hero");
            Console.WriteLine("You have chosen Mage. Complete the mission to proceed!");
        }

        Console.WriteLine($"\nPlayer Stats: {player}");

        // NPC Mission
        NPC npc = new NPC("Quest Giver");
        npc.OfferQuest(player);

        // Fighting Minion
        Minion minion = new Minion();
        Console.WriteLine("\nA Minion appears!");
        while (minion.Health > 0 && player.Health > 0)
        {
            player.Attack(minion);
            if (minion.Health > 0)
            {
                Console.WriteLine("Minion retaliates!");
                player.TakeDamage(5);
            }
        }

        if (player.Health <= 0)
        {
            Console.WriteLine("You have failed the mission. Game Over!");
            return;
        }

        // Update Rank after defeating Minion
        player.UpdateRank();

        Console.WriteLine("\nYou completed the mission! The Boss now appears!");

        // Automatic Boss Fight
        Boss boss = new Boss();
        Console.WriteLine("\nThe Boss appears!");

        if (choice == 1)
        {
            // Warrior's Outcome
            while (boss.Health > 0 && player.Health > 0)
            {
                if (player.Energy > 0)
                {
                    player.Attack(boss);
                }
                else
                {
                    Console.WriteLine($"{player.Name} has no energy left to attack!");
                    break;
                }

                if (boss.Health > 0)
                {
                    boss.Attack(player);
                }
            }

            Console.WriteLine("Despite your bravery, the Warrior has been defeated by the Boss. Better luck next time!");
        }
        else if (choice == 2)
        {
            // Mage's Outcome
            while (boss.Health > 0 && player.Health > 0)
            {
                player.Attack(boss);

                if (boss.Health > 0)
                {
                    boss.Attack(player);
                }
            }

            if (boss.Health <= 0)
            {
                Console.WriteLine("Congratulations! You won the battle  and saved the day!");
            }
        }

        Console.WriteLine("\nGame Over. Thanks for playing!");
    }
}
