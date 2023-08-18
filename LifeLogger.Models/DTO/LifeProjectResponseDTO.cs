
namespace LifeLogger.Models.DTO
{
    public class LifeProjectResponseDTO
    {
        public int ProjectId { get; set; }

        public string UserID{ get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsPublic { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int LifeMileStoneCount{ get; set; }
    }
}