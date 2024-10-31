using Services.Abstractions.DTOs;
using Services.Abstractions.Interfaces;
using Services.Services;
using System.Collections.Generic;
using Xunit;
namespace UnitTests.Services
{
    public class CacheServiceTests
    {
        private readonly ICacheService _cacheService;

        public CacheServiceTests()
        {
            _cacheService = new CacheService();
        }

        [Fact]
        public void Get_KeyDoesNotExist_ReturnsNull()
        {
            // Arrange
            string key = "non-existing-key";

            // Act
            var result = _cacheService.Get(key);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Set_ValidKeyAndValue_CanRetrieveValue()
        {
            // Arrange
            string key = "dogs";
            var dogs = new List<DogDTO>
        {
            new DogDTO { Name = "Doggy", Color = "Brown", TailLength = 15, Weight = 10 }
        };

            // Act
            _cacheService.Set(key, dogs);
            var result = _cacheService.Get(key);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dogs.Count, result.Count);
            Assert.Equal(dogs[0].Name, result[0].Name);
        }

        [Fact]
        public void Reset_AfterAddingEntries_ClearsCache()
        {
            // Arrange
            string key = "dogs";
            var dogs = new List<DogDTO>
            {
                new DogDTO { Name = "Doggy", Color = "Brown", TailLength = 15, Weight = 10 }
            };

            _cacheService.Set(key, dogs);

            // Act
            _cacheService.Reset();
            var result = _cacheService.Get(key);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Set_SameKeyTwice_OverridesPreviousValue()
        {
            // Arrange
            string key = "dogs";
            var initialDogs = new List<DogDTO>
        {
            new DogDTO { Name = "Doggy", Color = "Brown", TailLength = 15, Weight = 10 }
        };

            var newDogs = new List<DogDTO>
        {
            new DogDTO { Name = "Rover", Color = "Black", TailLength = 20, Weight = 12 }
        };

            // Act
            _cacheService.Set(key, initialDogs);
            _cacheService.Set(key, newDogs);
            var result = _cacheService.Get(key);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Rover", result[0].Name);
        }

        [Fact]
        public void Set_NullKey_ThrowsArgumentNullException()
        {
            // Arrange
            string key = null;
            var dogs = new List<DogDTO>
        {
            new DogDTO { Name = "Doggy", Color = "Brown", TailLength = 15, Weight = 10 }
        };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _cacheService.Set(key, dogs));
        }

        [Fact]
        public void Set_EmptyList_CanRetrieveEmptyList()
        {
            // Arrange
            string key = "empty-dogs";
            var emptyList = new List<DogDTO>();

            // Act
            _cacheService.Set(key, emptyList);
            var result = _cacheService.Get(key);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }

}