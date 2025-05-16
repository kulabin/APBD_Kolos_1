using APBD_Kolos_1.Model.DTOs;

namespace APBD_Kolos_1.Services;

public interface IVisitService
{
    Task<bool> DoesClientExist(int id);
    Task<bool> DoesVisitExist(int id);
    Task<bool> DoesMechanicExist(int id);
    Task<bool> DoesServiceExist(int id);
    Task<VisitDTO> GetVisit(int id);
    Task CreateVisit(NewVisitDTO newVisit);
    
}