using Microsoft.EntityFrameworkCore;
using RealEstateListingAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using System.Formats.Asn1;

namespace RealEstateListingAPI.Contexts
{
    public class comp584DbContext : DbContext
    {
        public comp584DbContext(DbContextOptions<comp584DbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }

    public static class DbSeeder
    {
        public static void SeedDatabase(comp584DbContext context)
        {
            if (!context.Users.Any())
            {
                var users = ReadUsersFromCsv("../RealEstateListingAPI/Data/mock_users.csv");
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            if (!context.Properties.Any())
            {
                var properties = ReadPropertiesFromCsv("../RealEstateListingAPI/Data/mock_properties.csv");
                context.Properties.AddRange(properties);
                context.SaveChanges();
            }

            if (!context.Appointments.Any())
            {
                var appointments = ReadAppointmentsFromCsv("../RealEstateListingAPI/Data/mock_appointments.csv");
                context.Appointments.AddRange(appointments);
                context.SaveChanges();
            }
        }

        private static IEnumerable<User> ReadUsersFromCsv(string filePath)
        {
            var users = new List<User>();

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });

            var records = csv.GetRecords<dynamic>(); // Use dynamic to access the CSV fields

            foreach (var record in records)
            {
                // Log each user for debugging
                Console.WriteLine($"Reading user: Name - {record.Name}, Email - {record.Email}, Role - {record.Role}");

                var user = new User
                {
                    Name = record.Name,
                    Email = record.Email,
                    PasswordHash = record.PasswordHash, // Assuming PasswordHash is a field in your CSV
                    Role = record.Role
                };

                // Log the status of the user object
                Console.WriteLine($"Created user object: Name - {user.Name}, Email - {user.Email}, Role - {user.Role}");

                users.Add(user);
            }

            return users;
        }


        private static IEnumerable<Property> ReadPropertiesFromCsv(string filePath)
        {
            var properties = new List<Property>();

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            var records = csv.GetRecords<Property>();

            foreach (var record in records)
            {
                // Log each property for debugging
                Console.WriteLine($"Reading property: Title - {record.Title}, Description - {record.Description}, Price - {record.Price}");

                var property = new Property
                {
                    Title = record.Title,
                    Description = record.Description,
                    Price = record.Price,
                    Image = null // Set Image to null for now
                };

                // Log the status of the property object
                Console.WriteLine($"Created property object: Title - {property.Title}, Description - {property.Description}, Price - {property.Price}");

                properties.Add(property);
            }

            return properties;
        }




        private static IEnumerable<Appointment> ReadAppointmentsFromCsv(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });
            return csv.GetRecords<Appointment>().ToList();
        }

    }
}
