using System;
using System.Linq;
using System.Threading.Tasks;
using Comp2139Lab1.Data;
using Comp2139Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comp2139Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectCommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectCommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjectComment/GetComments/5
        [HttpGet("GetComments/{projectId}")]
        public async Task<IActionResult> GetComments(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            var comments = await _context.ProjectComments
                .Where(c => c.ProjectId == projectId)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return Ok(comments);
        }

        // POST: api/ProjectComment/AddComment
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment([FromBody] ProjectComment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _context.Projects.FindAsync(comment.ProjectId);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            comment.CreatedDate = DateTime.UtcNow;
            _context.ProjectComments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }
    }
} 