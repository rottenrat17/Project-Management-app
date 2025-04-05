using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Comp2139Lab1.Data;
using Comp2139Lab1.Areas.ProjectManagement.Models;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Comp2139Lab1.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("ProjectManagement/[controller]")]
    public class ProjectTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectManagement/ProjectTask
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index(int? projectId, string searchString, bool isSearching = false)
        {
            if (projectId == null)
            {
                return NotFound();
            }

            ViewData["ProjectId"] = projectId;
            ViewData["CurrentFilter"] = searchString;
            ViewData["IsSearching"] = isSearching;

            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                return NotFound();
            }

            var tasks = project.Tasks.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(t => t.Title.Contains(searchString) 
                                 || t.Description != null && t.Description.Contains(searchString))
                             .AsQueryable();
            }

            return View(tasks.ToList());
        }

        // GET: ProjectManagement/ProjectTask/Search
        [HttpGet("Search")]
        public async Task<IActionResult> Search(int? projectId, string searchString)
        {
            return await Index(projectId, searchString, true);
        }

        // GET: ProjectManagement/ProjectTask/Details/5
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTask = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (projectTask == null)
            {
                return NotFound();
            }

            return View(projectTask);
        }

        // GET: ProjectManagement/ProjectTask/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create(int? projectId)
        {
            if (projectId == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            var projectTask = new ProjectTask
            {
                ProjectId = project.Id,
                Status = "New"
            };

            return View(projectTask);
        }

        // POST: ProjectManagement/ProjectTask/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectTask projectTask)
        {
            // Log information for debugging
            Console.WriteLine($"Received task: Title={projectTask.Title}, ProjectId={projectTask.ProjectId}");
            
            try
            {
                // Remove the Project property from ModelState validation
                ModelState.Remove("Project");
                
                if (ModelState.IsValid)
                {
                    // Ensure we load the full project details
                    var project = await _context.Projects.FindAsync(projectTask.ProjectId);
                    if (project == null)
                    {
                        ModelState.AddModelError("", "Project not found");
                        return View(projectTask);
                    }

                    // Reset tracking to avoid concurrency issues
                    _context.Entry(project).State = EntityState.Detached;

                    // Ensure we're creating a clean task object
                    var newTask = new ProjectTask
                    {
                        Title = projectTask.Title,
                        Description = projectTask.Description,
                        Status = projectTask.Status,
                        ProjectId = projectTask.ProjectId
                    };

                    // Add the new task
                    _context.Add(newTask);
                    
                    // Save with detailed error handling
                    try
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Details", "Projects", new { area = "ProjectManagement", id = projectTask.ProjectId });
                    }
                    catch (DbUpdateException dbEx)
                    {
                        Console.WriteLine("Database error: " + dbEx.Message);
                        if (dbEx.InnerException != null)
                        {
                            Console.WriteLine("Inner exception: " + dbEx.InnerException.Message);
                            ModelState.AddModelError("", "Database error: " + dbEx.InnerException.Message);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Database error: " + dbEx.Message);
                        }
                    }
                }
                else
                {
                    // Log validation errors
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    foreach (var error in errors)
                    {
                        Console.WriteLine("Validation error: " + error);
                        ModelState.AddModelError("", error);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                }
                ModelState.AddModelError("", "Error creating task: " + ex.Message);
            }
            
            // If we got this far, something failed, redisplay form
            return View(projectTask);
        }

        // GET: ProjectManagement/ProjectTask/Edit/5
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTask = await _context.ProjectTasks.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }

            return View(projectTask);
        }

        // POST: ProjectManagement/ProjectTask/Edit/5
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectTask projectTask)
        {
            if (id != projectTask.ProjectTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectTaskExists(projectTask.ProjectTaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Projects", new { area = "ProjectManagement", id = projectTask.ProjectId });
            }
            return View(projectTask);
        }

        // GET: ProjectManagement/ProjectTask/Delete/5
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTask = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (projectTask == null)
            {
                return NotFound();
            }

            return View(projectTask);
        }

        // POST: ProjectManagement/ProjectTask/Delete/5
        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectTask = await _context.ProjectTasks.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }

            int projectId = projectTask.ProjectId;
            _context.ProjectTasks.Remove(projectTask);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", "Projects", new { area = "ProjectManagement", id = projectId });
        }

        private async Task<bool> ProjectTaskExists(int id)
        {
            return await _context.ProjectTasks.AnyAsync(e => e.ProjectTaskId == id);
        }
    }
} 