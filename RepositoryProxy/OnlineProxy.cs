﻿using System.Collections.Generic;
using System.Linq;

using Model;

using RepositoryProxy.RepositoryService;

namespace RepositoryProxy
{
    public class OnlineProxy : IRepositoryProxy
    {
        public IEnumerable<Customer> GetCustomers()
        {
            IEnumerable<Customer> customers;

            using (var repositoryService = new RepositoryServiceClient())
                customers = repositoryService.GetCustomers();

            //Automatic change tracking is enabled by default when deserialising.
            //This is turned off to give the same behaviour as offline. Deletions have to be tracked manually anyway.
            foreach (var customer in customers) {
                customer.StopTracking();

                foreach (var order in customer.Orders)
                    order.StopTracking();
            }

            return customers;
        }

        public int SaveChanges(IEnumerable<Customer> customers, IEnumerable<Order> orders)
        {
            using (var repositoryService = new RepositoryServiceClient())
                return repositoryService.SaveChanges(customers.ToArray(), orders.ToArray());
        }

        public bool Online
        {
            get { return true; }
        }
    }
}