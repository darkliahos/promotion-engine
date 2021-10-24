namespace promotion_engine
{
    public enum PromotionType
    {
        None = 0,
        MultibuyOffer = 1,
        HaveCertainProductsInYourBasket = 2
    }

    public class GroupedCartItem
    {
        public string SKUReference { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
    }
}
