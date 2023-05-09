using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private DonorContext mContext;
        private DbSet<T> dbSet;

        public Repository(DonorContext context)
        {
            this.mContext = context;
            this.dbSet = context.Set<T>();
        }

        public virtual async Task AddRangeAsync(IList<T> entities)
        {
            Task t = Task.Run(()=>dbSet.AddRange(entities));
            await t;
        }

        public virtual async Task SaveChangesAsync()
        {
            await mContext.SaveChangesAsync();
        }
    }
}
