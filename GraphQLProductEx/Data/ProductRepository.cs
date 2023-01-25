using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using GraphQLProductEx.Models.Domain;
using GraphQLProductEx.Settings;

namespace GraphQLProductEx.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ConnectionStrings _connectionStrings;

        public ProductRepository(ConnectionStrings connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        private SqlConnection Connection => new SqlConnection(_connectionStrings.Ecom);

        public async Task<ProductMain> GetProductMainAsync(int productId, int storeId)
        {
            var sqlQuery = @"SELECT P.ID AS ProductId
                            , PSD.StoreID AS StoreId
                            ,P.DisplayName
                            ,P.Description
                            ,P.DisplayOrder
                            ,P.FriendlyURI AS FriendlyUri
                            ,P.VatRate
                            ,P.PageTitle
                            ,P.MetaDescription
                            ,P.MetaKeywords
                            ,P.ExternalSystemCode
                            ,P.IsDiscountForbidden
                            ,P.IsReturnForbidden
                            ,P.IsUseInfoInSeo
                            ,P.IsGiftCard
                            ,S.IsVatIncluded
                            ,P.IsActive
                            ,PSD.Price1
                            ,PSD.Price2
                            ,PSD.Price2_StartTime AS Price2StartTime
                            ,PSD.Price2_EndTime AS Price2EndTime
                            ,PSD.Price3
                            ,PSD.Price3_StartTime AS Price3StartTime
                            ,PSD.Price3_EndTime AS Price3EndTime
                            ,PSD.Price4
                            ,PSD.Price4_StartTime AS Price4StartTime
                            ,PSD.Price4_EndTime AS Price4EndTime
                            ,P.UpdateTime
                            ,P.IsPreOrder
                            ,P.EstimatedSupplyDate
                            ,P.IsLoyaltyPointRewardForbidden
                            ,P.IsLoyaltyPointUsageForbidden
                            ,P.IsAvailableForClickAndCollect
                            ,CASE WHEN EXISTS 
                            (SELECT 1 FROM PRD.ProductBanRelation AS PBR WHERE PBR.ProductId = P.ID AND (PBR.StoreId = @storeId OR PBR.StoreId = 0)) 
                            THEN 1 ELSE 0 END AS IsBanned 
                            FROM PRD.[Product] P
                            INNER JOIN PRD.[ProductStoreData] AS PSD ON P.ID = PSD.[ProductID]
                            INNER JOIN LK.[Store] AS S ON PSD.StoreID = S.ID
                            WHERE P.ID = @id
                            AND (PSD.StoreID = @storeId
                            OR PSD.StoreID = 0)";

            using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<ProductMain>(sqlQuery, new { id = productId, storeId });
            }
        }

        public async Task<IEnumerable<ProductImage>> GetProductImageAsync(int productId)
        {
            var sqlQuery = @"SELECT
                                    M.DisplayOrder
                                    ,'original' AS SizeCode
                                    ,PMF.CDNPath AS CdnPath
                                FROM PRD.ProductMedia M
                                INNER JOIN PRD.ProductMediaFile PMF
                                    ON PMF.ProductMediaID = M.ID
                                WHERE M.IsActive = 1
                                AND M.ProductID = @id
                                AND PMF.SizeID is null
                                AND M.ID =
                                    (
                                        select
                                               MIN(minM.ID)
                                        from PRD.ProductMedia minM with(nolock)
                                        where minM.ProductID = m.ProductID
                                            and minM.DisplayOrder = m.DisplayOrder
                                    )
                                ORDER BY M.DisplayOrder ASC";

            using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.QueryAsync<ProductImage>(sqlQuery, new { id = productId });
            }
        }

        public async Task<IEnumerable<ProductCategory>> GetProductCategoryAsync(int productId, int storeId)
        {

            var sqlQuery = @"SELECT
                        C.ID as CategoryId
                       ,C.ParentCategoryID as ParentCategoryId
                       ,C.Code
                       ,C.DisplayName
                       ,C.FriendlyURI as FriendlyUri
                       ,C.IsBrandCategory
                       ,C.DisplayOrder
                       ,C.IsMainCategory
                       ,C.IsVisible
                       ,AO.AttributeOptionCode AS GenderAttributeOptionCode
                    FROM PRD.Category_Product AS CP
                    INNER JOIN PRD.Category AS C
                        ON C.ID = CP.CategoryID
                    LEFT JOIN LK.AttributeOption AS AO
                        ON C.GenderAttributeOptionId = AO.ID        
                    WHERE C.IsActive = 1                     
                    AND CP.IsActive = 1
                    AND CP.ProductID = @id
                    AND (C.StoreID = @storeID
                    OR C.StoreID = 0)";

            using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.QueryAsync<ProductCategory>(sqlQuery, new { id = productId, storeID = storeId });
            }
        }

        public async Task<IEnumerable<ProductAttribute>> GetProductAttributeAsync(int productId)
        {

            var sqlQuery = @"SELECT
                        PA.ID AS ProductAttributeId
                       ,PA.VariantID as VariantId
                       ,A.ID AS AttributeId
                       ,AO.ID AS AttributeOptionId
                       ,AO.AttributeOptionCode
                       ,AO.Style
                       ,PA.[CustomValueText] AS CustomValueText
                       ,A.CodeName
                       ,A.DisplayName
                       ,A.DisplayOrder
                       ,A.IsVariant
                       ,A.IsVisibleToCustomer
                       ,AO.ValueText
                       ,AO.Description AS OptionDescription
                       ,AO.DisplayOrder AS OptionDisplayOrder
                       ,AO.AttributeLogo
                    FROM PRD.[ProductAttribute] PA
                    INNER JOIN LK.[Atribute] A
                        ON A.ID = PA.AttributeID
                    LEFT JOIN LK.[AttributeOption] AO
                        ON AO.ID = PA.[AttributeOptionID]
                    WHERE PA.ProductID = @id
                    AND A.IsActive = 1
                    AND (AO.ID IS NULL OR AO.IsActive = 1)";

            using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.QueryAsync<ProductAttribute>(sqlQuery, new { id = productId });
            }
        }

        public async Task<IEnumerable<ProductVariant>> GetProductVariantAsync(int productId)
        {
            var sqlQuery = @"SELECT
                        ID as VariantId
                       ,ExternalSystemCode
                       ,Barcode
                       ,IsActive
                    FROM PRD.Variant
                    WHERE ProductID = @id
                    AND IsActive = 1";

            using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.QueryAsync<ProductVariant>(sqlQuery, new { id = productId });
            }
        }

        public async Task<IEnumerable<string>> GetProductTagAsync(int productId)
        {
            var sqlQuery = @"SELECT
                        Tag
                    FROM PRD.ProductTag
                    WHERE ProductID = @id";

            using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.QueryAsync<string>(sqlQuery, new { id = productId });
            }
        }

        public async Task<IEnumerable<ProductOtherColor>> GetProductOtherColorAsync(int productId, int colorSwatchAttributeId, int colorAggregationAttributeId, int storeId)
        {
            var sqlQuery = @";WITH products as ( 
                            SELECT rp.RelatedProductID as ProductId FROM PRD.RelatedProduct as rp 
                            Where rp.ProductID = @id AND rp.RelationType = 'O' 
                            UNION ALL 
                            SELECT @id as ProductId
                            UNION ALL
                            SELECT pa.ProductID FROM PRD.ProductAttribute as pa
                            Where pa.AttributeID = @colorAggregationAttrId
                            AND pa.CustomValueTextHashed = (SELECT pam.CustomValueTextHashed from PRD.ProductAttribute as pam Where pam.ProductID = @id AND pam.AttributeID = @colorAggregationAttrId)),
                            RequiredProduct
                            AS (SELECT DISTINCT P.ID AS ProductID, P.FriendlyURI FROM PRD.Product AS P 
                            INNER JOIN products AS PS ON PS.ProductId = P.ID
                            LEFT JOIN PRD.ProductBanRelation as PBR ON PBR.ProductId = PS.ProductId AND (PBR.StoreId = @storeId OR PBR.StoreId = 0)
                            WHERE P.IsActive = 1 AND P.IsImageResized = 1 AND PBR.ProductId IS NULL)
                            SELECT RP.ProductID, RP.FriendlyURI, ao.ValueText as ColorName, ao.AttributeLogo, pa2.CustomValueText as ColorSwatchAttributeImagePath
                            FROM RequiredProduct AS RP 
                            INNER JOIN PRD.ProductAttribute as pa 
                            ON pa.ProductID = RP.ProductID AND pa.AttributeID = 1
                            INNER JOIN LK.AttributeOption as ao 
                            ON ao.ID = pa.AttributeOptionID 
                            LEFT JOIN PRD.ProductAttribute as pa2
                            ON pa2.ProductID = RP.ProductID AND pa2.AttributeID = @attributeId
                            ORDER BY ao.AttributeLogo, RP.ProductID ASC";

            using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.QueryAsync<ProductOtherColor>(sqlQuery, new
                {
                    id = productId,
                    attributeId = colorSwatchAttributeId,
                    colorAggregationAttrId = colorAggregationAttributeId,
                    storeId = storeId
                });
            }
        }

        public async Task<IEnumerable<ProductOtherColor>> GetProductOtherColorImageAsync(int productId, int colorAggregationAttributeId, int storeId)
        {
            var sqlQuery = @";WITH products as ( 
                            SELECT rp.RelatedProductID as ProductId FROM PRD.RelatedProduct as rp 
                            Where rp.ProductID = @id AND rp.RelationType = 'O' 
                            UNION ALL 
                            SELECT @id as ProductId
                            UNION ALL
                            SELECT pa.ProductID FROM PRD.ProductAttribute as pa
                            Where pa.AttributeID = @colorAggregationAttrId
                            AND pa.CustomValueTextHashed = (SELECT pam.CustomValueTextHashed from PRD.ProductAttribute as pam Where pam.ProductID = @id AND pam.AttributeID = @colorAggregationAttrId)),
                            RequiredProduct
                            AS (SELECT DISTINCT P.ID AS ProductID, P.FriendlyURI FROM PRD.Product AS P 
                            INNER JOIN products AS PS ON PS.ProductId = P.ID
                            LEFT JOIN PRD.ProductBanRelation as PBR ON PBR.ProductId = PS.ProductId AND (PBR.StoreId = @storeId OR PBR.StoreId = 0)
                            WHERE P.IsActive = 1 AND P.IsImageResized = 1 AND PBR.ProductId IS NULL)
                            SELECT RP.ProductID, RP.FriendlyURI, ao.ValueText as ColorName, ao.AttributeLogo, 
                            (SELECT TOP 1 CDNPath FROM [PRD].[vBO_ProductMediaFile] productMedia WHERE  productMedia.ProductID = RP.ProductID and productMedia.Code is null order by productMedia.ProductMediaDisplayOrder asc ) as ProductImagePath
                            FROM RequiredProduct AS RP 
                            INNER JOIN PRD.ProductAttribute as pa 
                            ON pa.ProductID = RP.ProductID AND pa.AttributeID = 1
                            INNER JOIN LK.AttributeOption as ao 
                            ON ao.ID = pa.AttributeOptionID 
                            ORDER BY ao.AttributeLogo, RP.ProductID ASC";

            using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.QueryAsync<ProductOtherColor>(sqlQuery, new
                {
                    id = productId,
                    colorAggregationAttrId = colorAggregationAttributeId,
                    storeId = storeId
                });
            }
        }

        public async Task<ProductSeller> GetProductSellerAsync(int productId)
        {
            var sqlQuery = @"SELECT 
	                        S.SellerId, 
	                        S.SellerCode, 
	                        S.SellerName,
                            SP.RichTextDescription,
                            SP.IsGiftWrapAvailable,
                            S.SellerTitle,
                            S.City,
                            S.MersisNo,
                            S.KepAddress,
                            SP.IsShowRichTextDescToApp,
                            SP.DaysToShip,
							S.TaxNo
                        FROM MP.SellerProduct SP
                        INNER JOIN MP.Seller S 
                            ON S.SellerId = SP.SellerId
                        WHERE S.IsActive = 1 
                            AND S.IsDeleted = 0 
                            AND SP.IsActive = 1 
                            AND SP.IsDeleted = 0 
                            AND SP.ProductId = @id";

            return await Connection.QueryFirstOrDefaultAsync<ProductSeller>(sqlQuery, new { id = productId });
        }

        public async Task<int> GetHealthCheckAsync()
        {
            await using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.QueryFirstAsync<int>("Select 1");
            }
        }

        public async Task<int> GetAttributeIdFromCodeAsync(string code)
        {
            using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.QueryFirstAsync<int>("SELECT ID from LK.Atribute Where CodeName = @codeName", new { codeName = code });
            }
        }

        public async Task<IEnumerable<ProductBreadcrumb>> GetProductBreadcrumbAsync(int productId, int storeId)
        {
            var sqlQuery = @"DECLARE @categoryId INT
                        SELECT TOP 1
                            @categoryId = ID
                        FROM PRD.Category
                        WHERE ID IN (SELECT
                                CategoryID
                            FROM PRD.Category_Product
                            WHERE ProductID = @productId
                            AND IsActive = 1)
                        AND IsMainCategory = 1
                        AND (StoreID = @storeId
                        OR StoreID = 0)
                        AND IsActive = 1
                        ORDER BY ID ASC
                        
                        ;
                        WITH RCTE
                        AS
                        (SELECT
                                C.ID as CategoryId
                               ,C.ParentCategoryID
                               ,C.DisplayName
                               ,C.FriendlyURI
                               ,C.GenderAttributeOptionId
                               ,1 AS Level
                            FROM PRD.Category AS C
                            WHERE C.ID = @categoryId
                        
                            UNION ALL
                        
                            SELECT
                                rh.ID as CategoryId
                               ,rh.ParentCategoryID
                               ,rh.DisplayName
                               ,rh.FriendlyURI
                               ,rh.GenderAttributeOptionId
                               ,rc.Level + 1 AS Level
                            FROM PRD.Category rh
                            INNER JOIN RCTE rc
                                ON rh.ID = rc.ParentCategoryID)
                        SELECT
                            R.CategoryId, R.ParentCategoryID, R.DisplayName, R.FriendlyURI, AO.AttributeOptionCode AS GenderAttributeOptionCode, R.[Level]
                        FROM RCTE AS R
                        LEFT JOIN LK.AttributeOption AS AO
                                ON R.GenderAttributeOptionId = AO.ID
                        ORDER BY Level DESC";

            using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.QueryAsync<ProductBreadcrumb>(sqlQuery, new { productId, storeId });
            }
        }

        /// <summary>
        /// Marka yada ürün çeşidine göre ürünün 3D yönlendirilmesinin istenip istenmediğini kontrol eder.
        /// Attribute ID'lar system kurulduğu andan itibaren aynı olduğu için parametre olarak alınmadı
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public async Task<int?> GetRedirectTo3DAsync(int productId, int storeId)
        {
            var sqlQuery = @";WITH attributeOptions
                            AS
                            (SELECT
                            		ao.AttributeOptionCode
                            	FROM PRD.ProductAttribute AS pa
                            	INNER JOIN LK.AttributeOption AS ao
                            		ON pa.AttributeOptionID = ao.ID
                            	WHERE pa.ProductID = @productId
                            	AND pa.AttributeID IN (3, 4))
                            
                            SELECT TOP 1
                            	ID
                            FROM PRD.RedirectTo3D AS r
                            INNER JOIN attributeOptions AS ao
                            	ON r.ProductTypeCode = ao.AttributeOptionCode
                            		OR r.BrandCode = ao.AttributeOptionCode
                            WHERE r.StoreID = @storeId";

            using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<int?>(sqlQuery, new { productId, storeId });
            }
        }

        public async Task<int> RefreshProductUpdateTimeAsync(List<int> productIds)
        {
            var sqlQuery = @"UPDATE PRD.Product SET UpdateTime = GETDATE(), UpdateUserID = 0 WHERE ID IN @productIds";

            await using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.ExecuteAsync(sqlQuery, new { productIds });
            }
        }

        public async Task UpdateProductScoresAsync(IEnumerable<ProductScoreModel> productScores)
        {
            string query = "UPDATE [PRD].[Product] SET Score = @score, UpdateTime = GETDATE(), UpdateUserID = 0 WHERE ID = @productId";

            await using (var sqlConnection = this.Connection)
            {
                await sqlConnection.ExecuteAsync(query, productScores);
            }
        }

        public async Task<IEnumerable<ProductMediaSize>> GetProductMediaSizes()
        {
            var mediaSizeQuery = @"select ID,
                                           Code,
                                           Width,
                                           Height
                                    from LK.MediaSize
                                    where IsActive = 1";
            
            using (var sqlConnection = this.Connection)
            {
                return await sqlConnection.QueryAsync<ProductMediaSize>(mediaSizeQuery);
            }
        }
        
        public async Task<IEnumerable<ProductMediaSize>> GetCachedProductMediaSizes()
        {
            return await GetProductMediaSizes();
        }
    }
}