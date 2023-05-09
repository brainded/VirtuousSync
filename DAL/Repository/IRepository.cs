using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IRepository<T> where T : class
    {
        Task SaveChangesAsync();

        Task AddRangeAsync(IList<T> entities);
    }
}
