namespace APBD_Kolos_1.Model.DTOs;

public class VisitDTO
{
    public int visit_id { get; set; }
    public ClientDTO client { get; set; }
    public MechanicDTO mechanic { get; set; }
    public List<VisitServiceDTO> services { get; set; }
    public DateTime _date { get; set; }
}

public class VisitServiceDTO
{
    public int visit_id { get; set; }
    public ServiceDTO service { get; set; }
    public int service_fee { get; set; }
}