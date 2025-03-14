using VendingMachineLibrary.Models;

namespace VendingMachineLibrary.BusinessLogic;

public interface IVendingMachineLogic
{
	void AddToItemInventory(List<ItemModel> items);
	List<ItemModel> GetItemInventory();
	List<ItemModel> GetItemsByName(string name);
	void RemoveItemFromInventory(ItemModel item);
	void RemoveItemInventory();

	void AddToCoinInventory(List<CoinModel> Coin);
	List<CoinModel> GetCoinInventory();
	List<CoinModel> GetCoinsByName(string name);
	void RemoveCoinInventory();

	void DispenseChange(List<CoinModel> change);
	List<CoinModel> CalculateChange(decimal totalInserted, decimal itemPrice);

	(ItemModel item, List<CoinModel> change, string errorMessage) RequestItem(ItemModel item, Guid userId);

	void InsertCoin(Guid userId, decimal amount);
	void RequestCoinRefund(Guid userId);
	decimal GetTotalInsertedCoin(Guid userId);

	decimal GetCurrentIncome();
	decimal GetTotalIncome();
}
