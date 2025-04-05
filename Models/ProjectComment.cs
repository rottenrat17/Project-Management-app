using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Comp2139Lab1.Areas.ProjectManagement.Models;

namespace Comp2139Lab1.Models
{
    public class ProjectComment
    {
        [Key]
        public int CommentId { get; set; }
        
        [Required(ErrorMessage = "Comment content is required")]
        public string Content { get; set; }
        
        public int ProjectId { get; set; }
        
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
} 