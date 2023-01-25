using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQLProductEx.Data;
using GraphQLProductEx.Models;
using GraphQLProductEx.Models.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQLProductEx.Types
{
    public class ProductType: ObjectGraphType<ProductMain>
    {
        public ProductType()
        {
            Field(p=>p.ProductId);
            Field(p=>p.StoreId);
            Field(p=>p.Description);
            Field(p=>p.MetaDescription);
            Field(p=>p.ExternalSystemCode);
            Field(p=>p.DisplayOrder);
            Field(p=>p.FriendlyUri);
            Field(p=>p.IsActive);
            Field(p=>p.IsBanned);
            Field(p=>p.IsDiscountForbidden);
            Field(p=>p.MetaKeywords);
            Field(p=>p.IsUseInfoInSeo);
            Field(p=>p.IsGiftCard);
            Field(p=>p.IsPreOrder);
            Field(p=>p.IsVatIncluded);
            Field(p=>p.Price1);
            
            Field<ProductSellerType>("productseller",
                
                resolve: context => 
                { 
                    return Task.Run(async ()=> await context.RequestServices.GetRequiredService<IProductRepository>().GetProductSellerAsync(context.Source.ProductId)).Result; 
                });
            
            Field<ListGraphType<ProductImageType>>("productimages",
                
                resolve: context => 
                { 
                    return Task.Run(async ()=> await context.RequestServices.GetRequiredService<IProductRepository>().GetProductImageAsync(context.Source.ProductId)).Result; 
                });
        }
    }
}