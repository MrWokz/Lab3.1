using System;
using System.Collections.Generic;

// Базовий клас Живий організм
public abstract class LivingOrganism
{
    public double Energy { get; set; }
    public int Age { get; set; }
    public double Size { get; set; }

    public LivingOrganism(double energy, int age, double size)
    {
        Energy = energy;
        Age = age;
        Size = size;
    }

    public virtual void AgeOneYear()
    {
        Age++;
        Energy -= 10; // Втрата енергії з віком
    }

    public bool IsAlive() => Energy > 0;
}

// Інтерфейс для розмноження
public interface IReproducible
{
    void Reproduce();
}

// Інтерфейс для полювання
public interface IPredator
{
    void Hunt(List<LivingOrganism> prey);
}

// Спадкоємний клас Тварина
public class Animal : LivingOrganism, IPredator, IReproducible
{
    public string Species { get; set; }

    public Animal(double energy, int age, double size, string species)
        : base(energy, age, size)
    {
        Species = species;
    }

    public void Hunt(List<LivingOrganism> prey)
    {
        if (prey.Count > 0)
        {
            // Полювання на організм
            var preyToHunt = prey[0];
            if (preyToHunt is Plant || preyToHunt is Animal)
            {
                Console.WriteLine($"The {Species} is hunting a {preyToHunt.GetType().Name}.");
                Energy += 20; // Збільшення енергії після полювання
                prey.RemoveAt(0); // Видалення жертви з екосистеми
            }
        }
        else
        {
            Console.WriteLine($"The {Species} found no prey.");
        }
    }

    public void Reproduce()
    {
        Console.WriteLine($"The {Species} is reproducing.");
        var newAnimal = new Animal(50, 0, 1, Species);
        Console.WriteLine($"A new {Species} has been born.");
    }
}

// Спадкоємний клас Рослина
public class Plant : LivingOrganism, IReproducible
{
    public string Type { get; set; }

    public Plant(double energy, int age, double size, string type)
        : base(energy, age, size)
    {
        Type = type;
    }

    public void Reproduce()
    {
        Console.WriteLine($"The {Type} is reproducing.");
        var newPlant = new Plant(50, 0, 1, Type);
        Console.WriteLine($"A new {Type} has grown.");
    }
}

// Спадкоємний клас Мікроорганізм
public class Microorganism : LivingOrganism
{
    public string Habitat { get; set; }

    public Microorganism(double energy, int age, double size, string habitat)
        : base(energy, age, size)
    {
        Habitat = habitat;
    }

    public void AbsorbNutrients()
    {
        Energy += 5; // Збільшення енергії від поглинання поживних речовин
        Console.WriteLine("The microorganism absorbed nutrients and gained energy.");
    }
}

// Клас Екосистема
public class Ecosystem
{
    public List<LivingOrganism> Organisms { get; set; }

    public Ecosystem()
    {
        Organisms = new List<LivingOrganism>();
    }

    public void AddOrganism(LivingOrganism organism)
    {
        Organisms.Add(organism);
    }

    public void SimulateDay()
    {
        foreach (var organism in Organisms)
        {
            organism.AgeOneYear();
            if (organism.IsAlive())
            {
                if (organism is IReproducible reproducible)
                {
                    reproducible.Reproduce();
                }

                if (organism is IPredator predator)
                {
                    predator.Hunt(Organisms);
                }

                if (organism is Microorganism micro)
                {
                    micro.AbsorbNutrients();
                }
            }
        }

        // Видалення організмів, які втрачають енергію до нуля
        Organisms.RemoveAll(o => !o.IsAlive());
    }

    public void DisplayStatus()
    {
        Console.WriteLine("Current status of the ecosystem:");
        foreach (var organism in Organisms)
        {
            Console.WriteLine($"{organism.GetType().Name} | Energy: {organism.Energy}, Age: {organism.Age}, Size: {organism.Size}");
        }
    }
}

// Демонстрація роботи
class Program
{
    static void Main(string[] args)
    {
        var ecosystem = new Ecosystem();

        var lion = new Animal(100, 5, 50, "Lion");
        var deer = new Animal(80, 3, 40, "Deer");
        var oak = new Plant(200, 10, 100, "Oak Tree");
        var bacterium = new Microorganism(50, 1, 0.01, "Soil");

        ecosystem.AddOrganism(lion);
        ecosystem.AddOrganism(deer);
        ecosystem.AddOrganism(oak);
        ecosystem.AddOrganism(bacterium);

        ecosystem.DisplayStatus();
        ecosystem.SimulateDay();
        ecosystem.DisplayStatus();
    }
}
