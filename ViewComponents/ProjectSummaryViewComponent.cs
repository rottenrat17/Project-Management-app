using Microsoft.AspNetCore.Mvc;
using Comp2139Lab1.Data;
using Comp2139Lab1.Areas.ProjectManagement.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Comp2139Lab1.ViewComponents
{
    public class ProjectSummaryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ProjectSummaryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? projectId = null, int count = 3)
        {
            var projects = await GetProjectsAsync(projectId, count);
            return View(projects);
        }

        private async Task<IEnumerable<Project>> GetProjectsAsync(int? projectId, int count)
        {
            if (projectId.HasValue)
            {
                // Return a specific project
                var project = await _context.Projects
                    .Include(p => p.Tasks)
                    .FirstOrDefaultAsync(p => p.Id == projectId.Value);
                
                return project != null ? new List<Project> { project } : Enumerable.Empty<Project>();
            }
            else
            {
                // Return the most recent projects
                return await _context.Projects
                    .Include(p => p.Tasks)
                    .OrderByDescending(p => p.Id)
                    .Take(count)
                    .ToListAsync();
            }
        }
    }
} 