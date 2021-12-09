using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Relation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Eid { get; set; }
        [Required]
        public int Tid { get; set; }
    }
}
