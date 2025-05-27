using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Shared.Models.IndividualsEmploymentIncomes.Employments;

public class Pay
{
    /// <summary>
    /// The gross pay for this employment.
    /// The value must be between 0 and 99999999999.99 up to 2 decimal places.
    /// </summary>
    /// <example>1024.99</example>
    [Range(0, 99999999999.99)]
    [XmlElement("taxablePayToDate")]
    public decimal? TaxablePayToDate { get; set; }

    /// <summary>
    /// The amount of tax deducted for this employment.
    /// The value must be between -99999999999.99 and 99999999999.99 up to 2 decimal places.
    /// </summary>
    /// <example>1024.99</example>
    [Range(-99999999999.99, 99999999999.99)]
    [XmlElement("totalTaxToDate")]
    public decimal? TotalTaxToDate { get; set; }

    /// <summary>
    /// The payment frequency.
    /// </summary>
    /// <example>fortnightly</example>
    [XmlElement("payFrequency")]
    public string? PayFrequency { get; set; }

    /// <summary>
    /// The date of the latest payment.
    /// Format: YYYY-MM-DD
    /// </summary>
    /// <example>2020-07-01</example>
    [XmlElement("paymentDate")]
    public string? PaymentDate { get; set; }

    /// <summary>
    /// The week count of the tax year. Integer between 1 and 56.
    /// </summary>
    /// <example>32</example>
    [Range(1, 56)]
    [XmlElement("taxWeekNo")]
    public int? TaxWeekNo { get; set; }

    /// <summary>
    /// The month count of the tax year. Integer between 1 and 12.
    /// </summary>
    /// <example>7</example>
    [Range(1, 12)]
    [XmlElement("taxMonthNo")]
    public int? TaxMonthNo { get; set; }
}