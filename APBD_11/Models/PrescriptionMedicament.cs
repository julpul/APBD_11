using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APBD_11.Models;

[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
public class PrescriptionMedicament
{
    [ForeignKey(nameof(IdMedicament))] public int IdMedicament { get; set; }
    [ForeignKey(nameof(IdPrescription))] public int IdPrescription { get; set; }
    public int Dose { get; set; }
    [Required] [MaxLength(100)] public string Details { get; set; }

    public Medicament Medicament { get; set; }
    public Prescription Prescription { get; set; }
}