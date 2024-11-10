using VendingMachineLibrary.DataAccess;
using VendingMachineLibrary.Models;

namespace VendingMachineLibrary.BusinessLogic;

public class VendingMachineLogic : IVendingMachineLogic
{
	private readonly IDataAccess _dataAccess;
	public VendingMachineLogic(IDataAccess dataAccess)
	{
		_dataAccess = dataAccess;
	}

	public void AddToCoinInventory(List<CoinModel> Coin)
	{
		_dataAccess.CoinInventory_AddCoin(Coin);
	}

	public void AddToItemInventory(List<ItemModel> items)
	{
		_dataAccess.ItemInventory_AddItems(items);
	}

	public List<CoinModel> CalculateChange(decimal totalInserted, decimal itemPrice)
	{
		decimal changeAmount = totalInserted - itemPrice;
		if (changeAmount < 0)
		{
			throw new InvalidOperationException("Insufficient funds.");
		}

		List<CoinModel> change = new List<CoinModel>();
		var coinInventory = _dataAccess.CoinInventory_GetAll()
										 .OrderByDescending(c => c.Value)
										 .ToList();

		foreach (var coin in coinInventory)
		{
			while (changeAmount >= coin.Value && coinInventory.Any(c => c.Value == coin.Value))
			{
				change.Add(coin);
				changeAmount -= coin.Value;
				changeAmount = Math.Round(changeAmount, 2); // To handle floating-point precision issues
				coinInventory.Remove(coin);
			}

			if (changeAmount == 0) break;
		}

		if (changeAmount > 0)
		{
			throw new InvalidOperationException("Unable to provide exact change.");
		}

		return change;
	}

	public void DispenseChange(List<CoinModel> change)
	{
		foreach (var coin in change)
		{
			var coinsToRemove = _dataAccess.CoinInventory_GetSpecificDenomination(coin.Value, 1);
			if (coinsToRemove.Count == 0)
			{
				throw new InvalidOperationException($"Insufficient coins of denomination {coin.Value}");
			}

			_dataAccess.Coinnventory_RemoveCoins(coinsToRemove);
		}
	}

	public List<CoinModel> GetCoinInventory()
	{
		return _dataAccess.CoinInventory_GetAll();
	}

	public List<CoinModel> GetCoinsByName(string name)
	{
		decimal value;

		switch (name.ToLower())
		{
			case "penny":
				value = 0.01m;
				break;
			case "nickel":
				value = 0.05m;
				break;
			case "dime":
				value = 0.10m;
				break;
			case "quarter":
				value = 0.25m;
				break;
			case "half dollar":
				value = 0.50m;
				break;
			case "dollar":
				value = 1.00m;
				break;
			default:
				throw new ArgumentException("Invalid coin name.");
		}

		return _dataAccess.CoinInventory_GetSpecificDenomination(value);
	}

	public decimal GetCurrentIncome()
	{
		return _dataAccess.MachineInfo_CurrentCoins();
	}

	public List<ItemModel> GetItemInventory()
	{
		return _dataAccess.ItemInventory_GetAll();
	}

	public List<ItemModel> GetItemsByName(string name)
	{
		List<ItemModel> items = _dataAccess.ItemInventory_GetAll();
		
		List<ItemModel> results = items.FindAll(x => x.Name == name);

		return results;
	}

	public decimal GetTotalIncome()
	{
		return _dataAccess.MachineInfo_TotalCoins();
	}

	public decimal GetTotalInsertedCoin(string userId)
	{
		return _dataAccess.UserCoin_Balance(userId);
	}

	public void InsertCoin(string userId, decimal amount)
	{
		_dataAccess.UserCoin_Deposit(userId, amount);
	}

	public void RemoveCoinInventory()
	{
		_dataAccess.CoinInventory_Clear();
	}

	public void RemoveItemFromInventory(ItemModel item)
	{
		_dataAccess.ItemInventory_RemoveItems(new List<ItemModel> { item });
	}

	public void RemoveItemInventory()
	{
		_dataAccess.ItemInventory_Clear();
	}

	public void RequestCoinRefund(string userId)
	{
		_dataAccess.UserCoin_Reset(userId);
	}

	public (ItemModel item, List<CoinModel> change, string errorMessage) RequestItem(ItemModel item, string userId)
	{
		var userBalance = _dataAccess.UserCoin_Balance(userId);
		if (userBalance < item.Price)
		{
			return (null, null, "Insufficient balance.");
		}

		var inventoryItem = _dataAccess.ItemInventory_GetItem(item);
		if (inventoryItem == null)
		{
			return (null, null, "Item not available.");
		}

		try
		{
			var change = CalculateChange(userBalance, item.Price);

			_dataAccess.ItemInventory_RemoveItems(new List<ItemModel> { inventoryItem });
			_dataAccess.UserCoin_Reset(userId);
			_dataAccess.MachineInfo_AddIncome(item.Price);

			DispenseChange(change);

			return (inventoryItem, change, null);
		}
		catch (InvalidOperationException ex)
		{
			return (null, null, ex.Message);
		}
	}
}
													