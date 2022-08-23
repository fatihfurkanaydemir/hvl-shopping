using Microsoft.AspNetCore.Mvc;
using ReviewService.Application.DTO_s;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Persistence;

namespace ReviewService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewServices _reviewService;

        public ReviewController(ReviewServices reviewService) =>
            _reviewService = reviewService;

        [HttpGet]
        public async Task<List<Review>> Get() =>
            await _reviewService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Review>> Get(string id)
        {
            var comment = await _reviewService.GetAsync(id);

            if (comment is null)
            {
                return NotFound();
            }

            return comment;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ReviewDTO newComment)
        {
            await _reviewService.CreateAsync(newComment);

            return CreatedAtAction(nameof(Get) ,newComment);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Review updatedComment)
        {
            var comment = await _reviewService.GetAsync(id);

            if (comment is null)
            {
                return NotFound();
            }

            updatedComment.Id = comment.Id;

            await _reviewService.UpdateAsync(id, updatedComment);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var comment = await _reviewService.GetAsync(id);

            if (comment is null)
            {
                return NotFound();
            }

            await _reviewService.RemoveAsync(id);

            return NoContent();
        }
    }
}