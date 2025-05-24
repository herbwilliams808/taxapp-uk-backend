using System.ComponentModel;
using Shared.Models.IndividualsReliefs.ForeignReliefs;
using Shared.Models.Incomes;

namespace Shared.Models.HttpMessages
{
    public class TaxEstimationRequest
    {
        public int? TaxYearEnding { get; set; }
        
        [DefaultValue("england")]
        public string Region => "england";

        public Incomes.Incomes Incomes { get; } = new();
        public IndividualsReliefs.IndividualsReliefs IndividualsReliefs { get; } = new();

        public ForeignReliefs ForeignReliefs { get; } = new();
        public OtherDeductions.OtherDeductions OtherDeductions { get; } = new();
        
        public IndividualsForeignIncome.IndividualsForeignIncome IndividualsForeignIncome { get; } = new();  
    }
    
}