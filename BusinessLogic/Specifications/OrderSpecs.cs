using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications
{
    public static class OrderSpecs
    {
        public class ByUser : Specification<Order>
        {
            public ByUser(string userId)
            {
                Query.Where(x => x.UserId == userId);
            }
        }
    }
}
