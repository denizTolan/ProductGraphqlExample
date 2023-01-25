using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQLProductEx.Data;
using GraphQLProductEx.Types;

namespace GraphQLProductEx.Query
{
    public class ProductQuery:ObjectGraphType
    {
        public ProductQuery(IProductRepository productRepository)
        {
            Field<ProductType>("product", arguments: new QueryArguments(
            new QueryArgument<IntGraphType> { Name = "id"},
                        new QueryArgument<IntGraphType> { Name = "storeid"}),
                
                resolve: context => 
                { 
                    return Task.Run(async ()=> await productRepository.GetProductMainAsync(context.GetArgument<int>("id"),context.GetArgument<int>("storeid"))).Result; 
                });
        }
    }
}