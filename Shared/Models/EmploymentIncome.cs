namespace Shared.Models;

public class EmploymentIncome
{
        public string EmployerName { get; set; } = string.Empty;
        public decimal Income { get; set; }
        public decimal TaxPaid { get; set; }
}