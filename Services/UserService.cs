using simple_api.Data;
using simple_api.Models;

namespace simple_api.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context = default!;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<User> GetAll()
        {
            if (_context.Users != null)
            {
                return _context.Users.ToList();
            }
            return new List<User>();
        }

        public User? Get(int id)
        {
            if (_context.Users != null)
            {
                return _context.Users.Find(id);
            }
            return null;
        }

        public void Add(User user)
        {
            if (_context.Users != null)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            if (_context.Users != null)
            {
                var user = _context.Users.Find(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                }
            }
        }

        public void Update(User user)
        {
            if (_context.Users != null)
            {
                var existingUser = _context.Users.Find(user.UserId);
                if (existingUser != null)
                {
                    _context.Entry(existingUser).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                }
            }
        }
    }
}
