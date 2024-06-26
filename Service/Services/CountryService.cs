using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Countries;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CountryService> _logger;

        public CountryService(ICountryRepository countryRepository,
                              IMapper mapper, 
                              ILogger<CountryService> logger)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateAsync(CountryCreateDto data)
        {
            if (data is null) throw new ArgumentNullException(nameof(data));

            await _countryRepository.CreateAsync(_mapper.Map<Country>(data));
        }

        public async Task EditAsync(int? id, CountryEditDto data)
        {
            if (id is null) throw new ArgumentNullException(nameof(id));

            if (data is null) throw new ArgumentNullException(nameof(data));

            var country = await _countryRepository.GetByIdAsync((int)id) ?? throw new NotFoundException("Data not found");

            _mapper.Map(data, country);
            await _countryRepository.EditAsync(country);
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null)
            {
                _logger.LogWarning("id is null");
                throw new ArgumentNullException(nameof(id));
            }

            var country = await _countryRepository.GetByIdAsync((int)id);

            if (country is null)
            {
                _logger.LogWarning("Data not found");
                throw new NotFoundException("Data not found");
            }

            await _countryRepository.DeleteAsync(country);
        }

        public async Task<IEnumerable<CountryDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CountryDto>>(await _countryRepository.GetAllAsync());
        }

        public async Task<CountryDetailDto> GetByIdAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException(nameof(id));

            var country = await _countryRepository.GetByIdAsync((int)id) ?? throw new NotFoundException("Data not found");

            return _mapper.Map<CountryDetailDto>(country);
        }
    }
}
