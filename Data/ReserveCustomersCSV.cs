using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReserveServer.Data
{
    public class ReserveCustomersCSV
    {
        //public int customer_id { get; set; }

        public string name { get; set; } = null!;

        public string email { get; set; } = null!;

        public string phone_number { get; set; } = null!;

        //public int reservation_id { get; set; }

        public DateOnly reservation_date { get; set; }

        public TimeOnly reservation_time { get; set; }

        public int? party_size { get; set; }

        public string? special_requests { get; set; }

    }
}
