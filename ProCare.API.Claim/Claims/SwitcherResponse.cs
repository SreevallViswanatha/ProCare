using System;

namespace ProCare.API.Claim.Claims
{
    public class SwitcherResponse
    {
        public string Value { get; set; }
        public DateTime Timestamp { get; set; }

        public SwitcherResponse(string value, DateTime timestamp)
        {
            Value = value;
            Timestamp = timestamp;
        }
    }
}
