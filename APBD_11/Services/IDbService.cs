using APBD_11.DTOs;

namespace APBD_11.Services;

public interface IDbService
{
    Task AddPrescription(NewRecepraDto dto);
    Task<PatientGetDto> GetPatient(int id);

}