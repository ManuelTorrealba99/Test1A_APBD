namespace Test1A_APBD.Models.DTO;

public class AppointmentServiceHistoryDTO
{
    public DateTime Date { get; set; }
    public List<PatientInformationDTO> Patient { get; set; }
    public List<DoctorInformationDTO> Doctor { get; set; }
    public List<AppointmentServiceDTO> AppointmentServices { get; set; }
}

public class PatientInformationDTO
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public DateTime dateOfBirth { get; set; }
}

public class DoctorInformationDTO
{
    public int doctorId { get; set; }
    public string pwz { get; set; }
}

public class AppointmentServiceDTO
{
    public string name { get; set; }
    public double serviceFee { get; set; }
}