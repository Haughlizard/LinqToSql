using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace LinqConsoleApp
{
    public class Northwind : DataContext
    {
        // Table<T> abstracts database details per table/data type.
        public Table<Customer> Customers;
        public Table<Order> Orders;
        public Northwind(string connection) : base(connection) { }
    }

    [Table(Name = "Customers")]
    public class Customer
    {
        private string _CustomerID;
        [Column(IsPrimaryKey = true, Storage = "_CustomerID")]
        public string CustomerID
        {
            get
            {
                return this._CustomerID;
            }
            set
            {
                this._CustomerID = value;
            }
        }
        private string _City;
        [Column(Storage = "_City")]
        public string City
        {
            get
            {
                return this._City;
            }
            set
            {
                this._City = value;
            }
        }

        private EntitySet<Order> _Orders;
        public Customer()
        {
            this._Orders = new EntitySet<Order>();
        }

        [Association(Storage = "_Orders", OtherKey = "CustomerID")]
        public EntitySet<Order> Orders
        {
            get { return this._Orders; }
            set { this._Orders.Assign(value); }
        }
    }

    [Table(Name = "Orders")]
    public class Order
    {
        private int _OrderID = 0;
        private string _CustomerID;
        private EntityRef<Customer> _Customer;
        public Order() { this._Customer = new EntityRef<Customer>(); }

        [Column(Storage = "_OrderID", DbType = "Int NOT NULL IDENTITY",
        IsPrimaryKey = true, IsDbGenerated = true)]
        public int OrderID
        {
            get { return this._OrderID; }
            // No need to specify a setter because IsDBGenerated is
            // true.
        }

        [Column(Storage = "_CustomerID", DbType = "NChar(5)")]
        public string CustomerID
        {
            get { return this._CustomerID; }
            set { this._CustomerID = value; }
        }

        [Association(Storage = "_Customer", ThisKey = "CustomerID")]
        public Customer Customer
        {
            get { return this._Customer.Entity; }
            set { this._Customer.Entity = value; }
        }
    }


    class Program
    {
        private const string CONNECT_STRING = @"Data Source=(localdb)\MSSQLLocalDB;
                                                Initial Catalog=Northwind;Integrated Security=True;
                                                Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;
                                                ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static void Main(string[] args)
        {
            Northwind db = new Northwind(CONNECT_STRING);
            //Table<Customer> Customers = db.GetTable<Customer>();

            //db.Log = Console.Out;
            // Query for customers in London.
            //IQueryable<Customer> custQuery =
            //from cust in Customers
            //where cust.City == "London"
            //select cust;

            //foreach (Customer cust in custQuery)
            //{
            //    Console.WriteLine("ID={0}, City={1}", cust.CustomerID,
            //    cust.City);
            //} 
            var custQuery =
                from cust in db.Customers
                where cust.Orders.Any()
                select cust;
            foreach (var custObj in custQuery)
            {
                Console.WriteLine("ID={0}, Qty={1}", custObj.CustomerID,
                custObj.Orders.Count);
            }
            // Prevent console window from closing.
            Console.ReadLine();
        }
    }
}
