using VendingMachineLibrary.Models;

namespace VendingMachineLibrary.DataAccess;

internal interface IDataAccess
{
	ItemModel ItemInventory_GetItem(ItemModel item);
	List<ItemModel> ItemInventory_GetTypes();
	List<ItemModel> ItemInventory_GetAll();
	void ItemInventory_AddItem(ItemModel item);
	void ItemInventory_AddItems(List<ItemModel> items);

	void UserMoney_Insert(string userId, decimal amount);
	decimal UserMoney_Total(string userId);
	void UserMoney_Clear(string userId);
	void UserMoney_Deposit(string userId);

	decimal MachineInfo_ItemPrice();
	decimal MachineInfo_EmptyMoney();
	decimal MachineInfo_CurrentIncome();
	decimal MachineInfo_TotalIncome();

	List<MoneyModel> MoneyInventory_WithdrawMoney(decimal moneyValue, int quantity);
	List<MoneyModel> MoneyInventory_GetAll();
	void MoneyInventory_AddMoney(List<MoneyModel> money);
}
