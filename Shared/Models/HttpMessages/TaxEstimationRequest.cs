using System.ComponentModel;

namespace Shared.Models.HttpMessages
{
    public class TaxEstimationRequest
    {
        public int? TaxYearEnding { get; set; }
        
        [DefaultValue("england")]
        public string Region { get; set; } = "england";
        public Incomes.Incomes Incomes { get; set; } = new Incomes.Incomes();
        public Reliefs.Reliefs Reliefs { get; set; } = new Reliefs.Reliefs();

    }
    
}