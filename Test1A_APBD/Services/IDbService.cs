using Test1A_APBD.Models.DTO;

namespace Test1A_APBD.Services;

public interface IDbService
{
    Task<AppointmentServiceHistoryDTO> GetAppointmentHistory(int id);
}