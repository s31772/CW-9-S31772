using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd_cw9.Models;
[Table("Prescription")]
public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }

    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    
    [Column("Patient_Id")]
    public int IdPatient { get; set; }
    [ForeignKey("IdPatient")]
    public Patient Patient { get; set; }
    
    [Column("Doctor_Id")]
    public int IdDoctor { get; set; }
    [ForeignKey("IdDoctor")]
    public Doctor Doctor { get; set; }
    
    public ICollection<Prescription_Medicament> Prescription_Medicaments { get; set; }
}