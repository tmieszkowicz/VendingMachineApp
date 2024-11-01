using Microsoft.VisualStudio.TestPlatform.Utilities;
using VendingMachineLibrary.DataAccess;
using VendingMachineLibrary.Models;

namespace VendingMachineLibrary.Tests.Mocks;

public class DataAccess : IDataAccess
{
	public List<MoneyModel> MoneyInventory { get; set; } = new List<MoneyModel>();
	public (decimal itemPrice, decimal moneyCurrent, decimal moneyTotal) MachineInfo { get; set; }
    public List<ItemModel> ItemInventory{ get; set; } = new List<ItemModel>();
	public Dictionary<string, decimal> UserCredit { get; set; } = new Dictionary<string, decimal>();

    public DataAccess()
    {
		// { Name = "Penny", Value = 0.01m },
		// { Name = "Nickel", Value = 0.05m },
		// { Name = "Dime", Value = 0.10m },
		// { Name = "Quarter", Value = 0.25m },
		// { Name = "Half Dollar", Value = 0.50m },
		// { Name = "Dollar Coin", Value = 1.00m },
	
		// { Name = "One Dollar", Value = 1.00m },
		// { Name = "Five Dollar", Value = 5.00m },
		// { Name = "Ten Dollar", Value = 10.00m },
		// { Name = "Twenty Dollar", Value = 20.00m },
		// { Name = "Fifty Dollar", Value = 50.00m },
		// { Name = "Hundred Dollar", Value = 100.00m }

		MoneyInventory.Add(new MoneyModel { Name = "Penny", Value = 0.01m });
		MoneyInventory.Add(new MoneyModel { Name = "Penny", Value = 0.01m });

		MoneyInventory.Add(new MoneyModel { Name = "Quarter", Value = 0.25m });
		MoneyInventory.Add(new MoneyModel { Name = "Quarter", Value = 0.25m });
		MoneyInventory.Add(new MoneyModel { Name = "Quarter", Value = 0.25m });

		MoneyInventory.Add(new MoneyModel { Name = "Half Dollar", Value = 0.50m });
		MoneyInventory.Add(new MoneyModel { Name = "Half Dollar", Value = 0.50m });

		MachineInfo = (0.79m, 22.50m, 210.10m);

		ItemInventory.Add(new ItemModel { Name = "Pepsi", Slot = "1" });
		ItemInventory.Add(new ItemModel { Name = "Pepsi", Slot = "1" });
		ItemInventory.Add(new ItemModel { Name = "Pepsi", Slot = "1" });

		ItemInventory.Add(new ItemModel { Name = "Diet Pepsi", Slot = "2" });

		ItemInventory.Add(new ItemModel { Name = "Sprite", Slot = "3" });
		ItemInventory.Add(new ItemModel { Name = "Sprite", Slot = "3" });
    }

    public void ItemInventory_AddItem(ItemModel item)
	{
		ItemInventory.Add(item);
	}

	public void ItemInventory_AddItems(List<ItemModel> items)
	{
		ItemInventory.AddRange(items);
	}

	public List<ItemModel> ItemInventory_GetAll()
	{
		return ItemInventory;
	}

	public ItemModel ItemInventory_GetItem(ItemModel item)
	{
		return ItemInventory.Where(x => x.Name == item.Name).FirstOrDefault();
	}

	public List<ItemModel> ItemInventory_GetTypes()
	{
		return ItemInventory.GroupBy(x => x.Name)
							.Select(x => x.First())
							.ToList();
	}

	public decimal MachineInfo_CurrentIncome()
	{
		return MachineInfo.moneyCurrent;
	}

	public decimal MachineInfo_EmptyMoney()
	{
		decimal output = MachineInfo.moneyCurrent;

		var value = MachineInfo;
		value.moneyCurrent = 0;
		MachineInfo = value;

		return output;
	}

	public decimal MachineInfo_ItemPrice()
	{
		return MachineInfo.itemPrice;
	}

	public decimal MachineInfo_TotalIncome()
	{
		return MachineInfo.moneyTotal;
	}

	public void MoneyInventory_AddMoney(List<MoneyModel> money)
	{
		MoneyInventory.AddRange(money);
	}

	public List<MoneyModel> MoneyInventory_GetAll()
	{
		return MoneyInventory;
	}

	public List<MoneyModel> MoneyInventory_WithdrawMoney(decimal moneyValue, int quantity)
	{
		var money = MoneyInventory.Where(x => x.Value == moneyValue).Take(quantity).ToList();

		money.ForEach(x => MoneyInventory.Remove(x));

		return money;
	}

	public void UserMoney_Clear(string userId)
	{
		if (UserCredit.ContainsKey(userId))
		{
			UserCredit[userId] = 0;
		}
	}

	public void UserMoney_Deposit(string userId)
	{
		if (UserCredit.ContainsKey(userId) == false)
		{
			throw new Exception("Deposit: User not found.");
		}

		var info = MachineInfo;
		info.moneyCurrent += UserCredit[userId];
		info.moneyTotal += UserCredit[userId];

		UserCredit.Remove(userId);
	}

	public void UserMoney_Insert(string userId, decimal amount)
	{
		UserCredit.TryGetValue(userId, out decimal moneyCurrent);
		UserCredit[userId] = moneyCurrent + amount;
	}

	public decimal UserMoney_Total(string userId)
	{
		UserCredit.TryGetValue(userId, out decimal moneyCurrent);
		return moneyCurrent;
	}
}
