using Domain.Interfaces;
using Persistence.DbContexts;
using Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DbContexts
{
    public sealed class UnitOfWork(RepositoryDbContext dbContext) : IUnitOfWork
    {
        private readonly RepositoryDbContext dbContext = dbContext;

        private DogRepository _dogRepository;
        public IDogRepository DogRepository => _dogRepository ??= new DogRepository(dbContext);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
