namespace GraphQLProductEx.Models.Domain
{
    public class ProductOtherColor
    {
        public int ProductId { get; set; }
        public string FriendlyUri { get; set; }
        public string ColorName { get; set; }
        public string AttributeLogo { get; set; }

        /// <summary>
        /// ProductViewType attribute ve swatch-dropdown-gosterim option değerine sahip ürünün renk url bilgisi
        /// </summary>
        public string ColorSwatchAttributeImagePath { get; set; }

        /// <summary>
        /// ProductViewType attribute ve standart-gosterim ya da bu ProductViewType attribute sahip olmayan option değerine sahip ürünün renk url bilgisi
        /// </summary>
        public string ProductImagePath { get; set; }
    }
}
