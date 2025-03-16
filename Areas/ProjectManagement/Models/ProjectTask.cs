using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Comp2139Lab1.Areas.ProjectManagement.Models
{
    public class ProjectTask
    {
        [Key]
        public int ProjectTaskId { get; set; }

        [Required]
        public string Title { get; set; } = "";
        
        public string? Description { get; set; }

        public string Status { get; set; } = "New";

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        // This is a navigation property - not required during creation
        public Project? Project { get; set; }
    }
} 