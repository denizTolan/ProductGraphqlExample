namespace GraphQLProductEx.Models.Domain
{
    public class ProductAttribute
    {
        public int ProductAttributeId { get; set; }
        public int? VariantId { get; set; }
        public int AttributeId { get; set; }
        public int? AttributeOptionId { get; set; }
        public string AttributeOptionCode { get; set; }
        public string Style { get; set; }
        public string CustomValueText { get; set; }
        public string CodeName { get; set; }
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVariant { get; set; }
        public bool IsVisibleToCustomer { get; set; }
        public string ValueText { get; set; }
        public string OptionDescription { get; set; }
        public int? OptionDisplayOrder { get; set; }
        public string AttributeLogo { get; set; }
    }
}
