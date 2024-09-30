using Api.src.Auth.application.Utils;
using Api.src.Review.application.mappers;
using Api.src.Review.domain.dto;
using Api.src.Review.domain.entity;
using Api.src.Review.domain.repository;
using backend.src.User.domain.dto;
using backend.src.User.domain.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.src.Review.infraestructure.api
{
    [Route("api/Review")]
    [ApiController]
    public class reviewController: ControllerBase
    {
        private ReviewRepository _reviewService;

        public reviewController(ReviewRepository reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        [Route("product/{id}")]
        public async Task<IActionResult> GetByProductId([FromRoute] Guid id) 
        {
            var reviews = await _reviewService.GetAllByProductAsync(id);
            return Ok();
        }

        [HttpGet]
        [Route("user/{id}")]
        public async Task<IActionResult> GetByUsertId([FromRoute] Guid id)
        {
            var reviews = await _reviewService.GetAllByUserAsync(id);
            return Ok() ;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(201, Type = typeof(ReviewDto))]
        [Authorize(Roles = nameof(UserRole.Shopper))]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto review)
        {
            var userId = Guid.Parse(Token.GetTokenPayload(Request).UserId);
            // var userId = review.userId;
            var created = await _reviewService.CreateAsync(review, userId);
            return Created($"api/Review", created.ToReviewDto());
        }

        [HttpPut]
        [Route("/{id}")]
        public async Task<IActionResult> updateReview([FromRoute] Guid id)
        {
            var created = await _reviewService.UpdateAsync(new ReviewEntity { });
            return Ok();
        }

        [HttpDelete]
        [Route("/{id}")]
        public async Task<IActionResult> DeleteReview([FromRoute] Guid id)
        {
            var deleted = await _reviewService.DeleteAsync(id);
            return StatusCode(204);
        }

    }
}
