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

	void AddItemsToInventory(List<ItemModel> items);
	List<ItemModel> RemoveItemsFromInventory();

	void AddMoneyToInventory(List<MoneyModel> money);
	decimal RemoveMoneyFromInventory();

	List<ItemModel> GetInventory();
	List<MoneyModel> GetMoneyInventory();

	void GetCurrentIncome();
	void GetTotalIncome();
}