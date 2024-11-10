using VendingMachineLibrary.BusinessLogic;
using VendingMachineLibrary.Models;
using VendingMachineLibrary.Tests.Mocks;

namespace VendingMachineLibrary.Tests;

public class VendingMachineLogicTests
{
	[Fact]
	public void AddToCoinInventory_AddsCoinsToInventory()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);
		List<CoinModel> coin = new List<CoinModel>
		{
			new CoinModel{ Name = "Quarter", Value = 0.25m},
			new CoinModel{ Name = "Quarter", Value = 0.25m},
			new CoinModel{ Name = "Dime", Value = 0.1m},
		};

		logic.AddToCoinInventory(coin);

		int expected = 5;
		int actual = dataAccess.CoinInventory.Where(x => x.Name == "Quarter").Count();

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void AddToItemInventory_AddsItemsToInventory()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);
		List<ItemModel> items = new List<ItemModel>
		{
			new ItemModel { Name = "Pepsi", Price = 1.99m, Slot = "1" },
			new ItemModel { Name = "Pepsi", Price = 1.99m, Slot = "1" }
		};

		logic.AddToItemInventory(items);

		int expected = 8;
		int actual = dataAccess.ItemInventory.Count;

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void CalculateChange_ReturnsCorrectChange()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		List<CoinModel> coins = logic.CalculateChange(2.00m, 1.99m);

		Assert.Equal(coins.FirstOrDefault().Name, "Penny");
	}

	[Fact]
	public void DispenseChange_DispensesCorrectChange()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		List<CoinModel> coins = logic.CalculateChange(2.00m, 1.99m);

		logic.DispenseChange(coins);

		int actual = dataAccess.ItemInventory.Count;
		int expected = 6;

		Assert.Equal(expected,actual);
	}

	[Fact]
	public void GetCoinInventory_ReturnsCorrectAmountOfCoins()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		int expected = dataAccess.CoinInventory.Count;
		var actual = logic.GetCoinInventory().Count;

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void GetCoinsByName_ReturnsCoinsOfSpecificName()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		var money = logic.GetCoinInventory();

		int expected = dataAccess.CoinInventory.Where(x => x.Name == "Quarter").Count();
		var actual = money.Where(x => x.Name == "Quarter").Count();

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void GetCurrentIncome_ReturnsCurrentIncome()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		var (expected, _) = dataAccess.MachineInfo;
		decimal actual = logic.GetCurrentIncome();

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void GetItemInventory_ReturnsCountOfItems()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		var items = logic.GetItemInventory();

		int expected = 6;
		int actual = logic.GetItemInventory().Count;

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void GetItemsByName_ReturnsItemsOfSpecificName()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		var items = logic.GetItemsByName("Pepsi");

		int expected = 3;
		int actual = items.Count;

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void GetTotalIncome_ReturnsTotalIncome()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		var (expected, _) = dataAccess.MachineInfo;
		decimal actual = logic.GetCurrentIncome();

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void GetTotalInsertedCoin_ReturnsCorrectAmountForUser()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		decimal expected = 1.20m;
		dataAccess.UserCredit.Add("admin", expected);

		decimal actual = logic.GetTotalInsertedCoin("admin");

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void InsertCoin_InsertsCorrectAmountForUser()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);
		string user1 = "admin";
		string user2 = "test";
		decimal money1 = 14.99m;
		decimal money2 = 0.10m;

		logic.InsertCoin(user1, money1);
		logic.InsertCoin(user2, money2);
		logic.InsertCoin(user1, money1);

		decimal expected = money2;
		decimal actual = dataAccess.UserCredit[user2];

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void RemoveCoinInventory_RemovesAllCoinsFromInventory()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		logic.RemoveCoinInventory();

		Assert.Empty(dataAccess.CoinInventory);
	}

	[Fact]
	public void RemoveItemFromInventory_RemovesItemSuccessfully()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		logic.RemoveItemFromInventory(dataAccess.ItemInventory.First(x => x.Name == "Pepsi"));

		Assert.Equal(5, dataAccess.ItemInventory.Count);
	}

	[Fact]
	public void RemoveItemInventory_RemovesAllItemsFromInventory()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		logic.RemoveItemInventory();

		Assert.Empty(dataAccess.ItemInventory);
	}

	[Fact]
	public void RequestCoinRefund_RefundsCorrectAmountForUser()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		string user = "admin";
		dataAccess.UserCredit[user] = 1.99m;

		logic.RequestCoinRefund(user);

		Assert.True(dataAccess.UserCredit[user] == 0);
	}

	[Fact]
	public void RequestItem_ReturnsCorrectItemAndChange_WhenValid()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		string user = "admin";
		dataAccess.UserCredit[user] = 1.99m;

		ItemModel expectedItem = new ItemModel() { Name = "Pepsi", Price = 1.99m, Slot = "1" };
		var initialState = dataAccess.MachineInfo;
		var results = logic.RequestItem(expectedItem, user);

		Assert.Equal(expectedItem.Name, results.item.Name);
		Assert.Equal(expectedItem.Slot, results.item.Slot);

		Assert.Equal(0, dataAccess.UserCredit[user]);
		Assert.Equal(initialState.CoinCurrent + 1.99m, dataAccess.MachineInfo.CoinCurrent);
		Assert.Equal(initialState.CoinTotal + 1.99m, dataAccess.MachineInfo.CoinTotal);

		Assert.True(String.IsNullOrWhiteSpace(results.errorMessage));
		Assert.True(results.change.Count() == 0);
	}

	[Fact]
	public void RequestItem_ReturnsError_WhenInsufficientFunds()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		string user = "admin";
		dataAccess.UserCredit[user] = 0.99m;

		ItemModel expectedItem = new ItemModel() { Name = "Pepsi", Price = 1.99m, Slot = "1" };
		var initialState = dataAccess.MachineInfo;
		var results = logic.RequestItem(expectedItem, user);

		Assert.Null(results.item);

		Assert.Equal(0.99m, dataAccess.UserCredit[user]);
		Assert.Equal(initialState.CoinCurrent, dataAccess.MachineInfo.CoinCurrent);
		Assert.Equal(initialState.CoinTotal, dataAccess.MachineInfo.CoinTotal);

		Assert.False(String.IsNullOrWhiteSpace(results.errorMessage));
	}

	[Fact]
	public void RequestItem_ReturnsError_WhenItemNotAvailable()
	{
		MockDataAccess dataAccess = new MockDataAccess();
		VendingMachineLogic logic = new VendingMachineLogic(dataAccess);

		string user = "admin";
		dataAccess.UserCredit[user] = 2.99m;

		ItemModel expectedItem = new ItemModel() { Name = "Dr. Pepper", Price = 2.29m, Slot = "4" };
		var initialState = dataAccess.MachineInfo;
		var results = logic.RequestItem(expectedItem, user);

		Assert.Null(results.item);

		Assert.Equal(2.99m, dataAccess.UserCredit[user]);
		Assert.Equal(initialState.CoinCurrent, dataAccess.MachineInfo.CoinCurrent);
		Assert.Equal(initialState.CoinTotal, dataAccess.MachineInfo.CoinTotal);

		Assert.False(String.IsNullOrWhiteSpace(results.errorMessage));
	}
}