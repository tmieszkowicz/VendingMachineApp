namespace VendingMachineLibrary.Tests.Mocks;

using System.Collections.Generic;
using VendingMachineLibrary.DataAccess;
using VendingMachineLibrary.Models;
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

		MachineInfo = (22.50m, 210.10m);

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
		return CoinInventory.Where(x => x.Value == coinValue).Take(quantity).ToList();
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
		return ItemInventory.Where(x => x.Name == item.Name).FirstOrDefault();
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

	public decimal UserCoin_Balance(string userId)
	{
		return UserCoin_Balance(userId);
	}

	public decimal UserCoin_Deposit(string userId, decimal coinValue)
	{
		return UserCoin_Deposit(userId,coinValue);
	}

	public void UserCoin_Reset(string userId)
	{
		UserCoin_Reset(userId);
	}
}
