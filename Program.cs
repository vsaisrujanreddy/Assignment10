using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace In_class_10_Entity_Framework
{
    class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public List<SaleDetail> ListOfProducts { get; set; }
    }



    class Product
    {
        public int ProductId { get; set; }
        public string ProdName { get; set; }
        public int ProdPrice { get; set; }
        public List<SaleDetail> OrderList { get; set; }
    }



    class SaleDetail
    {
        public int id { get; set; }
        public Order SaleOrderDet { get; set; }
        public Product SaleProductDet { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }

    class SaleDetailsContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Assignment10;Trusted_Connection=True;MultipleActiveResultSets=true";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }




    class Program
    {
        static void Main(string[] args)
        {
            using (SaleDetailsContext context = new SaleDetailsContext())
            {
                context.Database.EnsureCreated();



                Order order1 = new Order { CustomerName = "Jane" };
                Order order2 = new Order { CustomerName = "Sarah" };
                Order order3 = new Order { CustomerName = "Doe" };
                Product product1 = new Product
                {
                    ProdName = "Glucose",
                    ProdPrice = 95,
                };
                Product product2 = new Product
                {
                    ProdName = "Paracetemol",
                    ProdPrice = 45,
                };
                Product product3 = new Product
                {
                    ProdName = "Mlyrii",
                    ProdPrice = 38,
                };



                SaleDetail SaleDetail1 = new SaleDetail
                {
                    SaleOrderDet = order1,
                    SaleProductDet = product1,
                    Quantity = 4,
                    OrderDate = DateTime.Now
                };
                SaleDetail SaleDetail2 = new SaleDetail
                {
                    SaleOrderDet = order2,
                    SaleProductDet = product1,
                    Quantity = 7,
                    OrderDate = DateTime.Now
                };
                SaleDetail SaleDetail3 = new SaleDetail
                {
                    SaleOrderDet = order3,
                    SaleProductDet = product2,
                    Quantity = 9,
                    OrderDate = DateTime.Now
                };
                context.Orders.Add(order1);
                context.Orders.Add(order2);
                context.Orders.Add(order3);
                context.Products.Add(product1);
                context.Products.Add(product2);
                context.Products.Add(product3);
                context.SaleDetails.Add(SaleDetail1);
                context.SaleDetails.Add(SaleDetail2);
                context.SaleDetails.Add(SaleDetail3);



                context.SaveChanges();



                //Getting all the orders where a particular product is sold
                IQueryable<SaleDetail> SaleProductOrders = context.SaleDetails
                    .Include(c => c.SaleProductDet).Where(c => c.SaleProductDet == product1);


                //This Gives a list of all orders where the selected product is sold
                List<SaleDetail> SelectedSaleProductOrders = SaleProductOrders.ToList();


                //Orders where a given product sold maximum
                SaleDetail maxsaleproducts = context.SaleDetails
                  .Include(c => c.SaleProductDet).Where(c => c.SaleProductDet == product1).OrderByDescending(x => x.Quantity).FirstOrDefault();




            }




        }
    }
}