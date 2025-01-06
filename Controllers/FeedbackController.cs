using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using project3api_be.Data;
using project3api_be.Models;
using Microsoft.EntityFrameworkCore;

namespace project3api_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Endpoint tạo feedback
        [HttpPost("create-feedback")]
        public async Task<ActionResult<Feedback>> CreateFeedback([FromBody] Feedback feedback)
        {
            if (string.IsNullOrEmpty(feedback.FullName) || string.IsNullOrEmpty(feedback.Email))
            {
                return BadRequest(new { message = "FullName and Email are required" });
            }

            feedback.CreatedAt = DateTime.Now;
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllFeedbacks), new { id = feedback.FeedbackId }, feedback);
        }

        // Endpoint lấy danh sách tất cả feedback
        [HttpGet("get-allfeedback")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllFeedbacks()
        {
            var feedbacks = await _context.Feedbacks
                .Select(f => new
                {
                    f.FullName,
                    f.Title,
                    f.Content,
                    f.CreatedAt
                })
                .ToListAsync();

            return Ok(feedbacks);
        }

        // Endpoint sửa feedback
        [HttpPut("edit-feedback/{id}")]
        public async Task<ActionResult> EditFeedback(int id, [FromBody] Feedback updatedFeedback)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound(new { message = "Feedback not found" });
            }
            
            feedback.Title = updatedFeedback.Title ?? feedback.Title;
            feedback.Content = updatedFeedback.Content ?? feedback.Content;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Endpoint xóa feedback
        [HttpDelete("delete-feedback/{id}")]
        public async Task<ActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound(new { message = "Feedback not found" });
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Endpoint quản lý feedback cho admin
        [HttpGet("manager-feedback")]
        public async Task<ActionResult<IEnumerable<Feedback>>> ManagerFeedback()
        {
            return Ok(await _context.Feedbacks.ToListAsync());
        }
    }
}
