using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Comp2139Lab1.Areas.ProjectManagement.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        public string? Description { get; set; }
        
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.SpecifyKind(DateTime.Now.AddMonths(1), DateTimeKind.Utc);
        
        [Required]
        public string Status { get; set; } = "New";

        public List<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
} 