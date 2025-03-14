namespace VendingMachineLibrary.Tests.Mocks;

using System.Collections.Generic;
using System.ComponentModel;
using VendingMachineLibrary.DataAccess;
using VendingMachineLibrary.Models;
using Xunit.Abstractions;

public class MockDataAccess : IDataAccess
{
	public List<CoinModel> CoinInventory { get; set; } = new List<CoinModel>();
	public (decimal CoinCurrent, decimal CoinTotal) MachineInfo { get; set; }
	public List<ItemModel> ItemInventory { get; set; } = new List<ItemModel>();
	public Dictionary<string, decimal> UserCredit { get; set; } = new Dictionary<string, decimal>();

	public MockDataAccess()
	{
		CoinInventory.Add(new CoinModel { Name = "Penny", Value = 0.01m });
		CoinInventory.Add(new CoinModel { Name = "Penny", Value = 0.01m });

		CoinInventory.Add(new CoinModel { Name = "Quarter", Value = 0.25m });
		CoinInventory.Add(new CoinModel { Name = "Quarter", Value = 0.25m });
		CoinInventory.Add(new CoinModel { Name = "Quarter", Value = 0.25m });

		CoinInventory.Add(new CoinModel { Name = "Dollar", Value = 0.50m });
		CoinInventory.Add(new CoinModel { Name = "Dollar", Value = 0.50m });

		MachineInfo = (0m, 210.10m);

		ItemInventory.Add(new ItemModel { Name = "Pepsi", Price = 1.99m, Slot = "1" });
		ItemInventory.Add(new ItemModel { Name = "Pepsi", Price = 1.99m, Slot = "1" });
		ItemInventory.Add(new ItemModel { Name = "Pepsi", Price = 1.99m, Slot = "1" });

		ItemInventory.Add(new ItemModel { Name = "Diet Pepsi", Price = 2.49m, Slot = "2" });

		ItemInventory.Add(new ItemModel { Name = "Sprite", Price = 1.79m, Slot = "3" });
		ItemInventory.Add(new ItemModel { Name = "Sprite", Price = 1.79m, Slot = "3" });
	}

	public void CoinInventory_AddCoin(List<CoinModel> coin)
	{
		CoinInventory.AddRange(coin);
	}

	public void CoinInventory_Clear()
	{
		CoinInventory.Clear();
	}

	public List<CoinModel> CoinInventory_GetAll()
	{
		return CoinInventory;
	}

	public List<CoinModel> CoinInventory_GetSpecificDenomination(decimal coinValue, int quantity)
	{
		var coins = CoinInventory.Where(x => x.Value == coinValue).Take(quantity).ToList();
		coins.ForEach(x => CoinInventory.Remove(x));

		return coins;
	}

	public void ItemInventory_AddItems(List<ItemModel> items)
	{
		ItemInventory.AddRange(items);
	}

	public void ItemInventory_Clear()
	{
		ItemInventory.Clear();
	}

	public List<ItemModel> ItemInventory_GetAll()
	{
		return ItemInventory;
	}

	public List<ItemModel> ItemInventory_GetDistinctTypes()
	{
		return ItemInventory.GroupBy(x => x.Name).Select(x => x.First()).ToList();
	}

	public ItemModel ItemInventory_GetItem(ItemModel item)
	{
		return ItemInventory.FirstOrDefault(x => x.Name == item.Name && x.Slot == item.Slot);
	}

	public void ItemInventory_RemoveItems(List<ItemModel> items)
	{
		foreach (var item in items)
		{
			var itemToRemove = ItemInventory.FirstOrDefault(x => x.Name == item.Name && x.Slot == item.Slot);
			if (itemToRemove != null)
			{
				ItemInventory.Remove(itemToRemove);
			}
		}
	}

	public decimal MachineInfo_CurrentCoins()
	{
		return MachineInfo.CoinCurrent;
	}

	public decimal MachineInfo_EmptyCoins()
	{
		var currentMachineInfo = MachineInfo;
		decimal currentCoins = currentMachineInfo.CoinCurrent;

		currentMachineInfo.CoinCurrent = 0;
		MachineInfo = currentMachineInfo;

		return currentCoins;
	}

	public decimal MachineInfo_TotalCoins()
	{
		return MachineInfo.CoinTotal;
	}

	public decimal UserCoin_Balance(Guid userId)
	{
        string userKey = userId.ToString();

        return UserCredit.ContainsKey(userKey) ? UserCredit[userKey] : 0;
	}

	public decimal UserCoin_Deposit(Guid userId, decimal coinValue)
	{
        string userKey = userId.ToString();

        if (UserCredit.ContainsKey(userKey))
		{
			UserCredit[userKey] += coinValue;
		}
		else
		{
			UserCredit[userKey] = coinValue;
		}

		return UserCredit[userKey];
	}

	public void UserCoin_Reset(Guid userId)
	{
        string userKey = userId.ToString();

        if (UserCredit.ContainsKey(userKey))
		{
			UserCredit[userKey] = 0;
		}
	}

	public void CoinInventory_RemoveCoins(List<CoinModel> coins)
	{
		foreach (var coin in coins)
		{
			var coinToRemove = CoinInventory.FirstOrDefault(x => x.Name == coin.Name && x.Value == coin.Value);
			if (coinToRemove != null)
			{
				CoinInventory.Remove(coinToRemove);
			}
		}
	}

	public void MachineInfo_AddIncome(decimal income)
	{
		var currentMachineInfo = MachineInfo;
		decimal currentCoins = currentMachineInfo.CoinCurrent;
		decimal totalCoins = currentMachineInfo.CoinTotal;

		currentMachineInfo.CoinCurrent += income;
		currentMachineInfo.CoinTotal += income;
		MachineInfo = currentMachineInfo;
	}
}
