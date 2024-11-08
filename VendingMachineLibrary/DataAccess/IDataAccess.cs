﻿using VendingMachineLibrary.Models;

namespace VendingMachineLibrary.DataAccess;

internal interface IDataAccess
{
	ItemModel ItemInventory_GetItem(ItemModel item);
	List<ItemModel> ItemInventory_GetDistinctTypes();
	List<ItemModel> ItemInventory_GetAll();
	void ItemInventory_AddItems(List<ItemModel> items);
	void ItemInventory_RemoveItems(List<ItemModel> items);
	void ItemInventory_Clear();

	List<CoinModel> CoinInventory_GetSpecificDenomination(decimal CoinValue, int quantity);
	List<CoinModel> CoinInventory_GetAll();
	void CoinInventory_AddCoin(List<CoinModel> Coin);
	void CoinInventory_Clear();

	decimal UserCoin_Balance(string userId);
	void UserCoin_Reset(string userId);
	decimal UserCoin_Deposit(string userId, decimal CoinValue);

	decimal MachineInfo_EmptyCoins();
	decimal MachineInfo_CurrentCoins();
	decimal MachineInfo_TotalCoins();
}
