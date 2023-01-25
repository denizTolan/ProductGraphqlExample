using GraphQL.Types;
using GraphQLProductEx.Models.Domain;

namespace GraphQLProductEx.Types
{
    public class ProductSellerType: ObjectGraphType<ProductSeller>
    {
        public ProductSellerType()
        {
            Field(p=>p.SellerId);
            Field(p=>p.SellerName);
            Field(p=>p.SellerCode);
            Field(p=>p.SellerTitle);
        }
    }
}