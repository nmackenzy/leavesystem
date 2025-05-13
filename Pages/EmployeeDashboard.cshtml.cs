using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using simple_api.Models;
using simple_api.Services;

public class EmployeeDashboardModel : PageModel
{
    private readonly LeaveRequestService _leaveService;
    private readonly UserService _userService;

    public EmployeeDashboardModel(LeaveRequestService leaveService, UserService userService)
    {
        _leaveService = leaveService;
        _userService = userService;
    }

    public User? Employee { get; set; }

    public List<LeaveRequest> MyRequests { get; set; } = new();

    [BindProperty]
    public LeaveRequest NewRequest { get; set; } = new();

    public IActionResult OnGet()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var role = HttpContext.Session.GetString("Role");

        if (userId == null || role != "Employee")
            return RedirectToPage("/Index");

        Employee = _userService.Get(userId.Value);
        MyRequests = _leaveService
            .GetAll()
            .Where(r => r.UserId == userId.Value)
            .ToList();

        return Page();
    }

    public IActionResult OnPost()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToPage("/Index");

        // validation to check that start date is before end date
        if (NewRequest.StartDate > NewRequest.EndDate)
        {
            ModelState.AddModelError(string.Empty, "Start date must be before end date.");
        }

        // validation to check that dates are in the future
        if (NewRequest.StartDate < DateTime.Today)
        {
            ModelState.AddModelError(string.Empty, "Start date must be in the future.");
        }

        if (!ModelState.IsValid)
        {
            Employee = _userService.Get(userId.Value);
            MyRequests = _leaveService
                .GetAll()
                .Where(r => r.UserId == userId.Value)
                .ToList();
            return Page();
        }

        NewRequest.UserId = userId.Value;
        _leaveService.Add(NewRequest);
        return RedirectToPage();
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Clear(); // clears user session
        return RedirectToPage("/Index"); // redirect to login
    }

}
