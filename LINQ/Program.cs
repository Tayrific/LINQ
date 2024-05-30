using System.Xml.Linq;

namespace LINQ
{

    public class Customer
    {
        public string CustomerID { get; set; }
        public string City { get; set; }

        public override string ToString()
        {
            return CustomerID + "\t" + City;
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            string likes = "I like fruit";

            string[] fruits = { "Orange", "Apple", "Grapefruit", "Pear", "Pineapple",
            "Grapes", "Peach","Melon","Coconut" };

            int[] numbers = { 5, 6, 3, 8, 2, 9, 1, 6, 15, 66, 34, 23, 32, 45, 29, 30 };
            var getNumbers = from number
                                in numbers
                                where number % 2 == 0
                                orderby number
                                select number;
            //Console.WriteLine(string.Join(", ", getNumbers));

            var fruitwithG = from fruit in fruits
                                where fruit.Contains("G") && (fruit.Length < 8)
                                select fruit;
            //Console.WriteLine(string.Join(", ", fruitwithG));
            //Console.ReadLine();

           //numQuery();

            XMLQuery();
            ObjectQuery();
        }

        static void numQuery()
        {
            var nums = new int[] { 1, 4, 9, 16, 25, 36 };
            var evenNumbers = from num in nums
                                where (num % 2) == 0
                                select num;

            Console.WriteLine("Results: ");
            foreach (var number in evenNumbers)
            {
                Console.WriteLine(number);
               
            }
            Console.ReadLine();
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
        
        static IEnumerable<Customer> CreateCustomer()
        {
            return
                from c in XDocument.Load("Customers.xml")
                .Descendants("Customers").Descendants()
                select new Customer
                {
                    City = c.Attribute("City").Value,
                    CustomerID = c.Attribute("CustomerID").Value
                };
        }

        static void ObjectQuery()
        {
            var results = from c in CreateCustomer()
                        where c.City == "London"
                        select c;

            foreach (var c in results)
            {
                Console.WriteLine(c);
            }
               
        }
    }    

}
