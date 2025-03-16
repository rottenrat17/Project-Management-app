using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Comp2139Lab1.Models;
using Comp2139Lab1.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Comp2139Lab1.Controllers;

[Route("")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    [HttpGet("index")]
    [HttpGet("home")]
    [HttpGet("home/index")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("privacy")]
    [HttpGet("home/privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet("search")]
    [HttpGet("home/search")]
    public async Task<IActionResult> Search(string searchType, string searchString)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            return RedirectToAction(nameof(Index));
        }

        ViewData["SearchString"] = searchString;
        ViewData["SearchType"] = searchType;

        if (searchType == "Project")
        {
            var projects = await _context.Projects
                .Where(p => p.Name.Contains(searchString) 
                    || p.Description != null && p.Description.Contains(searchString))
                .Include(p => p.Tasks)
                .ToListAsync();
            
            return View("SearchResults", projects);
        }
        else if (searchType == "Task")
        {
            var tasks = await _context.ProjectTasks
                .Where(t => t.Title.Contains(searchString) 
                    || t.Description != null && t.Description.Contains(searchString))
                .Include(t => t.Project)
                .ToListAsync();
            
            return View("SearchTaskResults", tasks);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("not-found")]
    [HttpGet("home/not-found")]
    public new IActionResult NotFound()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpGet("error")]
    [HttpGet("home/error")]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
