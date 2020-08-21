using System;
namespace EBANX.Core.Dtos
{
    public class EventReqDto
    {
        public string Type { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal Amount { get; set; }
    }
}
