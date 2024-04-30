using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReserveServer.Data;
using ReserveServer.ReserveModel;

namespace ReserveServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ReservationGoldenContext _db;
        private readonly string _pathName;

        public SeedController(ReservationGoldenContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _pathName = Path.Combine(environment.ContentRootPath, "Data/info.csv");
        }

        [HttpPost("Customer")]
        public async Task<ActionResult<Reservation>> SeedCustomer()
        {
            // create a lookup dictionary containing all the countries already existing 
            // into the Database (it will be empty on first run).
            Dictionary<string, Customer> customersByName = _db.Customers
                .AsNoTracking().ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);

            List<ReserveCustomersCSV> records = csv.GetRecords<ReserveCustomersCSV>().ToList();
            foreach (ReserveCustomersCSV record in records)
            {
                if (customersByName.ContainsKey(record.name))
                {
                    continue;
                }

                Customer customer = new()
                {
                    Name = record.name,
                    Email = record.email,
                    Phone = record.phone_number
                };
                await _db.Customers.AddAsync(customer);
                customersByName.Add(record.name, customer);
            }

            await _db.SaveChangesAsync();

            return new JsonResult(customersByName.Count);

        }

        [HttpPost("Reservation")]
        public async Task<ActionResult<Reservation>> SeedReservation()
        {
            Dictionary<string, Customer> reservation = await _db.Customers//.AsNoTracking()
            .ToDictionaryAsync(r => r.Name);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null
            };
            int reservationCount = 0;
            using (StreamReader reader = new(_pathName))
            using (CsvReader csv = new(reader, config))
            {
                IEnumerable<ReserveCustomersCSV>? records = csv.GetRecords<ReserveCustomersCSV>();
                foreach (ReserveCustomersCSV record in records)
                {
                    if (!reservation.TryGetValue(record.name, out Customer? value))
                    {
                        Console.WriteLine($"Not found customer for {record.name}");
                        return NotFound(record);
                    }

                    Reservation reservations = new()
                    {
                        CustomerId = value.CustomerId,
                        ReservationDate = record.reservation_date,
                        ReservationTime = record.reservation_time,
                        PartySize = record.party_size,
                        SpecialRequests = record.special_requests,
                    };
                    _db.Reservations.Add(reservations);
                    reservationCount++;
                }
                await _db.SaveChangesAsync();
            }
            return new JsonResult(reservationCount);
        }
    }
}
