namespace GraphQLProductEx.Models.Domain
{
    public class ProductVariant
    {
        public int VariantId { get; set; }
        public string ExternalSystemCode { get; set; }
        public string Barcode { get; set; }
        public bool IsActive { get; set; }
    }
}
