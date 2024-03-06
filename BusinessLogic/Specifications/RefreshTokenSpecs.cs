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
    }
}
