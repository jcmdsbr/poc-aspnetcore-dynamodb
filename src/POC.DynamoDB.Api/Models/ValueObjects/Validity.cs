using System;

namespace POC.DynamoDB.Api.Models.ValueObjects
{
    public class Validity
    {
        public Validity(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; }
        public DateTime End { get; }
    }
}