using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome
{
    /// <summary>
    /// Represents shares awarded or received as part of an employment income.
    /// </summary>
    public record SharesAwardedOrReceived(
        [Required]
        [property: SwaggerSchema("Name of the employer.")]
        string EmployerName,

        [property: SwaggerSchema("Reference of the employer, if available.")]
        string? EmployerRef,

        [Required]
        [property: SwaggerSchema("The type of scheme plan associated with the shares.")]
        SchemePlanType SchemePlanType,

        [Required]
        [property: SwaggerSchema("The date the shares ceased to be subject to the plan.")]
        DateTime DateSharesCeasedToBeSubjectToPlan,

        [Required]
        [property: SwaggerSchema("The number of share securities awarded.")]
        int NoOfShareSecuritiesAwarded,

        [Required]
        [property: SwaggerSchema("The class of shares awarded.")]
        string ClassOfShareAwarded,

        [Required]
        [property: SwaggerSchema("The date the shares were awarded.")]
        DateTime DateSharesAwarded,

        [Required]
        [property: SwaggerSchema("Indicates if the shares are subject to restrictions.")]
        bool SharesSubjectToRestrictions,

        [Required]
        [property: SwaggerSchema("Indicates if an election was entered to ignore restrictions.")]
        bool ElectionEnteredIgnoreRestrictions,

        [Required]
        [property: SwaggerSchema("The actual market value of the shares on the award date.")]
        decimal ActualMarketValueOfSharesOnAward,

        [Required]
        [property: SwaggerSchema("The unrestricted market value of the shares on the award date.")]
        decimal UnrestrictedMarketValueOfSharesOnAward,

        [Required]
        [property: SwaggerSchema("The amount paid for the shares on the award date.")]
        decimal AmountPaidForSharesOnAward,

        [Required]
        [property: SwaggerSchema("The market value of the shares after restrictions were lifted.")]
        decimal MarketValueAfterRestrictionsLifted,

        [Required]
        [property: SwaggerSchema("The taxable amount related to the shares.")]
        decimal TaxableAmount
    );
}
