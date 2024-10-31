using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Moq;
using Services.Abstractions.DTOs;
using Services.Abstractions.Interfaces;
using Services.Services;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;


namespace UnitTests.Services
{
    public class DogServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<IDogValidator> _dogValidatorMock;
        private readonly DogService _dogService;

        public DogServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _cacheServiceMock = new Mock<ICacheService>();
            _dogValidatorMock = new Mock<IDogValidator>();
            _dogService = new DogService(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object, _dogValidatorMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnCachedData_WhenCacheExists()
        {
            // Arrange
            var cachedDogs = new List<DogDTO> { new DogDTO { Name = "Buddy" } };
            _cacheServiceMock.Setup(c => c.Get(It.IsAny<string>())).Returns(cachedDogs);

            // Act
            var result = await _dogService.GetAllAsync("name", "asc", 1, 10);

            // Assert
            Assert.Equal(cachedDogs, result);
            _unitOfWorkMock.Verify(u => u.DogRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetAllAsync_ShouldRetrieveFromRepository_WhenCacheIsEmpty()
        {
            // Arrange
            var dogs = new List<Dog> { new Dog { Name = "Buddy" } };
            var dogDTOs = new List<DogDTO> { new DogDTO { Name = "Buddy" } };
            _cacheServiceMock.Setup(c => c.Get(It.IsAny<string>())).Returns((List<DogDTO>)null);
            _unitOfWorkMock.Setup(u => u.DogRepository.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dogs);
            _mapperMock.Setup(m => m.Map<List<DogDTO>>(It.IsAny<List<Dog>>())).Returns(dogDTOs);

            // Act
            var result = await _dogService.GetAllAsync("name", "asc", 1, 10);

            // Assert
            Assert.Equal(dogDTOs, result);
            _cacheServiceMock.Verify(c => c.Set(It.IsAny<string>(), dogDTOs), Times.Once);
        }

        [Fact]
        public async Task Insert_ShouldThrowException_WhenDogIsInvalid()
        {
            // Arrange
            var dogDTO = new DogDTO { Name = "" };
            _dogValidatorMock.Setup(v => v.IsValid(dogDTO)).Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _dogService.Insert(dogDTO));
            _unitOfWorkMock.Verify(u => u.DogRepository.Insert(It.IsAny<Dog>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Insert_ShouldInsertAndSave_WhenDogIsValid()
        {
            // Arrange
            var dogDTO = new DogDTO { Name = "Buddy" };
            var dogEntity = new Dog { Name = "Buddy" };
            _dogValidatorMock.Setup(v => v.IsValid(dogDTO)).Returns(true);
            _unitOfWorkMock.Setup(u => u.DogRepository.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Dog>());
            _mapperMock.Setup(m => m.Map<Dog>(dogDTO)).Returns(dogEntity);

            // Act
            await _dogService.Insert(dogDTO);

            // Assert
            _unitOfWorkMock.Verify(u => u.DogRepository.Insert(dogEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(c => c.Reset(), Times.Once);
        }

        [Fact]
        public void PaginateDogs_ShouldThrowException_WhenPageNumberOrPageSizeIsInvalid()
        {
            // Arrange
            var dogs = new List<Dog> { new Dog { Name = "Buddy" } };

            // Act & Assert
            Assert.Throws<CustomException>(() => _dogService.PaginateDogs(dogs, 0, 10));
            Assert.Throws<CustomException>(() => _dogService.PaginateDogs(dogs, 1, 0));
        }
        [Fact]
        public async Task Insert_ShouldThrowDuplicateDogNameException_WhenDogNameIsNotUnique()
        {
            // Arrange
            var dogDto = new DogDTO { Name = "Buddy", Color = "Brown", TailLength = 10, Weight = 25 };
            var existingDog = new Dog { Name = "Buddy", Color = "Brown", TailLength = 10, Weight = 25 };

            _unitOfWorkMock.Setup(u => u.DogRepository.FindByConditionAsync(It.IsAny<Expression<Func<Dog, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingDog);

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _dogService.Insert(dogDto));
        }
        [Fact]
        public void SortDogs_ShouldSortByAttributeAscending()
        {
            // Arrange
            var dogs = new List<Dog>
            {
                new Dog { Name = "Buddy", Color = "black", TailLength = 5, Weight = 20 },
                new Dog { Name = "Ace", Color = "white", TailLength = 10, Weight = 15 }
            };

            // Act
            var sortedByName = _dogService.SortDogs(dogs, "name", "asc").ToList();
            var sortedByWeight = _dogService.SortDogs(dogs, "weight", "asc").ToList();

            // Assert
            Assert.Equal("Ace", sortedByName.First().Name);
            Assert.Equal(15, sortedByWeight.First().Weight);
        }

        [Fact]
        public void SortDogs_ShouldSortByAttributeDescending()
        {
            // Arrange
            var dogs = new List<Dog>
            {
                new Dog { Name = "Buddy", Color = "black", TailLength = 5, Weight = 20 },
                new Dog { Name = "Ace", Color = "white", TailLength = 10, Weight = 15 }
            };

            // Act
            var sortedByName = _dogService.SortDogs(dogs, "name", "desc").ToList();
            var sortedByWeight = _dogService.SortDogs(dogs, "weight", "desc").ToList();

            // Assert
            Assert.Equal("Buddy", sortedByName.First().Name);
            Assert.Equal(20, sortedByWeight.First().Weight);
        }
    }

}
