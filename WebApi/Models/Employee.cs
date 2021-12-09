using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Employee
    {
        [Key] //主键
        public int Id { get; set; }
        [Required] //必填项
        public string FristName { get; set; }
        [Required] //必填项
        public string LastName { get; set; }
        public DateTime HiredDate { get; set; }
    }
}
