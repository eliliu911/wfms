namespace WebApi.Models
{
    public class V_EmployeeTasks
    {
        public int Id { get; set; }
        public int Eid { get; set; }
        public int Tid { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public DateTime HiredDate { get; set; }
        public string TaskName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Deadline { get; set; }
    }
}
