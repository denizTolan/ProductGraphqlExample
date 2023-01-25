namespace GraphQLProductEx.Models.Domain
{
    public class ProductCategory
    {
        public int CategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public string FriendlyUri { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsBrandCategory { get; set; }
        public bool IsMainCategory { get; set; }
        public bool IsVisible { get; set; }
        public string GenderAttributeOptionCode { get; set; }
    }
}
