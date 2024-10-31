namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IDogRepository DogRepository { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
