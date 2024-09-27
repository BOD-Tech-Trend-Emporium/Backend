using Api.src.Category.domain.dto;
using Api.src.Category.domain.entity;

namespace Api.src.Category.application.mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToCategoryDto(this CategoryEntity category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public static CategoryEntity ToCategoryModelForCreate(this CreateCategoryDto category)
        {
            return new CategoryEntity
            {
                Name = category.Name,
            };
        }

        public static CategoryEntity ToCategoryModelForUpdate(this UpdateCategoryDto category)
        {
            return new CategoryEntity
            {
                Name = category.Name,
            };
        }

    }
}
