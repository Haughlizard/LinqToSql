using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LinqDataManipulationApp
{
    class Program
    {
        private const string CONNECT_STRING = @"Data Source=(localdb)\MSSQLLocalDB;
                                                Initial Catalog=Northwind;Integrated Security=True;
                                                Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;
                                                ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static void Main(string[] args)
        {
            // 使用连接字符串连接数据库
            Northwnd db = new Northwnd(CONNECT_STRING);

            Customer newCust = new Customer();
            newCust.CompanyName = "AdventureWorks Cafe";
            newCust.CustomerID = "ADVCA";

            db.Customers.InsertOnSubmit(newCust);

            Console.WriteLine("\nCustomers matching CA before insert");
            foreach (var c in db.Customers.Where(cust => cust.CustomerID.Contains("CA")))
            {
                Console.WriteLine("{0}, {1}, {2}",
                c.CustomerID, c.CompanyName, c.Orders.Count);
            }

            var existingCust =
                (from c in db.Customers
                 where c.CustomerID == "ALFKI"
                 select c).First();
            existingCust.ContactName = "New Contact";

            Order order0 = existingCust.Orders[0];
            OrderDetail detail0 = order0.OrderDetails[0];

            // Display the order to be deleted.
            Console.WriteLine("The Order Detail to be deleted is: OrderID = {0}, ProductID = {1}",
            detail0.OrderID, detail0.ProductID);
            // Mark the Order Detail row for deletion from the database.
            db.OrderDetails.DeleteOnSubmit(detail0);

            db.SubmitChanges();

            Console.WriteLine("\nCustomers matching CA after update");
            foreach (var c in db.Customers.Where(cust =>
            cust.CustomerID.Contains("CA")))
            {
                Console.WriteLine("{0}, {1}, {2}",
                c.CustomerID, c.CompanyName, c.Orders.Count);
            }

            // Keep the console window open after activity stops.
            Console.ReadLine();
        }
    }
}
