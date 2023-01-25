namespace GraphQLProductEx.Models.Domain
{
    public class ProductSeller
    {
        public int SellerId { get; set; }
        public string SellerCode { get; set; }
        public string SellerName { get; set; }
        public bool? IsGiftWrapAvailable { get; set; }
        public string RichTextDescription { get; set; }
        public string SellerTitle { get; set; }
        public string City { get; set; }
        public string MersisNo { get; set; }
        public string KepAddress { get; set; }
        public bool IsShowRichTextDescToApp { get; set; }
        public int? DaysToShip { get; set; }
        public string TaxNo { get; set; }
    }
}
