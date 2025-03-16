using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VendingMachineRazerUI.Pages;

public class GenerateAdminModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;
    private const string adminRole = "admin";

    public bool RoleCreated { get; set; }
    public Guid AdminUserId{ get; set; }

    public GenerateAdminModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IConfiguration config)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _config = config;
    }
    public async Task OnGet()
    {
        var roleExists = await _roleManager.RoleExistsAsync(adminRole);

        if (!roleExists)
        {
            await _roleManager.CreateAsync(new IdentityRole(adminRole));
            RoleCreated = true;
        }

        string userEmail = _config.GetValue<string>("AdminUser");
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, adminRole);
            if (Guid.TryParse(user.Id, out Guid parsedId))
            {
                AdminUserId = parsedId;
            }
        }
    }
}
