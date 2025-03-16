using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VendingMachineLibrary.BusinessLogic;
using VendingMachineLibrary.Models;

namespace VendingMachineRazerUI.Pages;

[Authorize]
public class VendingMachineModel : PageModel
{
    private readonly IVendingMachineLogic _vendingMachine;
    public decimal DepositedAmount { get; set; }
    public List<ItemModel> Items { get; set; }

    [BindProperty]
    public decimal Deposit { get; set; }
    [BindProperty]
    public ItemModel SelectedItem { get; set; }

    public Guid UserId { get; set; }
    [BindProperty(SupportsGet = true)]
    public string OutputText { get; set; }
    [BindProperty(SupportsGet = true)]
    public string ErrorMessage { get; set; }

    public VendingMachineModel(IVendingMachineLogic vendingMachine)
    {
        _vendingMachine = vendingMachine;
    }
    public void OnGet()
    {
        SetUserIdFromClaim();

        DepositedAmount = _vendingMachine.GetTotalInsertedCoin(UserId);
        Items = _vendingMachine.GetItemInventory().GroupBy(x => x.Name).Select(x => x.First()).ToList();
    }

    public IActionResult OnPost()
    {
        SetUserIdFromClaim();

        if (Deposit > 0)
        {
            _vendingMachine.InsertCoin(UserId, Deposit);
        }

        return RedirectToPage();
    }
    public IActionResult OnPostItem()
    {
        SetUserIdFromClaim();

        var results = _vendingMachine.RequestItem(SelectedItem, UserId);
        OutputText = string.Empty;

        if (results.errorMessage != null)
        {
            ErrorMessage = results.errorMessage;
        }
        else
        {
            OutputText = $"Item {results.item.Name} dispensed. Enjoy!";

            if (results.change.Count > 0)
            {
                OutputText += "<br>Dispensing change.<br>";
                results.change.ForEach(item => OutputText += $"{item.Name}<br>");
            }
            else
            {
                OutputText += "<br>There is no change";
            }
        }

        return RedirectToPage(new { ErrorMessage, OutputText });
    }
    public IActionResult OnPostCancel()
    {
        DepositedAmount = _vendingMachine.GetTotalInsertedCoin(UserId);
        _vendingMachine.RequestCoinRefund(UserId);

        OutputText = $"You refunded {DepositedAmount,0:C}";

        return RedirectToPage(new { OutputText });
    }
    void SetUserIdFromClaim()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(userIdString, out var userId))
        {
            UserId = userId;
        }
    }
}
