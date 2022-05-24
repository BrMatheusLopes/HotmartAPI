using HotmartAPI.Data.Entities;
using System.Collections.Generic;

namespace HotmartAPI.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order GetOrderByEmailAndCode(string email, string code);
        Order GetOrderByEmailAndTransaction(string email, string transaction);
        Order Create(Order order);
        Order Update(Order order);
    }
}
