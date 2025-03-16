using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Comp2139Lab1.Data;
using Comp2139Lab1.Areas.ProjectManagement.Models;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Comp2139Lab1.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("ProjectManagement/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectManagement/Projects
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index(string searchString, bool isSearching = false)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["IsSearching"] = isSearching;

            var projects = from p in _context.Projects
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(p => p.Name.Contains(searchString)
                                      || p.Description != null && p.Description.Contains(searchString));
            }

            return View(await projects.Include(p => p.Tasks).ToListAsync());
        }

        // GET: ProjectManagement/Projects/Search
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string searchString)
        {
            return await Index(searchString, true);
        }

        // GET: ProjectManagement/Projects/Details/5
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: ProjectManagement/Projects/Create
        [HttpGet("Create")]
        public IActionResult Create() => View();

        // POST: ProjectManagement/Projects/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Convert local dates to UTC for PostgreSQL
                    if (project.StartDate.Kind != DateTimeKind.Utc)
                    {
                        project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
                    }
                    
                    if (project.EndDate.Kind != DateTimeKind.Utc)
                    {
                        project.EndDate = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc);
                    }
                    
                    _context.Projects.Add(project);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating project: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError("", ex.InnerException.Message);
                    }
                }
            }
            return View(project);
        }

        // GET: ProjectManagement/Projects/Edit/5
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: ProjectManagement/Projects/Edit/5
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Convert local dates to UTC for PostgreSQL
                    if (project.StartDate.Kind != DateTimeKind.Utc)
                    {
                        project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
                    }
                    
                    if (project.EndDate.Kind != DateTimeKind.Utc)
                    {
                        project.EndDate = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc);
                    }
                    
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating project: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        ModelState.AddModelError("", ex.InnerException.Message);
                    }
                    return View(project);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: ProjectManagement/Projects/Delete/5
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: ProjectManagement/Projects/Delete/5
        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
} 