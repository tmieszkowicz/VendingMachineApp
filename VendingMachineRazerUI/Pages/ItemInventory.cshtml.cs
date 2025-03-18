using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VendingMachineLibrary.BusinessLogic;
using VendingMachineLibrary.Models;

namespace VendingMachineRazerUI.Pages;

[Authorize(Roles = "admin")]
public class ItemInventoryModel : PageModel
{
    private readonly IVendingMachineLogic _vendingMachine;
    public List<ItemModel> Items { get; set; }
    public List<SelectListItem> ItemOptions { get; set; }
    public List<SelectListItem> SlotOptions { get; set; }

    [BindProperty]
    public string SelectedItem { get; set; }
    [BindProperty]
    public string SelectedSlot { get; set; }
    [BindProperty]
    public decimal SelectedPrice { get; set; }

    public ItemInventoryModel(IVendingMachineLogic vendingMachine)
    {
        _vendingMachine = vendingMachine;
    }

    public void OnGet() 
    {
        Items =  _vendingMachine.GetItemInventory().OrderBy(x => x.Slot).ToList();
        ItemOptions = Items.GroupBy(x => x.Name).Select(x => new SelectListItem
        {
            Value = x.Key,
            Text = x.Key,
        }).ToList();
        
        SlotOptions = Items.GroupBy(x => x.Slot).Select(x => new SelectListItem
        {
            Value = x.Key,
            Text = x.Key,
        }).ToList();
    }
    public IActionResult OnPost()
    {
        _vendingMachine.AddToItemInventory(new List<ItemModel> 
        { 
            new ItemModel
            {
                Slot = SelectedSlot,
                Name = SelectedItem,
                Price = SelectedPrice,
            }
        });

        return RedirectToPage();
    }
}
