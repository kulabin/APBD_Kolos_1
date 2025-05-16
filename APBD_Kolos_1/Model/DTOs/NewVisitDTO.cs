namespace APBD_Kolos_1.Model.DTOs;

public class NewVisitDTO
{
    public int NewVisitId { get; set; }
    public int client_id { get; set; }
    public int mechanic_id { get; set; }
    public IEnumerable<VisitServiceDTO> visits { get; set; } = new List<VisitServiceDTO>();
    public DateTime date { get; set; }
}