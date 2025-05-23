using System.ComponentModel.DataAnnotations;

namespace APBD_11.Models;

public class Patient
{
    [Key]
    public int IdPatioent { get; set; }
    [Required]
    [MaxLength(100)]
    public string Firstname { get; set; }
    [Required]
    [MaxLength(100)]
    public string Lastname { get; set; }
    [Required]
    public DateOnly DateOfBirth { get; set; }
    
    public ICollection<Prescription> Prescriptions { get; set; }
}