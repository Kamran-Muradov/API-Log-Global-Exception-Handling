using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Cities;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepo;
        private readonly ICountryRepository _countryRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CityService> _logger;

        public CityService(ICityRepository cityRepo,
                           ICountryRepository countryRepo,
                           IMapper mapper,
                           ILogger<CityService> logger)
        {
            _cityRepo = cityRepo;
            _countryRepo = countryRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateAsync(CityCreateDto data)
        {
            var existCountry = await _countryRepo.GetByIdAsync(data.CountryId);

            if (existCountry is null)
            {
                _logger.LogWarning($"Country not found - {data.CountryId} - {DateTime.Now}");
                throw new NotFoundException("Data Not Found");
            }

            await _cityRepo.CreateAsync(_mapper.Map<City>(data));
        }

        public async Task<IEnumerable<CityDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CityDto>>(await _cityRepo.GetAllWithCountryAsync());
        }

        public async Task<CityDto> GetByIdAsync(int id)
        {
            return _mapper.Map<CityDto>(await _cityRepo
                .FindBy(m => m.Id == id, m => m.Country)
                .FirstOrDefaultAsync());
        }

        public async Task<CityDto> GetByNameAsync(string name)
        {
            ArgumentNullException.ThrowIfNull(nameof(name));

            return _mapper.Map<CityDto>(await _cityRepo
                .FindBy(m => m.Name == name, m => m.Country)
                .FirstOrDefaultAsync());
        }
    }
}
