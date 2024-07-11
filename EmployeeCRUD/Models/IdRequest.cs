using System.ComponentModel.DataAnnotations;

namespace EmployeeCRUD.Models
{
    public class IdRequest
    {
            [Required]
            public int Id { get; set; }
       
    }
}
