using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using simple_api.Models;
using simple_api.Services;

public class ManagerDashboardModel : PageModel
{
    private readonly LeaveRequestService _leaveService;
    private readonly UserService _userService;

    public ManagerDashboardModel(LeaveRequestService leaveService, UserService userService)
    {
        _leaveService = leaveService;
        _userService = userService;
    }

    public User? Manager { get; set; }
    public List<LeaveRequest> PendingRequests { get; set; } = new();

    public IActionResult OnGet()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var role = HttpContext.Session.GetString("Role");

        if (userId == null || role != "Manager")
            return RedirectToPage("/Index");

        Manager = _userService.Get(userId.Value);
        PendingRequests = _leaveService
            .GetAll()
            .Where(r => r.Status == "Pending")
            .ToList();

        // for each pending request, manually assign the user info (who made the request)
        foreach (var req in PendingRequests)
        {
            req.User = _userService.Get(req.UserId);
        }

        return Page();
    }

    public IActionResult OnPost(int RequestId, string action, string? ManagerComments)
    {
        var request = _leaveService.Get(RequestId);
        
        if (request == null || request.Status != "Pending")
            return RedirectToPage();

        request.Status = action == "approve" ? "Approved" : "Rejected";
        request.ManagerComments = ManagerComments;
        _leaveService.Update(request);

        return RedirectToPage();
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Clear();
        return RedirectToPage("/Index");
    }
}
