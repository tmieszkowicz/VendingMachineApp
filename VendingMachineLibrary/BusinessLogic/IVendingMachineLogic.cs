using VendingMachineLibrary.Models;

namespace VendingMachineLibrary.BusinessLogic;

internal interface IVendingMachineLogic
{
	List<ItemModel> ListItems();
	void InsertMoney(decimal amount);
	decimal GetItemPrice(ItemModel item);
	(ItemModel item, List<MoneyModel> change, string errorMessage) RequestItem(ItemModel item);
	void RequestMoneyRefund();
	decimal GetTotalInsertedMoney();

	void AddToItemInventory(List<ItemModel> items);
	List<ItemModel> RemoveFromItemInventory();

	void AddToMoneyInventory(List<MoneyModel> money);
	decimal RemoveFromMoneyInventory();

	List<ItemModel> GetItemInventory();
	List<MoneyModel> GetMoneyInventory();

	void GetCurrentIncome();
	void GetTotalIncome();
}