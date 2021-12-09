namespace WebApi.Models
{
    public class Employee_Gen
    {
        public int Id { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public DateTime HiredDate { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
