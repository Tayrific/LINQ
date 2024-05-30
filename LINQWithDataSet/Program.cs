using System;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using System.Configuration;


namespace LINQWithDataSet
{
    [Table("Customers")]
    public class Customer
    {
        [Column]
        public string CustomerID { get; set; }
        [Column]
        public string City { get; set; }

        [Column]
        public string ContactName { get; set; }

        public ICollection<Order> Orders { get; set; }

        public override string ToString()
        {
            return CustomerID + "\t" + City;
        }
    }

    [Table("Orders")]
    public class Order
    {
        [Column]
        public int OrderID { get; set; } // Correct the property name to OrderID
        [Column]
        public string OrderDate { get; set; } // Correct the property name to OrderDate
        [Column]
        public string ShipCity { get; set; } // Correct the property name to ShipCity

        public override string ToString()
        {
            return OrderID + "\t" + OrderDate;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            ObjectQuery();
            //XMLQuery();
        }

        public static void XMLQuery()
        {
            var doc = XDocument.Load("Customers.xml");

            var results = from c in doc.Descendants("Customer")
                          where c.Attribute("City").Value == "London"
                          select c;

            XElement transformedResults =
                new XElement("Londoners",
                    from customer in results
                    select new XElement("Contact",
                        new XAttribute("ID", customer.Attribute("CustomerID").Value),
                        new XElement("Name", customer.Attribute("ContactName").Value),
                        new XElement("City", customer.Attribute("City").Value)));

            Console.WriteLine("Results:\n{0}", transformedResults);
            transformedResults.Save("Output.xml");
        }

        public class NorthwindContext : DbContext
        {
            public DbSet<Customer> Customers { get; set; }
            public DbSet<Order> Orderings { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["NORTHWIND"].ConnectionString);
            }

        }

        static void ObjectQuery()
        {
            var db = new NorthwindContext();
            var results = from c in db.Customers
                          from o in c.Orders
                          where c.City == "London"
                          select new { c.ContactName, o.OrderID }; // Use o.OrderID instead of o.orderID
            foreach (var c in results)
                Console.WriteLine("{0}\t{1}", c.ContactName, c.OrderID);
        }
    }
}
