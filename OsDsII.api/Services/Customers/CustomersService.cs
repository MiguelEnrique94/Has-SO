﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OsDsII.api.Controllers;
using OsDsII.api.Dtos.Customers;
using OsDsII.api.Exceptions;
using OsDsII.api.Models;
using OsDsII.api.Repository.Customers;

namespace OsDsII.api.Services.Customers
{
    public sealed class CustomersService : ICustomersService
    {
        private readonly ICustomersRepository _customersRepository;
        private readonly IMapper _mapper;

        public CustomersService(ICustomersRepository customersRepository, IMapper mapper)
        {
            _customersRepository = customersRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateCustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);

            var customerExists = await _customersRepository.GetCustomerByEmailAsync(customer.Email);
            if (customerExists != null && !customerExists.Equals(customer))
            {
                throw new ConflictException("Customer already exists");
            }

            await _customersRepository.AddCustomerAsync(customer);
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            
            List<Customer> customers = await _customersRepository.GetAllAsync();
            var customersDto = _mapper.Map<List<CustomerDto>>(customers);
            return customersDto;


        } 
        
        public async Task<CustomerDto> GetByIdAsync(int id)
        {
            Customer customer = await _customersRepository.GetByIdAsync(id);
            
            if (customer is null)
            {
                throw new NotFoundException("Customer not found");
            }
            var customerDto = _mapper.Map<CustomerDto>(customer);
            return customerDto;
        }

        public async Task DeleteAsync(int id)
        {
            Customer customer = await _customersRepository.GetByIdAsync(id);
            if (customer is null)
            {
                throw new NotFoundException("Customer not found");
            }
            await _customersRepository.DeleteCustomerAsync(customer);
        }

        public async Task UpdateAsync(int id, CreateCustomerDto customer)
        {
            Customer customerExists = await _customersRepository.GetByIdAsync(id);
            if (customerExists is null)
           throw new NotFoundException("Customer not found");
            {
            }
            customerExists.Name = customer.Name;
            customerExists.Email = customer.Email;
            customerExists.Phone = customer.Phone;

            await _customersRepository.UpdateCustomerAsync(customerExists);
        }
    }
}
