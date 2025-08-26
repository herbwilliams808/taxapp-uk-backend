using System.Text.Json.Serialization;

namespace Core.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome
{
    /// <summary>
    /// Enumeration of scheme plan types for share options.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SchemePlanType
    {
        /// <summary>
        /// Enterprise Management Incentive.
        /// </summary>
        EMI,

        /// <summary>
        /// Company Share Option Plan.
        /// </summary>
        CSOP,

        /// <summary>
        /// Save As You Earn.
        /// </summary>
        SAYE,

        /// <summary>
        /// Other types of scheme plans.
        /// </summary>
        Other
    }
}