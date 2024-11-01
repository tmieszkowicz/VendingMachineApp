using VendingMachineLibrary.Models;

namespace VendingMachineLibrary.BusinessLogic;

internal class VendingMachineLogic : IVendingMachineLogic
{
	public void AddToItemInventory(List<ItemModel> items)
	{
		throw new NotImplementedException();
	}

	public void AddToMoneyInventory(List<MoneyModel> money)
	{
		throw new NotImplementedException();
	}

	public void GetCurrentIncome()
	{
		throw new NotImplementedException();
	}

	public List<ItemModel> GetItemInventory()
	{
		throw new NotImplementedException();
	}

	public decimal GetItemPrice(ItemModel item)
	{
		throw new NotImplementedException();
	}

	public List<MoneyModel> GetMoneyInventory()
	{
		throw new NotImplementedException();
	}

	public void GetTotalIncome()
	{
		throw new NotImplementedException();
	}

	public decimal GetTotalInsertedMoney()
	{
		throw new NotImplementedException();
	}

	public void InsertMoney(decimal amount)
	{
		throw new NotImplementedException();
	}

	public List<ItemModel> ListItems()
	{
		throw new NotImplementedException();
	}

	public List<ItemModel> RemoveFromItemInventory()
	{
		throw new NotImplementedException();
	}

	public decimal RemoveFromMoneyInventory()
	{
		throw new NotImplementedException();
	}

	public (ItemModel item, List<MoneyModel> change, string errorMessage) RequestItem(ItemModel item)
	{
		throw new NotImplementedException();
	}

	public void RequestMoneyRefund()
	{
		throw new NotImplementedException();
	}
}
