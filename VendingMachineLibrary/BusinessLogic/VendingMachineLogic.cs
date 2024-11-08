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
		throw new NotImplementedException();
	}

	public void AddToItemInventory(List<ItemModel> items)
	{
		throw new NotImplementedException();
	}

	public List<CoinModel> CalculateChange(decimal totalInserted, decimal itemPrice)
	{
		throw new NotImplementedException();
	}

	public void DispenseChange(List<CoinModel> change)
	{
		throw new NotImplementedException();
	}

	public List<CoinModel> GetCoinInventory()
	{
		throw new NotImplementedException();
	}

	public List<CoinModel> GetCoinsByName(string name)
	{
		throw new NotImplementedException();
	}

	public decimal GetCurrentIncome()
	{
		throw new NotImplementedException();
	}

	public List<ItemModel> GetItemInventory()
	{
		throw new NotImplementedException();
	}

	public List<ItemModel> GetItemsByName(string name)
	{
		throw new NotImplementedException();
	}

	public decimal GetTotalIncome()
	{
		throw new NotImplementedException();
	}

	public decimal GetTotalInsertedCoin(string userId)
	{
		throw new NotImplementedException();
	}

	public void InsertCoin(string userId, decimal amount)
	{
		throw new NotImplementedException();
	}

	public void RemoveCoinInventory()
	{
		throw new NotImplementedException();
	}

	public void RemoveItemFromInventory(ItemModel item)
	{
		throw new NotImplementedException();
	}

	public void RemoveItemInventory()
	{
		throw new NotImplementedException();
	}

	public void RequestCoinRefund(string userId)
	{
		throw new NotImplementedException();
	}

	public (ItemModel item, List<CoinModel> change, string errorMessage) RequestItem(ItemModel item, string userId)
	{
		throw new NotImplementedException();
	}
}
