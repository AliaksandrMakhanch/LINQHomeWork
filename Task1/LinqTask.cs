using System;
using System.Collections.Generic;
using System.Linq;
using Task1.DoNotChange;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(customer => customer.Orders.Sum(order => order.Total) > limit);
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            return customers.Select(c => (
                c,
                suppliers = suppliers.Where(supplier => supplier.City == c.City && supplier.Country == c.Country)
            ));
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(IEnumerable<Customer> customers, IEnumerable<Supplier> suppliers)
        {
            return from customer in customers
                   select (customer, suppliers.Where(supplier => supplier.City == customer.City && supplier.Country == customer.Country));
        }

        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
            return customers.Where(customer => customer.Orders.Any(order => order.Total > limit));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(IEnumerable<Customer> customers)
        {
            return customers.Where(customer => customer.Orders.Any())
                            .Select(customer => (customer, customer.Orders.Min(order => order.OrderDate)));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(IEnumerable<Customer> customers)
        {
            return customers.Where(customer => customer.Orders.Any())
                            .Select(customer => (customer, dateOfEntry: customer.Orders.Min(order => order.OrderDate)))
                            .OrderBy(item => item.dateOfEntry.Year)
                            .ThenBy(item => item.dateOfEntry.Month)
                            .ThenByDescending(item => item.customer.Orders.Sum(order => order.Total))
                            .ThenBy(item => item.customer.CompanyName);
        }

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            return customers.Where(customer =>
                !int.TryParse(customer.PostalCode, out _) ||
                string.IsNullOrWhiteSpace(customer.Region) ||
                !customer.Phone.Contains("("));
        }

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {
            return products.GroupBy(product => product.Category)
                           .Select(group => new Linq7CategoryGroup
                           {
                               Category = group.Key,
                               UnitsInStockGroup = group.GroupBy(product => product.UnitsInStock)
                                                        .Select(subGroup => new Linq7UnitsInStockGroup
                                                        {
                                                            UnitsInStock = subGroup.Key,
                                                            Prices = subGroup.OrderBy(product => product.UnitPrice)
                                                                              .Select(product => product.UnitPrice)
                                                        })
                           });
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
            IEnumerable<Product> products,
            decimal cheap,
            decimal middle,
            decimal expensive
        )
        {
            var groupedProducts = new[]
            {
                (category: cheap, products: products.Where(product => product.UnitPrice <= cheap)),
                (category: middle, products: products.Where(product => product.UnitPrice > cheap && product.UnitPrice <= middle)),
                (category: expensive, products: products.Where(product => product.UnitPrice > middle && product.UnitPrice <= expensive)),
            };

            return groupedProducts;
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(IEnumerable<Customer> customers)
        {
            return customers.GroupBy(customer => customer.City)
                            .Select(group =>
                            (
                                city: group.Key,
                                averageIncome: (int)group.Average(customer => customer.Orders.Sum(order => order.Total)),
                                averageIntensity: group.Sum(customer => customer.Orders.Count()) / group.Count()
                            ));
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            return string.Join("", suppliers.Select(supplier => supplier.Country)
                                            .Distinct()
                                            .OrderBy(country => country.Length)
                                            .ThenBy(country => country));
        }
    }
}