using System;

namespace GraphQLProductEx.Models.Domain
{
    public class ProductMain
    {
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public string FriendlyUri { get; set; }
        public int VatRate { get; set; }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string ExternalSystemCode { get; set; }
        public bool IsDiscountForbidden { get; set; }
        public bool IsReturnForbidden { get; set; }
        public bool IsUseInfoInSeo { get; set; }
        public bool IsGiftCard { get; set; }
        public bool IsVatIncluded { get; set; }
        public bool IsActive { get; set; }
        public decimal Price1 { get; set; }
        public decimal? Price2 { get; set; }
        public DateTime? Price2StartTime { get; set; }
        public DateTime? Price2EndTime { get; set; }
        public decimal? Price3 { get; set; }
        public DateTime? Price3StartTime { get; set; }
        public DateTime? Price3EndTime { get; set; }
        public decimal? Price4 { get; set; }
        public DateTime? Price4StartTime { get; set; }
        public DateTime? Price4EndTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsPreOrder { get; set; }
        public DateTime? EstimatedSupplyDate { get; set; }
        public bool IsBanned { get; set; }
        public bool IsLoyaltyPointRewardForbidden { get; set; }
        public bool IsLoyaltyPointUsageForbidden { get; set; }
        public bool IsAvailableForClickAndCollect { get; set; }

        public ProductSeller ProductSeller { get; set; }
    }
}
