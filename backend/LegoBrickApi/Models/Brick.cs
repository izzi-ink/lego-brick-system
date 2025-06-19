using System.ComponentModel.DataAnnotations;

namespace LegoBrickApi.Models
{
    public class Brick
    {
        // Primary key - auto-generated
        public int Id { get; set; }
        
        // Required fields
        [Required]
        [StringLength(100)]
        public string ServiceName { get; set; } = "";
        
        [Required]
        [StringLength(100)]
        public string ProjectName { get; set; } = "";
        
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = "";
        
        // What this service needs as input
        [StringLength(300)]
        public string DataConsumed { get; set; } = "";
        
        // What this service outputs
        [StringLength(300)]
        public string DataProduced { get; set; } = "";
        
        // Categories/tags for matching
        [StringLength(200)]
        public string Tags { get; set; } = "";
        
        [Required]
        [StringLength(100)]
        public string CreatorName { get; set; } = "";
        
        // Automatically set when created
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}