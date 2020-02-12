using FleetManagement.Entities;
using FleetManagement.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FleetManagement.API.BAL
{
    public class CustomerManager
    {
        private readonly IDocumentDBRepository<Customer> Respository;
        public CustomerManager(IDocumentDBRepository<Customer> _respository)
        {
            Respository = _respository;
        }

        public async Task<Customer> CreateAsync(Customer newcustomer)
        {
            var item = await Respository.CreateItemAsync(newcustomer);
            return (Customer)(dynamic)item;
        }

        public async Task<Customer> UpdateAsync(string id, Customer updatedcustomer)
        {
            var item = await Respository.UpdateItemAsync(id, updatedcustomer);
            return (Customer)(dynamic)item;
        }

        public async Task DeleteAsync(string id)
        {
            Customer item = await Respository.GetItemAsync(id);
            // just using the manager to do something extra

            if (item == null)
            {
                throw new Exception("Id can not be found");
            }
            await Respository.DeleteItemAsync(id);
        }

        public async Task<List<Customer>> ListAsync()
        {
            List<Customer> customers =  (await Respository.GetAllItemsAsync()).ToList();
            return customers;
        }

        public async Task<List<Customer>> FilterByNameAsync(string searchname)
        {
            List<Customer> customers = (await Respository.GetItemsAsync(x=>x.Name.ToLower().Contains(searchname.ToLower()))).ToList();
            return customers;
        }

        public async Task<Customer> GetAsync(string id)
        {
            return await Respository.GetItemAsync(id);
        }
    }
}
