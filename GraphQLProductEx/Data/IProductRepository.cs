using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLProductEx.Models.Domain;

namespace GraphQLProductEx.Data
{
    public interface IProductRepository
    {
        Task<ProductMain> GetProductMainAsync(int productId, int storeId);
        Task<IEnumerable<ProductImage>> GetProductImageAsync(int productId);
        Task<IEnumerable<ProductCategory>> GetProductCategoryAsync(int productId, int storeId);
        Task<IEnumerable<ProductAttribute>> GetProductAttributeAsync(int productId);
        Task<IEnumerable<ProductVariant>> GetProductVariantAsync(int productId);
        Task<IEnumerable<string>> GetProductTagAsync(int productId);
        Task<IEnumerable<ProductOtherColor>> GetProductOtherColorAsync(int productId, int colorSwatchAttributeId, int colorAggregationAttributeId, int storeId);
        Task<ProductSeller> GetProductSellerAsync(int productId);
        Task<int> GetHealthCheckAsync();
        Task<int> GetAttributeIdFromCodeAsync(string code);
        Task<IEnumerable<ProductBreadcrumb>> GetProductBreadcrumbAsync(int productId, int storeId);
        Task<int?> GetRedirectTo3DAsync(int productId, int storeId);
        Task<int> RefreshProductUpdateTimeAsync(List<int> productIds);
        Task UpdateProductScoresAsync(IEnumerable<ProductScoreModel> productScores);
        Task<IEnumerable<ProductOtherColor>> GetProductOtherColorImageAsync(int productId, int colorAggregationAttributeId, int storeId);
        Task<IEnumerable<ProductMediaSize>> GetProductMediaSizes();
        Task<IEnumerable<ProductMediaSize>> GetCachedProductMediaSizes();
    }
}