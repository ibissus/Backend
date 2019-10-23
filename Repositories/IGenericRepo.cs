using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.Repositories
{
    public interface IGenericRepo<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        IQueryable<TEntity> Get();
        Task SaveAsync();
    }
}