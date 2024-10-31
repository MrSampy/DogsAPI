using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Services.Abstractions.DTOs;
using Services.Abstractions.Interfaces;
using System.Xml.Linq;

namespace Services.Services
{
    public class DogService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, IDogValidator dogValidator) : IDogService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IDogValidator _dogValidator = dogValidator;
        public async Task<List<DogDTO>> GetAllAsync(string attribute, string order, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            string cacheKey = $"dogs:{attribute}:{order}:{pageNumber}:{pageSize}";

            var cachedDogs = _cacheService.Get(cacheKey);
            if (cachedDogs != null) return cachedDogs;

            var dogs = await _unitOfWork.DogRepository.GetAllAsync(cancellationToken);
            var result = _mapper.Map<List<DogDTO>>(GetResultedDogs(dogs, attribute, order, pageNumber, pageSize));

            _cacheService.Set(cacheKey, result);

            return result;
        }

        public List<Dog> GetResultedDogs(IEnumerable<Dog> dogs, string attribute, string order, int pageNumber, int pageSize) 
        {
            var sortedDogs = SortDogs(dogs, attribute, order);
            var paginatedDogs = PaginateDogs(sortedDogs, pageNumber, pageSize);
            return paginatedDogs.ToList();
        }

        public IEnumerable<Dog> PaginateDogs(IEnumerable<Dog> dogs, int pageNumber, int pageSize) 
        {
            if (pageNumber < 1 || pageSize < 1)
                throw new CustomException("Page number and page size must be positive integers.");

            return dogs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        public IEnumerable<Dog> SortDogs(IEnumerable<Dog> dogs, string attribute, string order)
        {
            bool isDescending = order?.ToLower() == "desc";
            return attribute.ToLower() switch
            {
                "name" => isDescending ? dogs.OrderByDescending(d => d.Name) : dogs.OrderBy(d => d.Name),
                "color" => isDescending ? dogs.OrderByDescending(d => d.Color) : dogs.OrderBy(d => d.Color),
                "tail_length" => isDescending ? dogs.OrderByDescending(d => d.TailLength) : dogs.OrderBy(d => d.TailLength),
                "weight" => isDescending ? dogs.OrderByDescending(d => d.Weight) : dogs.OrderBy(d => d.Weight),
                _ => dogs.OrderBy(d => d.Name)
            };
        }

        public async Task Insert(DogDTO dog, CancellationToken cancellationToken = default)
        {
            if (dog == null || !_dogValidator.IsValid(dog))
                throw new CustomException("Dog data is not valid!");

            var existingDog = await _unitOfWork.DogRepository
                .FindByConditionAsync(d => d.Name == dog.Name, cancellationToken);

            if (existingDog != null)
            {
                throw new CustomException($"A dog with the name {dog.Name} already exists.");
            }

            _unitOfWork.DogRepository.Insert(_mapper.Map<Dog>(dog));

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            _cacheService.Reset();
        }
    }
}
