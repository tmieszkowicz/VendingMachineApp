using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VendingMachineLibrary.BusinessLogic;
using VendingMachineLibrary.Models;

namespace VendingMachineRazerUI.Pages;

[Authorize(Roles = "admin")]
public class CoinInventoryModel : PageModel
{
    private readonly IVendingMachineLogic _vendingMachine;
    public List<CoinModel> Coins { get; set; }

    [BindProperty]
    public CoinModel CoinToAdd { get; set; } = new CoinModel();

    public CoinInventoryModel(IVendingMachineLogic vendingMachine)
    {
        _vendingMachine = vendingMachine;
    }

    public void OnGet()
    {
        Coins = _vendingMachine.GetCoinInventory().OrderBy(x => x.Value).ToList();
    }
    public IActionResult OnPost() 
    {
        _vendingMachine.AddToCoinInventory(new List<CoinModel> { CoinToAdd });
        
        return RedirectToPage();
    }
}
