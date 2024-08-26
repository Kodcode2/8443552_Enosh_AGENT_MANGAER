using Microsoft.AspNetCore.Mvc;

namespace AgentsClient.Service
{
    public interface ILoginService
    {
        Task LoginAsync();
    }
}
