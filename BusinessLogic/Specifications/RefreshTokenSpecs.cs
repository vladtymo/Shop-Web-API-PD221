using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications
{
    public static class RefreshTokenSpecs
    {
        public class ByToken : Specification<RefreshToken>
        {
            public ByToken(string value)
            {
                Query.Where(x => x.Token == value);
            }
        }
        public class ByDate : Specification<RefreshToken>
        {
            public ByDate(DateTime date)
            {
                Query.Where(x => x.CreationDate < date);
            }
        }
    }
}
