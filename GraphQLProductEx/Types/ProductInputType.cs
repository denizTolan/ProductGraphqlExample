using GraphQL.Types;

namespace GraphQLProductEx.Types
{
    public class ProductInputType : InputObjectGraphType
    {
        public ProductInputType()
        {
            Field<IntGraphType>("id");
            Field<StringGraphType>("storeid");
        }
    }
}