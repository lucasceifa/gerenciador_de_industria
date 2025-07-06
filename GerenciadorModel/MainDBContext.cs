using Microsoft.EntityFrameworkCore;
namespace GerenciadorModel
{
    public class MainDBContext : DbContext
    {
        public MainDBContext(DbContextOptions<MainDBContext> options) : base(options)
        {
        }
    }
}
