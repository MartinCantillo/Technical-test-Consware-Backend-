
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{

    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        public DbSet<User> Users { set; get; }

        public DbSet<TravelRequest> TravelRequests { set; get; }

    }
}