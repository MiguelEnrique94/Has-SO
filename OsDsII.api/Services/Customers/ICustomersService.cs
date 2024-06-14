using OsDsII.api.Dtos.Customers;
using OsDsII.api.Models;

namespace OsDsII.api.Services.Customers
{
    public interface ICustomersService
    {
        public Task CreateAsync(CreateCustomerDto customer);
        public Task<List<CustomerDto>> GetAllAsync();
        public Task<CustomerDto> GetByIdAsync(int id);
        public Task DeleteAsync(int id);
        public Task UpdateAsync(int id, CreateCustomerDto customer);

    }
}
