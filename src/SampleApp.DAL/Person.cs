using System.Collections.Generic;

namespace SampleApp.DAL
{
    public class Person
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public Address Address { get; set; }

        public IEnumerable<string> Colours { get; set; }
        public IEnumerable<string> Companies { get; set; }
    }
}