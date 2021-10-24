using System.Collections.Generic;

namespace promotion_engine
{
    public class Discount
    {
        public IEnumerable<string> SKUReference { get; set; }

        public decimal Percentage { get; set; }

        public int QualifyingQuantity { get; set; }

        public PromotionType Type { get; set; }
    }
}
