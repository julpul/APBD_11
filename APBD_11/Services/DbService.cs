using APBD_11.Data;
using APBD_11.DTOs;
using APBD_11.Exeptions;
using APBD_11.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_11.Services;

public class DbService : IDbService
{
   private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddPrescription(NewRecepraDto dto)
    {
        if (dto.DueDate < dto.Date)
            throw new ConflictException("Due date cannot be earlier than prescription date.");

        if (dto.medicaments.Count > 10)
            throw new ConflictException("Too many medicaments.");

        var patient = await _context.Patients
            .FirstOrDefaultAsync(p =>
                p.Firstname == dto.Patient.FirstName &&
                p.Lastname == dto.Patient.LastName &&
                p.DateOfBirth == dto.Patient.Birthdate);

        if (patient == null)
        {
            patient = new Patient
            {
                Firstname = dto.Patient.FirstName,
                Lastname = dto.Patient.LastName,
                DateOfBirth = dto.Patient.Birthdate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        var doctor = await _context.Doctors.FindAsync(dto.IdDoctor);
        if (doctor == null)
            throw new NotFoundException("Doctor not found.");

        var medicaments = await _context.Medicaments
            .Where(m => dto.medicaments.Select(x => x.IdMedicament).Contains(m.IdMedicament))
            .ToListAsync();

        if (medicaments.Count != dto.medicaments.Count)
            throw new NotFoundException("One or more medicaments not found.");

        var prescription = new Prescription
        {
            Date = dto.Date.ToDateTime(TimeOnly.MinValue),
            DueDate = dto.DueDate.ToDateTime(TimeOnly.MinValue),
            IdDoctor = dto.IdDoctor,
            IdPatient = patient.IdPatioent, // <- pamiętaj: tu nadal literówka w modelu!
            PrescriptionMedicaments = dto.medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Description // zgodnie z DTO
            }).ToList()
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
    }


    public async Task<PatientGetDto> GetPatient(int id)
    {
        var patient = await _context.Patients
            .Where(p => p.IdPatioent == id)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync();



        if (patient == null)
            throw new NotFoundException("Patient not found");

        return new PatientGetDto
        {
            IdPatient = patient.IdPatioent,
            FirstName = patient.Firstname,
            LastName = patient.Lastname,
            Birthdate = patient.DateOfBirth.ToDateTime(TimeOnly.MinValue),
            Prescriptions = patient.Prescriptions?
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = p.Doctor == null ? null : new DoctorDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName
                    },
                    Medicaments = p.PrescriptionMedicaments?
                        .Select(pm => new MedicamentGetDto
                        {
                            IdMedicament = pm.IdMedicament,
                            Name = pm.Medicament?.Name,
                            Description = pm.Medicament?.Description,
                            Dose = pm.Dose
                        }).ToList() ?? new List<MedicamentGetDto>()
                }).ToList() ?? new List<PrescriptionDto>()
        };
    }

}