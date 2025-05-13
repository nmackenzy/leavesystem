using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using simple_api.Services;

public class IndexModel : PageModel
{
    private readonly UserService _userService;

    public IndexModel(UserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    public int UserId { get; set; }

    public string? ErrorMessage { get; set; }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        var user = _userService.Get(UserId);
        if (user == null)
        {
            ErrorMessage = "Invalid User ID.";
            return Page();
        }

        HttpContext.Session.SetInt32("UserId", user.UserId);
        HttpContext.Session.SetString("Role", user.Role);

        return RedirectToPage(user.Role == "Manager" ? "/ManagerDashboard" : "/EmployeeDashboard");
    }
}
