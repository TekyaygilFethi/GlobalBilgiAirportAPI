using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Data.Base
{
    public class ResponseObject<T>
    {
        public string Message { get; set; } = "Success";

        public T Data { get; set; }
    }
}
