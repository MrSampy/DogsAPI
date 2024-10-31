using Services.Abstractions.DTOs;
using Services.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    public class DogValidatorTests
    {
        private readonly DogValidator _validator;

        public DogValidatorTests()
        {
            _validator = new DogValidator();
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenDogIsValid()
        {
            // Arrange
            var dog = new DogDTO
            {
                Name = "Buddy",
                Color = "red&white",
                TailLength = 10,
                Weight = 20
            };

            // Act
            var result = _validator.IsValid(dog);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenNameIsNullOrEmpty()
        {
            // Arrange
            var dog = new DogDTO
            {
                Name = "",
                Color = "red&white",
                TailLength = 10,
                Weight = 20
            };

            // Act
            var result = _validator.IsValid(dog);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenTailLengthIsNotPositive()
        {
            // Arrange
            var dog = new DogDTO
            {
                Name = "Buddy",
                Color = "red&white",
                TailLength = -1,
                Weight = 20
            };

            // Act
            var result = _validator.IsValid(dog);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenWeightIsNotPositive()
        {
            // Arrange
            var dog = new DogDTO
            {
                Name = "Buddy",
                Color = "red&white",
                TailLength = 10,
                Weight = -5
            };

            // Act
            var result = _validator.IsValid(dog);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("red")]
        [InlineData("red&blue")]
        [InlineData("red&blue&green")]
        public void IsValidColor_ShouldReturnTrue_ForValidColors(string color)
        {
            // Act
            var result = _validator.IsValidColor(color);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("red&")]
        [InlineData("&blue")]
        [InlineData("red&&blue")]
        public void IsValidColor_ShouldReturnFalse_ForInvalidColors(string color)
        {
            // Act
            var result = _validator.IsValidColor(color);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenColorIsInvalid()
        {
            // Arrange
            var dog = new DogDTO
            {
                Name = "Buddy",
                Color = "red&",
                TailLength = 10,
                Weight = 20
            };

            // Act
            var result = _validator.IsValid(dog);

            // Assert
            Assert.False(result);
        }
    }

}
