using Web.Data;
using Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Web.Services
{
    public class DonationRequestService
    {
        private readonly DataContext _context;

        public DonationRequestService(DataContext context)
        {
            _context = context;
        }

        public void AddRequest(DonationRequest request)
        {
            _context.DonationRequests.Add(request);
            _context.SaveChanges();
        }
        public List<DonationRequest> GetAll()
        {
            return _context.DonationRequests.OrderBy(m => m.Id).ToList();
        }

        public async Task<DonationRequest?> GetCustomFoodById(int id)
        {
            return await _context.DonationRequests.FindAsync(id);
        }

        public List<DonationRequest> GetRequestsByUser(string id)
        {
            return _context.DonationRequests
                .Include(d => d.Donation)
                .Where(x => x.Recipient.Id.Equals(id))
                .OrderByDescending(m => m.Date)
                .ToList();
        }
    }
}
