using Services.Abstractions.DTOs;
using Services.Abstractions.Interfaces;
using System.Collections.Concurrent;

namespace Services.Services
{
    public sealed class CacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, List<DogDTO>> _cachedDogs;

        public CacheService()
        {
            _cachedDogs = new ConcurrentDictionary<string, List<DogDTO>>();
        }

        public List<DogDTO>? Get(string key) =>
            _cachedDogs.TryGetValue(key, out var result) ? result : null;

        public void Reset() => _cachedDogs.Clear();

        public void Set(string key, List<DogDTO> dogs) =>
            _cachedDogs[key] = dogs;
    }
}
