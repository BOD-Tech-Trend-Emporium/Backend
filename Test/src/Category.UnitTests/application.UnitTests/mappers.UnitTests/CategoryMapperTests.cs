
using Api.src.Category.application.mappers;
using Api.src.Category.domain.dto;
using Api.src.Category.domain.entity;
using Api.src.Category.domain.enums;
using FluentAssertions;

namespace Test.Category.UnitTests.application.UnitTests.mappers.UnitTests
{
    public class CategoryMapperTests
    {
        [Fact]
        public void Given_CategoryEntity_When_AllFieldsMatch_Then_CategoryDto()
        {
            //Arrange
            CategoryEntity category = new() { Id=Guid.NewGuid(), Name="Music", Status=CategoryStatus.Created };

            //ACT
            var result = CategoryMapper.ToCategoryDto(category);
            var expected = new CategoryDto() { Id = category.Id, Name = category.Name };

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryDto>();
            result.Should().BeEquivalentTo(expected);

        }
        [Fact]
        public void Given_CategoryEntity_When_NotAllFieldsMatch_Then_CategoryDto()
        {
            //Arrange
            CategoryEntity category = new() { Id = Guid.NewGuid()};

            //ACT
            var result = CategoryMapper.ToCategoryDto(category);
            var expected = new CategoryDto() { Id = category.Id};

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryDto>();
            result.Should().BeEquivalentTo(expected);

        }

        [Fact]
        public void Given_CreateCategoryDto_When_AllFieldsMatch_Then_CategoryEntity()
        {
            //Arrange
            var createCategoryDto = new CreateCategoryDto() { Name = "Music" };
            //ACT
            var result = CategoryMapper.ToCategoryModelForCreate(createCategoryDto);
            CategoryEntity expected = new() {Name = createCategoryDto.Name};

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryEntity>();
            result.Should().BeEquivalentTo(expected);

        }

        [Fact]
        public void Given_CreateCategoryDto_When_NoAllFieldsMatch_Then_CategoryEntity()
        {
            //Arrange
            var createCategoryDto = new CreateCategoryDto();
            //ACT
            var result = CategoryMapper.ToCategoryModelForCreate(createCategoryDto);
            CategoryEntity expected = new();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryEntity>();
            result.Should().BeEquivalentTo(expected);

        }

        [Fact]
        public async void Given_CreateCategoryDto_When_NameFieldDoesNotExist_Then_CategoryEntityWithNotNameField()
        {
            //Arrange
            var createCategoryDto = new CreateCategoryDto();
            //ACT
            var result = CategoryMapper.ToCategoryModelForCreate(createCategoryDto);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryEntity>();
            result.Name.Should().BeNull();

        }
    }
}
