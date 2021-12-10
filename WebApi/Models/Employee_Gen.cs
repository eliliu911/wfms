namespace WebApi.Models
{
    public class Employee_Gen
    {
        public Employee Employee { get; set; }  = new Employee();
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
