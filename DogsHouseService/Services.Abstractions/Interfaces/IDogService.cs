using Services.Abstractions.DTOs;

namespace Services.Abstractions.Interfaces
{
    public interface IDogService
    {
        Task<List<DogDTO>> GetAllAsync(string attribute, string order, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        Task Insert(DogDTO dog, CancellationToken cancellationToken = default);
    }
}
