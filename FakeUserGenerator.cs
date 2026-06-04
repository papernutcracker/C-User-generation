using System;
using Serilog;

namespace FakeUserApp
{
    public class FakeUserGenerator
    {
        private readonly Random _random = new Random();

        private readonly string[] _firstNames = { "John", "Mary", "James", "Patricia", "Robert", "Jennifer", "Michael", "Linda", "William", "Elizabeth" };
        private readonly string[] _lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Garcia", "Rodriguez", "Wilson" };
        private readonly string[] _cities = { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia", "San Antonio" };
        private readonly string[] _streets = { "Broadway", "Main Street", "First Avenue", "Oak Street", "Pine Road", "Maple Drive", "Cedar Lane" };

        public User Generate()
        {
            Log.Debug("Starting generation of a new fake user...");

            var firstName = _firstNames[_random.Next(_firstNames.Length)];
            var lastName = _lastNames[_random.Next(_lastNames.Length)];

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = $"+1 (555) {_random.Next(100, 999)}-{_random.Next(1000, 9999)}",
                Address = $"{_random.Next(1, 999)} {_streets[_random.Next(_streets.Length)]}, {_cities[_random.Next(_cities.Length)]}"
            };

            user.Email = $"{firstName.ToLower()}.{lastName.ToLower()}{_random.Next(10, 99)}@example.com";

            Log.Information("Successfully generated user: {FirstName} {LastName}", user.FirstName, user.LastName);
            return user;
        }
    }
}
