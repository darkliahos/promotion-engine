namespace promotion_engine.Models
{
    public class PromotionApplication
    {
        public string SKUReference { get; set; }

        public int QuantityApplied { get; set; }

        public int PromoPrice { get; set; }

        public PromotionType Type { get; set; }
    }
}
