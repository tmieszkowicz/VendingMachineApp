using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using VendingMachineLibrary.BusinessLogic;
using VendingMachineLibrary.DataAccess;

internal class Program
{
	private static IServiceProvider _serviceProvider;

	private static void Main(string[] args)
	{
		RegisterServices();

		string selected = string.Empty;

		var vendingMachine = _serviceProvider.GetService<IVendingMachineLogic>();

		do
		{
			selected = ShowMenu();

			switch (selected)
			{
				case "1"://Show avaliable items
					break;
				case "2"://Insert money
					break;
				case "3"://Money balance
					break;
				case "4"://Purchase
					break;
				case "6"://Cancel transaction
					break;
				case "7"://Request refund
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

	private static string ShowMenu()
	{
		Console.WriteLine("========================================");
		Console.WriteLine("           VENDING MACHINE             ");
		Console.WriteLine("========================================");
		Console.WriteLine("1. Show avaliable items");

		Console.WriteLine("2. Insert money");
		Console.WriteLine("3. Money balance");

		Console.WriteLine("4. Purchase");

		Console.WriteLine("6. Cancel transaction");
		Console.WriteLine("7. Request refund");

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

		collection.AddSingleton(collection);
		collection.AddTransient<IDataAccess, DataAccessTextFile>();
		collection.AddTransient<IVendingMachineLogic, VendingMachineLogic>();

		_serviceProvider = collection.BuildServiceProvider();
	}
}