using System;
using System.Collections.Generic;

public class VendingMachine
{
    private Dictionary<string, int> products;
    private Dictionary<int, int> coins;

    public VendingMachine()
    {
        products = new Dictionary<string, int>
        {
            { "Soda", 150 },
            { "Chips", 100 },
        };

        coins = new Dictionary<int, int>
        {
            { 25, 10 },
            { 50, 5 },
            { 100, 2 }
        };
    }

    public void AddCoins(int denomination, int count)
    {
        if (coins.ContainsKey(denomination))
        {
            coins[denomination] += count;
        }
        else
        {
            Console.WriteLine("Invalid coin denomination.");
        }
    }

    public void PurchaseProduct(string product, int payment)
    {
        if (products.ContainsKey(product))
        {
            int price = products[product];
            if (payment >= price)
            {
                int change = payment - price;
                Console.WriteLine($"Attempting to dispense change: {change} cents.");
                if (GiveChange(change))
                {
                    Console.WriteLine($"Dispensing {product}. Your change is {change} cents.");
                }
                else
                {
                    Console.WriteLine("Unable to provide change. Transaction cancelled.");
                }
            }
            else
            {
                Console.WriteLine($"Insufficient payment. {product} costs {price} cents.");
            }
        }
        else
        {
            Console.WriteLine("Product not available.");
        }
    }

    private bool GiveChange(int change)
    {
        Dictionary<int, int> tempCoins = new Dictionary<int, int>(coins);
        List<int> coinTypes = new List<int> { 100, 50, 25 };

        foreach (var coin in coinTypes)
        {
            while (change >= coin && tempCoins[coin] > 0)
            {
                change -= coin;
                tempCoins[coin]--;
            }
        }

        if (change == 0)
        {
            coins = tempCoins;
            return true;
        }

        return false;
    }

    public void DisplayProducts()
    {
        Console.WriteLine("Available products:");
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Key}: {product.Value} cents");
        }
    }

    public void DisplayCoins()
    {
        Console.WriteLine("Coins available:");
        foreach (var coin in coins)
        {
            Console.WriteLine($"{coin.Key} cents: {coin.Value} coins");
        }
    }

    public static void Main(string[] args)
    {
        VendingMachine vm = new VendingMachine();

        while (true)
        {
            Console.WriteLine("\nWelcome to the Vending Machine!");
            vm.DisplayProducts();
            Console.WriteLine("Enter the name of the product you want to buy:");
            string product = Console.ReadLine();

            Console.WriteLine("Enter the number of coins you want to insert (format: denomination:count, e.g., 25:4 for four 25-cent coins):");
            int totalInserted = 0;
            while (true)
            {
                string coinInput = Console.ReadLine();
                if (coinInput.ToLower() == "done")
                {
                    break;
                }

                var parts = coinInput.Split(':');
                if (parts.Length == 2 && int.TryParse(parts[0], out int denomination) && int.TryParse(parts[1], out int count))
                {
                    vm.AddCoins(denomination, count);
                    totalInserted += denomination * count;
                    Console.WriteLine($"Total inserted: {totalInserted} cents. Type 'done' to finish inserting coins.");
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter the coin denomination and count in the format 'denomination:count' or type 'done' to finish.");
                }
            }

            Console.WriteLine($"Attempting to purchase {product} with {totalInserted} cents.");
            vm.PurchaseProduct(product, totalInserted);
            vm.DisplayCoins();

            Console.WriteLine("Would you like to buy another product? (yes/no)");
            if (Console.ReadLine().ToLower() != "yes")
            {
                break;
            }
        }

        Console.WriteLine("Thank you for using the Vending Machine. Goodbye!");
    }
}
