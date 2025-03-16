using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Comp2139Lab1.Data;
using Comp2139Lab1.Areas.ProjectManagement.Models;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Comp2139Lab1.Controllers
{
    [Route("Projects")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Projects
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.Include(p => p.Tasks).ToListAsync());
        }

        // GET: /Projects/Search
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData["CurrentFilter"] = searchString;

            var projects = await _context.Projects
                .Where(p => p.Name.Contains(searchString) 
                    || p.Description != null && p.Description.Contains(searchString))
                .Include(p => p.Tasks)
                .ToListAsync();
            
            return View(projects);
        }

        // GET: /Projects/Details/5
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

            return RedirectToAction("Details", "Projects", new { area = "ProjectManagement", id = project.Id });
        }

        // GET: /Projects/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return RedirectToAction("Create", "Projects", new { area = "ProjectManagement" });
        }
    }
} 