using Domain.Entities;
using Domain.Interfaces;
using Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Persistence.Repository
{
    public sealed class DogRepository(RepositoryDbContext dbContexts) : IDogRepository
    {
        private readonly RepositoryDbContext _dbContexts = dbContexts;

        public async Task<List<Dog>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContexts.Dogs.AsNoTracking().ToListAsync(cancellationToken);
        }
        public async Task<Dog?> FindByConditionAsync(Expression<Func<Dog, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _dbContexts.Dogs.AsNoTracking().FirstOrDefaultAsync(expression, cancellationToken);
        }
        public void Insert(Dog dog)
        {
            _dbContexts.Dogs.Add(dog);
        }
    }
}
