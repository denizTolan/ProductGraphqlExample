using GraphQL.Types;
using GraphQLProductEx.Models.Domain;

namespace GraphQLProductEx.Types
{
    public class ProductImageType: ObjectGraphType<ProductImage>
    {
        public ProductImageType()
        {
            Field(p=>p.CdnPath);
            Field(p=>p.DisplayOrder);
            Field(p=>p.SizeCode);
        }
    }
}