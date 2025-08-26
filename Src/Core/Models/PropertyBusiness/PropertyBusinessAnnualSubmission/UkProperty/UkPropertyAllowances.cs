namespace Core.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkProperty
{
    /// <summary>
    /// Object holding UK Non FHL Property Allowances.
    /// </summary>
    public class UkPropertyAllowances
    {
        /// <summary>
        /// The amount claimed on equipment bought (except cars) up to maximum annual amount.
        /// Must be between 0 and 99999999999.99 up to 2 decimal places.
        /// </summary>
        public decimal? AnnualInvestmentAllowance { get; init; }

        /// <summary>
        /// The amount of zero emissions goods vehicle allowance for goods vehicles purchased for business use.
        /// Must be between 0 and 99999999999.99 up to 2 decimal places.
        /// </summary>
        public decimal? ZeroEmissionsGoodsVehicleAllowance { get; init; }

        /// <summary>
        /// Allowance for renovation or conversion of derelict business properties.
        /// Must be between 0 and 99999999999.99 up to 2 decimal places.
        /// </summary>
        public decimal? BusinessPremisesRenovationAllowance { get; init; }

        /// <summary>
        /// All other capital allowances.
        /// Must be between 0 and 99999999999.99 up to 2 decimal places.
        /// </summary>
        public decimal? OtherCapitalAllowance { get; init; }

        /// <summary>
        /// Cost of Replacing Domestic Items - formerly Wear and Tear allowance.
        /// Must be between 0 and 99999999999.99 up to 2 decimal places.
        /// </summary>
        public decimal? CostOfReplacingDomesticItems { get; init; }

        /// <summary>
        /// The amount of tax exemption for individuals with income from land or property.
        /// Must be between 0 and 1000.00 up to 2 decimal places.
        /// </summary>
        public decimal? PropertyIncomeAllowance { get; init; }

        /// <summary>
        /// The expenditure incurred on electric charge-point equipment.
        /// Must be between 0 and 99999999999.99 up to 2 decimal places.
        /// </summary>
        public decimal? ElectricChargePointAllowance { get; init; }

        /// <summary>
        /// Details about structured building allowance.
        /// </summary>
        public List<UkPropertyStructuredBuildingAllowance>? StructuredBuildingAllowance { get; init; }

        /// <summary>
        /// Details about enhanced structured building allowance.
        /// </summary>
        public List<UkPropertyEnhancedStructuredBuildingAllowance>? EnhancedStructuredBuildingAllowance { get; init; }

        /// <summary>
        /// The amount of zero emissions car allowance.
        /// Must be between 0 and 99999999999.99 up to 2 decimal places.
        /// </summary>
        public decimal? ZeroEmissionsCarAllowance { get; init; }
    }
}
