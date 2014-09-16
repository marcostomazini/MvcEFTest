using System.ComponentModel.DataAnnotations;

namespace MvcEFTest.Entities
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}