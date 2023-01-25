namespace GraphQLProductEx.Models.Domain
{
    public class ProductBreadcrumb
    {
        public int CategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
        public string DisplayName { get; set; }
        public string FriendlyUri { get; set; }
        public int Level { get; set; }
        public string GenderAttributeOptionCode { get; set; }
    }
}
