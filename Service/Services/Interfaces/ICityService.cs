using Service.DTOs.Admin.Cities;

namespace Service.Services.Interfaces
{
    public interface ICityService
    {
        Task CreateAsync(CityCreateDto data);
        Task<IEnumerable<CityDto>> GetAllAsync();
        Task<CityDto> GetByIdAsync(int id);
        Task<CityDto> GetByNameAsync(string name);
    }
}
