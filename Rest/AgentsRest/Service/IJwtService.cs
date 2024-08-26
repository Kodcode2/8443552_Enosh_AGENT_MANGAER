using AgentsRest.Dto;

namespace AgentsRest.Service
{
    public interface IJwtService
    {
        string CreateToken(string id);
    }
}