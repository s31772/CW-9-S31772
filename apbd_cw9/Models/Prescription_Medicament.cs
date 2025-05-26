using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw9.Models;
[Table("Prescription_Medicament")]
[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
public class Prescription_Medicament
{
    [Column("Medicament_Id")]
    public int IdMedicament { get; set; }
    
    [ForeignKey(nameof(IdMedicament))]
    public Medicament Medicament { get; set; }  

    [Column("Prescription_Id")]
    public int IdPrescription { get; set; }
    [ForeignKey(nameof(IdPrescription))]
    public Prescription Prescription { get; set; }  

    public int? Dose { get; set; }  

    [MaxLength(100)]
    public string Details { get; set; }
}