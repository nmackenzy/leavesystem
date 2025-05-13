using simple_api.Data;
using simple_api.Models;

namespace simple_api.Services
{
    public class LeaveRequestService
    {
        private readonly ApplicationDbContext _context = default!;

        public LeaveRequestService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<LeaveRequest> GetAll()
        {
            if (_context.LeaveRequests != null)
            {
                return _context.LeaveRequests.ToList();
            }
            return new List<LeaveRequest>();
        }

        public LeaveRequest? Get(int id)
        {
            if (_context.LeaveRequests != null)
            {
                return _context.LeaveRequests.Find(id);
            }
            return null;
        }

        public void Add(LeaveRequest leaveRequest)
        {
            if (_context.LeaveRequests != null)
            {
                leaveRequest.Status = "Pending"; // set default status
                _context.LeaveRequests.Add(leaveRequest);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            if (_context.LeaveRequests != null)
            {
                var leaveRequest = _context.LeaveRequests.Find(id);
                if (leaveRequest != null)
                {
                    _context.LeaveRequests.Remove(leaveRequest);
                    _context.SaveChanges();
                }
            }
        }

        public void Update(LeaveRequest leaveRequest)
        {
            if (_context.LeaveRequests != null)
            {
                var existingRequest = _context.LeaveRequests.Find(leaveRequest.RequestId);
                if (existingRequest != null)
                {
                    _context.Entry(existingRequest).CurrentValues.SetValues(leaveRequest);
                    _context.SaveChanges();
                }
            }
        }
    }
}
