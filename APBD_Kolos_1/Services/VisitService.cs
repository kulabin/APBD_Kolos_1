using System.Data;
using APBD_Kolos_1.Model.DTOs;
using Microsoft.Data.SqlClient;

namespace APBD_Kolos_1.Services;

public class VisitService : IVisitService
{
    private readonly string _connectionString =
        "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True";

    public async Task<bool> DoesClientExist(int id)
    {
        var command = "SELECT 1 FROM Client WHERE Id = @id";
        await using (SqlConnection conn = new SqlConnection(_connectionString))
        await using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result != null;
        }

    }

    public async Task<bool> DoesVisitExist(int id)
    {
        var command = "SELECT 1 FROM Visit WHERE Id = @id";
        await using (SqlConnection conn = new SqlConnection(_connectionString))
        await using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result != null;
        }
    }

    public async Task<bool> DoesMechanicExist(int id)
    {
        var command = "SELECT 1 FROM Mechanic WHERE Id = @id";
        await using (SqlConnection conn = new SqlConnection(_connectionString))
        await using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result != null;
        }
    }

    public async Task<bool> DoesServiceExist(int id)
    {
        var command = "SELECT 1 FROM Service WHERE Id = @id";
        await using (SqlConnection conn = new SqlConnection(_connectionString))
        await using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result != null;
        }
    }

    public async Task<VisitDTO> GetVisit(int id)
    {
        VisitDTO visit  = null;
        
        var command = @"SELECT v.date , c.first_name,c.last_name,
                            c.date_of_birth,m.mechanic_id,m.licence_number,s.name,vs.service_fee 
                            from Visit v join Client c on c.client_id = v.client_id
                            join Mechanic m on m.mechanic_id = v.mechanic_id
                            join Visit_Service vs on vs.service_id = v.service_id 
                            join Service s on s.service_id = vs.service_id
                            where v.id = @id";
        
        using(SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int idOrdinal = reader.GetOrdinal("visit_id");
                    visit = new VisitDTO()
                    {
                        _date = reader.GetDateTime(idOrdinal),
                        client = new ClientDTO(),
                        mechanic = new MechanicDTO(),
                        services = new List<VisitServiceDTO>()
                    };
                    var client = new ClientDTO()
                    {
                        first_name = reader.GetString(1),
                        last_name = reader.GetString(2),
                        date_of_birth = reader.GetDateTime(3)
                    };
                    var mechanic = new MechanicDTO()
                    {
                        mechanic_Id = reader.GetInt32(0),
                        licence_number = reader.GetString("licence_number")
                    };
                    var VisitService = new VisitServiceDTO()
                    {
                        service = new ServiceDTO()
                        {
                            name = reader.GetString("name"),
                        },
                        service_fee = reader.GetInt32("service_fee")
                    };
                    visit.client = client;
                    visit.mechanic = mechanic;
                    visit.services.Add(VisitService);
                }
            }
        }

        return visit;
    }

    public async Task CreateVisit(NewVisitDTO newVisit)
    {
        var command = @"INSERT INTO Visit Values (@visit_id,@client_id,@mechanic_id,@date); SELECT @@IDENTITY AS visit_id;";
        
        await using (SqlConnection conn = new SqlConnection(_connectionString))
        await using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@visit_id", newVisit.NewVisitId);
            cmd.Parameters.AddWithValue("@client_id", newVisit.client_id);
            cmd.Parameters.AddWithValue("@mechanic_id", newVisit.mechanic_id);
            cmd.Parameters.AddWithValue("@date", newVisit.date);
            
            await conn.OpenAsync();
            var transaction = conn.BeginTransaction();
            cmd.Transaction = transaction as SqlTransaction;
            try
            {
                var id = await cmd.ExecuteScalarAsync();
                foreach (var service in newVisit.visits)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "INSERT INTO Visit_Service VALUES (@visit_id,@service_id,@service_fee)";
                    cmd.Parameters.AddWithValue("@visit_id", newVisit.NewVisitId);
                    cmd.Parameters.AddWithValue("@service_id", service);
                    cmd.Parameters.AddWithValue("@service_fee", service.service_fee);
                    await cmd.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        } 
    }

    
}