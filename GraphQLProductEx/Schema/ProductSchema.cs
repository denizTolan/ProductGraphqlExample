using GraphQLProductEx.Query;

namespace GraphQLProductEx.Schema
{
    public class ProductSchema:GraphQL.Types.Schema
    {
        public ProductSchema(ProductQuery productQuery)
        {
            Query = productQuery;
        }
    }
}