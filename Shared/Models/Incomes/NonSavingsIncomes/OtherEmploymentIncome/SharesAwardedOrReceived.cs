using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.Incomes.NonSavingsIncomes.OtherEmploymentIncome
{
    /// <summary>
    /// Represents shares awarded or received as part of an employment income.
    /// </summary>
    public record SharesAwardedOrReceived(
        [property: Required]
        [property: SwaggerSchema("Name of the employer.")]
        string EmployerName,

        [property: SwaggerSchema("Reference of the employer, if available.")]
        string? EmployerRef,

        [property: Required]
        [property: SwaggerSchema("The type of scheme plan associated with the shares.")]
        SchemePlanType SchemePlanType,

        [property: Required]
        [property: SwaggerSchema("The date the shares ceased to be subject to the plan.")]
        DateTime DateSharesCeasedToBeSubjectToPlan,

        [property: Required]
        [property: SwaggerSchema("The number of share securities awarded.")]
        int NoOfShareSecuritiesAwarded,

        [property: Required]
        [property: SwaggerSchema("The class of shares awarded.")]
        string ClassOfShareAwarded,

        [property: Required]
        [property: SwaggerSchema("The date the shares were awarded.")]
        DateTime DateSharesAwarded,

        [property: Required]
        [property: SwaggerSchema("Indicates if the shares are subject to restrictions.")]
        bool SharesSubjectToRestrictions,

        [property: Required]
        [property: SwaggerSchema("Indicates if an election was entered to ignore restrictions.")]
        bool ElectionEnteredIgnoreRestrictions,

        [property: Required]
        [property: SwaggerSchema("The actual market value of the shares on the award date.")]
        decimal ActualMarketValueOfSharesOnAward,

        [property: Required]
        [property: SwaggerSchema("The unrestricted market value of the shares on the award date.")]
        decimal UnrestrictedMarketValueOfSharesOnAward,

        [property: Required]
        [property: SwaggerSchema("The amount paid for the shares on the award date.")]
        decimal AmountPaidForSharesOnAward,

        [property: Required]
        [property: SwaggerSchema("The market value of the shares after restrictions were lifted.")]
        decimal MarketValueAfterRestrictionsLifted,

        [property: Required]
        [property: SwaggerSchema("The taxable amount related to the shares.")]
        decimal TaxableAmount
    );
}
