using HotmartAPI.Data;
using HotmartAPI.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace HotmartAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RepositoryContext _context;

        public OrderRepository(RepositoryContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        public Order GetOrderByEmailAndCode(string email, string code)
        {
            return _context.Orders
                .Where(x => x.BuyerEmail.Equals(email) && x.SubscriberCode.Equals(code))
                .OrderByDescending(x => x.Id)
                .LastOrDefault();
        }

        public Order GetOrderByEmailAndTransaction(string email, string transaction)
        {
            return _context.Orders
                .Where(x => x.BuyerEmail.Equals(email) && x.Transaction.Equals(transaction))
                .OrderByDescending(x => x.Id)
                .LastOrDefault();
        }

        public Order Create(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }

        public Order Update(Order order)
        {
            var result = _context.Orders
                .Where(x => x.BuyerEmail.Equals(order.BuyerEmail) && x.SubscriberCode.Equals(order.SubscriberCode))
                .OrderByDescending(x => x.Id)
                .LastOrDefault();

            if (result == null)
                return null;

            _context.Entry(result).CurrentValues.SetValues(order);
            _context.SaveChanges();
            return order;
        }
    }
}
