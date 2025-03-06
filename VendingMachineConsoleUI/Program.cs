using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using VendingMachineLibrary.BusinessLogic;
using VendingMachineLibrary.DataAccess;
using VendingMachineLibrary.Models;

internal class Program
{
	private static IServiceProvider _serviceProvider;
	private static IVendingMachineLogic _vendingMachine;
	private static string userId;

	private static void Main(string[] args)
	{
        RegisterServices();

		string selected = string.Empty;

		_vendingMachine = _serviceProvider.GetService<IVendingMachineLogic>();
		userId = new Guid().ToString();

		do
		{
			selected = ShowMenu();

			switch (selected)
			{
				case "1"://Show avaliable items
					ShowItems();
					break;
				case "2"://Insert money
					InsertMoney();
					break;
				case "3"://Money balance
					ShowMoneyBalance();
					break;
				case "4"://Purchase
					PurchaseItem();
					break;
				case "6"://Request refund
                    RequestRefund();
					break;
				case "7":
					foreach (var item in _vendingMachine.GetCoinInventory())
					{
                        Console.WriteLine(item.Value);
					}
					Console.ReadLine();
					break;
				case "9"://Exit the application
					break;

				default:
					break;
			}
			Console.Clear();

		} while (selected != "9");
		Console.WriteLine("Have a good one!");
		Console.ReadLine();
	}

    private static void PurchaseItem()
    {
        var items = _vendingMachine.GetItemInventory().DistinctBy(name => name.Name).ToList();

		items.ForEach(item=>  Console.WriteLine($"{item.Slot} - {item.Name.PadRight(20)} {item.Price,0:C}"));

        Console.WriteLine("\nWhich item would you like?");

		string itemIdentifier = Console.ReadLine();
		bool isValidItendifier = int.TryParse(itemIdentifier, out int itemNumber);
		ItemModel requestItem;

		try
		{
			requestItem = items[itemNumber - 1];
		}
		catch
		{
			Console.WriteLine("\nPlease enter a valid number.");

			Console.WriteLine("\nPress anything to continue...\n");
			Console.ReadLine();
			return;
		}

        var results = _vendingMachine.RequestItem(requestItem, userId);

		if (results.errorMessage != null)
		{
			Console.WriteLine(results.errorMessage);
		}
		else
		{	
			Console.WriteLine($"Item {requestItem.Name} dispensed. Enjoy!");
			if (results.change.Count > 0)
			{
                Console.WriteLine("Dispensing change.");

				results.change.ForEach(item => Console.WriteLine($"{item.Name}"));
            }
			else
			{
				Console.WriteLine("There is no change");
			}
		}


        Console.WriteLine("\nPress anything to continue...\n");
        Console.ReadLine();
    }

    private static void RequestRefund()
    {
        var balance = _vendingMachine.GetTotalInsertedCoin(userId);
        _vendingMachine.RequestCoinRefund(userId);
        Console.WriteLine($"You refunded {balance,0:C}");

        Console.WriteLine("\nPress anything to continue...\n");
        Console.ReadLine();
    }

    private static void ShowMoneyBalance()
    {
		var balance = _vendingMachine.GetTotalInsertedCoin(userId);
		Console.WriteLine($"Your balance is {balance,0:C}");

        Console.WriteLine("\nPress anything to continue...\n");
        Console.ReadLine();
    }

    private static void InsertMoney()
    {
        Console.WriteLine("How much would you like to add?");
		string amountText = Console.ReadLine();

		bool isValidAmount = decimal.TryParse(amountText, out decimal amount);
		_vendingMachine.InsertCoin(userId, amount);

        Console.WriteLine("\nPress anything to continue...\n");
        Console.ReadLine();
    }

    private static void ShowItems()
    {
        var items = _vendingMachine.GetItemInventory();

        foreach (var item in items.DistinctBy(name => name.Name))
        {
            Console.WriteLine($"{item.Name.PadRight(20)} {item.Price,0:C}");
        }

        Console.WriteLine("\nPress anything to continue...\n");
		Console.ReadLine();
    }

    private static string ShowMenu()
	{
		Console.WriteLine("██    ██ ███████ ███    ██ ██████  ██ ███    ██  ██████      ███    ███  █████   ██████ ██   ██ ██ ███    ██ ███████ \r\n██    ██ ██      ████   ██ ██   ██ ██ ████   ██ ██           ████  ████ ██   ██ ██      ██   ██ ██ ████   ██ ██      \r\n██    ██ █████   ██ ██  ██ ██   ██ ██ ██ ██  ██ ██   ███     ██ ████ ██ ███████ ██      ███████ ██ ██ ██  ██ █████   \r\n ██  ██  ██      ██  ██ ██ ██   ██ ██ ██  ██ ██ ██    ██     ██  ██  ██ ██   ██ ██      ██   ██ ██ ██  ██ ██ ██      \r\n  ████   ███████ ██   ████ ██████  ██ ██   ████  ██████      ██      ██ ██   ██  ██████ ██   ██ ██ ██   ████ ███████ \r\n                                                                                                                     \r\n                                                                                                                     ");
		Console.WriteLine("========================================");
		Console.WriteLine("1. Show avaliable items");

		Console.WriteLine("2. Insert money");
		Console.WriteLine("3. Money balance");

		Console.WriteLine("4. Purchase");

		Console.WriteLine("6. Request refund");

		Console.WriteLine("9. Exit the application");
		Console.WriteLine("========================================");
		Console.WriteLine("Please select an option :");

		return Console.ReadLine();
	}


	private static void RegisterServices()
	{
		var collection = new ServiceCollection();

		IConfiguration config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", true, true)
			.Build();

		collection.AddSingleton(config);
		collection.AddTransient<IDataAccess, DataAccessTextFile>();
		collection.AddTransient<IVendingMachineLogic, VendingMachineLogic>();

		_serviceProvider = collection.BuildServiceProvider();
	}
}