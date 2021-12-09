using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Task
    {
        [Key] //主键
        public int Id { get; set; }
        [Required] //必填项
        public string TaskName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Deadline { get; set; }
    }
}
