using Api.src.Review.domain.entity;
using Api.src.Review.domain.repository;
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
        public async Task<IActionResult> CreateReview()
        {
            var created = await _reviewService.CreateAsync(new ReviewEntity { });
            return Ok();
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
