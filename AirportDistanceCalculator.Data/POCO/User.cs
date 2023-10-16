using AirportDistanceCalculator.Data.POCO.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Data.POCO
{
    [Table("Users")]
    public class User:PrimaryKeyEntity<Guid>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
