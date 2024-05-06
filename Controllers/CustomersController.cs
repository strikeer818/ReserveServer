using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReserveServer.ReserveModel;

namespace ReserveServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(ReservationGoldenContext context) : ControllerBase
    {
        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await context.Customers.ToListAsync();
        }
    }
}