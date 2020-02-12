using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagement.API.BAL;
using FleetManagement.Entities;
using FleetManagement.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FleetController : ControllerBase
    {
        private readonly IDocumentDBRepository<Customer> respository;
        private CustomerManager customerManager;
        public FleetController(IDocumentDBRepository<Customer> _respository)
        {
            respository = _respository;
            customerManager = new CustomerManager(respository);
        }
        // GET: api/Fleet
        [HttpGet]
        public async Task<ActionResult> ListAsync([FromQuery]string name="")
        {
            List<Customer> customers = await customerManager.FilterByNameAsync(name);
            return new OkObjectResult(customers);
        }

        
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("Name,Address")] Customer item)
        {
            if (ModelState.IsValid)
            {
                Customer newcustomer = await customerManager.CreateAsync(item);
                return new OkObjectResult(newcustomer);
            }
            else
            {
                return BadRequest();
            }
        }
        
        [HttpPut]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind("Id,Name")] Customer item)
        {
            if (ModelState.IsValid)
            {
                Customer updatedcustomer = await customerManager.UpdateAsync(item.Id, item);
                return new OkObjectResult(updatedcustomer);
            }
            else
            {
                return BadRequest();
            }
        }


        // for the sake of creating some call without using validation 
        [HttpDelete]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                await customerManager.DeleteAsync(id);

                return new OkResult();
            }
            catch(Exception ex)
            {
                //Log(ex.Message);
                return new BadRequestResult();
            }
        }

        [HttpGet("~/Details")]
        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            Customer item = await customerManager.GetAsync(id);
            return new OkObjectResult(item);
        }
    }
}
