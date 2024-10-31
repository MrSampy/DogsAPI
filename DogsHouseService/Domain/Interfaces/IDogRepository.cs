using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IDogRepository
    {
        Task<List<Dog>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Dog?> FindByConditionAsync(Expression<Func<Dog, bool>> expression, CancellationToken cancellationToken = default);
        void Insert(Dog dog);
    }
}
