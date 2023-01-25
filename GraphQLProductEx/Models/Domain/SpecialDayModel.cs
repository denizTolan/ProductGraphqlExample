using System;

namespace GraphQLProductEx.Models.Domain
{
    public class SpecialDayModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}