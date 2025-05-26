using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_cw9.Models;

[Table("Doctor")]
public class Doctor
{
    [Key]
    public int IdDoctor { get; set; }
    
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [MaxLength(100)]
    public string LastName { get; set; }
    
    [MaxLength(100)]
    [RegularExpression(@".+@.+\..+", ErrorMessage = "Email musi zawierać znak '@' oraz kropkę po '@'.")]
    public string Email { get; set; }
    
    public ICollection<Prescription> Prescriptions { get; set; }
}