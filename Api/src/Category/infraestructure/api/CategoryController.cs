using Api.src.Auth.application.Utils;
using Api.src.Auth.domain.dto;
using Api.src.Category.application.mappers;
using Api.src.Category.domain.dto;
using Api.src.Category.domain.repository;
using backend.src.User.domain.entity;
using backend.src.User.domain.enums;
using backend.src.User.infraestructure.api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Api.src.Category.infraestructure.api
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private CategoryRepository _categoryService;

        public CategoryController(CategoryRepository categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CategoryDto>))]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllApprovedAsync();
            var categoriesList = categories.Select(i => i.ToCategoryDto());
            return Ok(categoriesList);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Employee)}")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto category)
        {
            var result = await _categoryService.CreateAsync(
                category.ToCategoryModelForCreate(),
                (UserRole)Enum.Parse(typeof(UserRole), Token.GetTokenPayload(Request).Role));
            return Created($"/api/category/{result.Id}", result.ToCategoryDto());
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Employee)}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _categoryService.DeleteCategoryByIdAsync(id, (UserRole)Enum.Parse(typeof(UserRole), Token.GetTokenPayload(Request).Role));
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Employee)}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCategoryDto category)
        {
            var result = await _categoryService.UpdateCategoryByIdAsync(id, category.ToCategoryModelForUpdate() , (UserRole)Enum.Parse(typeof(UserRole), Token.GetTokenPayload(Request).Role));
            return Ok(result);
        }
    }
}
