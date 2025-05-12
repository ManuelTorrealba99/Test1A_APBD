namespace Test1A_APBD.Models.DTO;

public class AppointmentRequestDTO
{
    public int appointmentId { get; set; }
    public int patientId { get; set; }
    public string pwz { get; set; }
    public List<ServiceDTO> services { get; set; }
}

public class ServiceDTO
{
    public string serviceName { get; set; }
    public double serviceFee { get; set; }
}