using AgentsClient.ViewModel;

namespace AgentsClient.Service
{
    public interface ITableService
    {
        Task<PositionsVM> GetAllPositionsAsync();
    }
}
