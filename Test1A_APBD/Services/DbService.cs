using System.Data.Common;
using Test1A_APBD.Exceptions;
using Test1A_APBD.Models.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Test1A_APBD.Services;

public class DbService : IDbService
{
    private readonly string _connectionString;
    public DbService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? string.Empty;
    }
    
    public async Task<AppointmentServiceHistoryDTO> GetAppointmentHistory(int appointmentId)
    {
        var query =
            @"SELECT date, p.first_name, p.last_name, p.date_of_birth, d.doctor_id, d.PWZ, s.name, s.base_fee
            FROM Appointment a
            JOIN Patient p on a.patient_id =  p.patient_id
            JOIN Doctor d on a.doctor_id = d.doctor_id
            JOIN Appointment_Service ap on a.appointment_id = ap.appointment_id
            JOIN Service s on ap.service_id = s.service_id
            WHERE a.appointment_id = @appointmentId";
        
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = query;
        await connection.OpenAsync();
        
        command.Parameters.AddWithValue("@appointmentId", appointmentId);
        var reader = await command.ExecuteReaderAsync();
        
        AppointmentServiceHistoryDTO appointments = null;
        
        while (await reader.ReadAsync())
        {
            if (appointments is null)
            {
                appointments = new AppointmentServiceHistoryDTO()
                {
                    Date = reader.GetDateTime(0),
                    Patient = new List<PatientInformationDTO>(),
                    Doctor = new List<DoctorInformationDTO>(),
                    AppointmentServices = new List<AppointmentServiceDTO>()
                };
            }
            
            string patientname = reader.GetString(1);

            var patient = appointments.Patient.FirstOrDefault(e => e.firstName.Equals(patientname));
            if (patient is null)
            {
                patient = new PatientInformationDTO()
                {
                    firstName = patientname,
                    lastName = reader.GetString(2),
                    dateOfBirth = reader.GetDateTime(3),
                };
                appointments.Patient.Add(patient);
            }
            
            int doctor_Id = reader.GetInt32(4);

            var doctor = appointments.Doctor.FirstOrDefault(e => e.doctorId.Equals(doctor_Id));
            if (doctor is null)
            {
                doctor = new DoctorInformationDTO()
                {
                    doctorId = doctor_Id,
                    pwz = reader.GetString(5)
                };
                appointments.Doctor.Add(doctor);
            }
            
            string servicename = reader.GetString(6);

            var appointment = appointments.AppointmentServices.FirstOrDefault(e => e.name.Equals(servicename));
            if (appointment is null)
            {
                appointment = new AppointmentServiceDTO()
                {
                    name = servicename,
                    serviceFee = reader.GetDouble(7)
                };
                appointments.AppointmentServices.Add(appointment);
            }
            
            
        }       
        
        if (appointments is null)
        {
            throw new NotFoundException("No appointmets found.");
        }
        
        return appointments;
    }
}