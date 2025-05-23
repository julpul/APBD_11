using System.Reflection.Metadata.Ecma335;

namespace APBD_11.DTOs;

public class NewRecepraDto
{
    public PatientDto Patient { get; set; }
    public List<MedicamentsDto> medicaments { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    
    public int IdDoctor { get; set; }
}
public class PatientDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly Birthdate { get; set; }
}
public class MedicamentsDto
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
}

