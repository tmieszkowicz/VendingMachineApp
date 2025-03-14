using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using VendingMachineLibrary.Models;

namespace VendingMachineLibrary.DataAccess;

public class DataAccessTextFile : IDataAccess
{
	private readonly IConfiguration _config;
	private string coinTextFile;
	private string itemTextFile;
	private string machineInfoTextFile;
	private string userInfoTextFile;
	private const char DELIMITER = ';';

	public DataAccessTextFile(IConfiguration config)
    {
		_config = config;
		coinTextFile = _config.GetValue<string>("DataFile:CoinInventory");
		itemTextFile = _config.GetValue<string>("DataFile:ItemInventory");
		machineInfoTextFile = _config.GetValue<string>("DataFile:MachineInfo");
		userInfoTextFile = _config.GetValue<string>("DataFile:UserInfo");
	}
	private List<CoinModel> RetrieveCoins()
	{
		var lines = File.ReadAllLines(coinTextFile);
		var result = new List<CoinModel>();

		try
		{
			foreach (var line in lines)
			{
				var data = line.Split(DELIMITER);

				result.Add(new CoinModel
				{
					Name = data[0],
					Value = decimal.Parse(data[1])
				});
			}
		}
		catch (IndexOutOfRangeException ex)
		{
			throw new Exception("Missing data in the coin text file.", ex);
		}
		catch (FormatException ex)
		{
			throw new Exception("Corrupted data in the coin text file.", ex);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}

		return result;
	}
	private void SaveCoins(List<CoinModel> coins)
	{
		try
		{
			File.WriteAllLines(coinTextFile, coins.Select(x => $"{x.Name}{DELIMITER}{x.Value}"));
		}
		catch (IOException ex)
		{
			throw new Exception("Error during writing data to the coin text file.", ex);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
	private (decimal coinCurrent, decimal coinTotal) RetrieveMachineInfo()
	{
		var line = File.ReadAllLines(machineInfoTextFile).First();

		if (string.IsNullOrWhiteSpace(line))
		{
			throw new Exception("Machine info file is empty or missing data.");
		}

		string[] data = line.Split(DELIMITER);

		if (data.Length < 1 || data.Any(string.IsNullOrWhiteSpace))
		{
			throw new Exception("Machine info file has missing or corrupted data.");
		}

		if (!decimal.TryParse(data[0], out decimal coinCurrent))
		{
			throw new Exception("Failed to parse current coins value from machine info file.");
		}
		if (!decimal.TryParse(data[1], out decimal coinTotal))
		{
			throw new Exception("Failed to parse total coins value from machine info file.");
		}

		return (coinCurrent, coinTotal);
	}
	private Dictionary<string, decimal> RetrieveUserInfo()
	{
		var result = new Dictionary<string, decimal>();

		var lines = File.ReadAllLines(userInfoTextFile);

		try
		{
			foreach (var line in lines)
			{
				string[] data = line.Split(DELIMITER);
				decimal.TryParse(data[1], out decimal currentCredit);
				result.Add(data[0], currentCredit);
			}
		}
		catch (IndexOutOfRangeException)
		{
			throw new Exception("User info file has missing or corrupted data.");
		}
		catch (FormatException)
		{
			throw new Exception("Failed to parse credit value from user info file.");
		}
		catch (Exception ex)
		{
			throw;
		}

		return result;
	}
	private void SaveUserInfo(Guid userId, decimal credit)
	{
		try
		{
			var lines = File.ReadAllLines(userInfoTextFile).ToList();

            var existingLine = lines.FirstOrDefault(line => line.StartsWith(userId.ToString() + DELIMITER));

            if (existingLine != null)
			{
				lines[lines.IndexOf(existingLine)] = $"{userId}{DELIMITER}{credit}";
			}
			else
			{
				lines.Add($"{userId}{DELIMITER}{credit}");
			}

			File.WriteAllLines(userInfoTextFile, lines);
		}
		catch (IOException ex)
		{
			throw new Exception("Error during writing data to the user info text file.", ex);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
	private void SaveMachineInfo(decimal coinCurrent, decimal coinTotal)
	{
		try
		{
			string[] stringToSave = { $"{coinCurrent}{DELIMITER}{coinTotal}"};
			File.WriteAllLines(machineInfoTextFile, stringToSave);
		}
		catch (IOException ex)
		{
			throw new Exception("Error during writing data to the machine info text file.", ex);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
	private List<ItemModel> RetrieveItems()
	{
		var lines = File.ReadAllLines(itemTextFile);
		var result = new List<ItemModel>();

		try
		{
			foreach (var line in lines)
			{
				string[] data = line.Split(DELIMITER);

				result.Add(new ItemModel
				{
					Name = data[0],
					Price = decimal.Parse(data[1]),
					Slot = data[2]
				});
			}
		}
		catch (IndexOutOfRangeException ex)
		{
			throw new Exception("Missing data in the item text file.", ex);
		}
		catch (FormatException ex)
		{
			throw new Exception("Corrupted data in the item text file.", ex);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}

		return result;
	}
	private void SaveItems(List<ItemModel> items)
	{
		try
		{
			File.AppendAllLines(itemTextFile, items.Select(x => $"{x.Name}{DELIMITER}{x.Price}{DELIMITER}{x.Slot}"));
		}
		catch (IOException ex)
		{
			throw new Exception("Error during writing data to the item text file.", ex);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
	public void CoinInventory_AddCoin(List<CoinModel> coins)
	{
		coins.AddRange(RetrieveCoins());
		SaveCoins(coins);
	}
	public void CoinInventory_Clear()
	{
		SaveCoins(new List<CoinModel>());
	}
	public List<CoinModel> CoinInventory_GetAll()
	{
		return RetrieveCoins();
	}
	public List<CoinModel> CoinInventory_GetSpecificDenomination(decimal CoinValue, int quantity = 1)
	{
		var coins = RetrieveCoins();
		var result = coins.Where(x => x.Value == CoinValue).Take(quantity).ToList();

		if (result.Count < quantity)
		{
			throw new InvalidOperationException("Not enough coins of the specified denomination available.");
		}
		
		result.ForEach(x => coins.Remove(x));

		SaveCoins(coins);

		return result;
	}
	public void CoinInventory_RemoveCoins(List<CoinModel> coins)
	{
		var coinStock = RetrieveCoins();

		foreach (var coin in coins)
		{
			var match = coinStock.FirstOrDefault(x => x.Name == coin.Name && x.Value == coin.Value);
			if (match != null)
			{
				coinStock.Remove(match);
			}
			else
			{
				throw new Exception($"Coin {coin.Name} of value {coin.Value} not found in inventory.");
			}
		}

		SaveCoins(coinStock);
	}
	public void ItemInventory_AddItems(List<ItemModel> items)
	{
		items.AddRange(RetrieveItems());
		SaveItems(items);
	}
	public void ItemInventory_Clear()
	{
		SaveItems(new List<ItemModel>());
	}
	public List<ItemModel> ItemInventory_GetAll()
	{
		return RetrieveItems();
	}
	public List<ItemModel> ItemInventory_GetDistinctTypes()
	{
		var items = RetrieveItems();
		var result = items.GroupBy(x=>x.Name).Select(x=>x.First()).ToList();

		return result;
	}
	public ItemModel ItemInventory_GetItem(ItemModel item)
	{
		var items = ItemInventory_GetAll();

		var result =  items.FirstOrDefault(x => x.Name == item.Name && x.Slot == item.Slot);

		if (result != null)
		{
			MachineInfo_AddIncome(item.Price);

		}

		return result;
	}
	public void ItemInventory_RemoveItems(List<ItemModel> items)
	{
		var stock = RetrieveItems();

		foreach (var item in items)
		{
			var match = stock.FirstOrDefault(x =>
						x.Name == item.Name &&
						x.Price == item.Price &&
						x.Slot == item.Slot);

			if (match != null)
			{
				stock.Remove(match);
			}
		}

		SaveItems(stock);
	}
	public void MachineInfo_AddIncome(decimal income)
	{
		var info = RetrieveMachineInfo();

		info.coinCurrent += income;
		info.coinTotal += income;

		SaveMachineInfo(info.coinCurrent, info.coinTotal);
	}
	public decimal MachineInfo_CurrentCoins()
	{
		var info = RetrieveMachineInfo();

		return info.coinCurrent;
	}
	public decimal MachineInfo_EmptyCoins()
	{
		var info = RetrieveMachineInfo();

		SaveMachineInfo(0, info.coinTotal);
		return info.coinCurrent;
	}
	public decimal MachineInfo_TotalCoins()
	{
		var info = RetrieveMachineInfo();

		return info.coinTotal;
	}
	public decimal UserCoin_Balance(Guid userId)
	{
		Dictionary<string, decimal> userInfo = RetrieveUserInfo();

		if(userInfo.TryGetValue(userId.ToString(), out var balance))
			return balance;

        SaveUserInfo(userId, 0m);
		return 0m;
	}
	public decimal UserCoin_Deposit(Guid userId, decimal coinValue)
	{
		Dictionary<string, decimal> userInfo = RetrieveUserInfo();

		decimal currentCoins = UserCoin_Balance(userId);
		currentCoins += coinValue;
		SaveUserInfo(userId, currentCoins);

		return currentCoins;

		throw new Exception("User not found.");
	}
	public void UserCoin_Reset(Guid userId)
	{
		SaveUserInfo(userId, 0);
	}
}
