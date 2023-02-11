using Microsoft.ML.Data;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class D001_TaxiFareInput
{
    [LoadColumn(0)]
    public string VendorId;

    [LoadColumn(1)]
    public string RateCode;

    [LoadColumn(2)]
    public float PassengerCount;

    [LoadColumn(3)]
    public float TripTime;

    [LoadColumn(4)]
    public float TripDistance;

    [LoadColumn(5)]
    public string PaymentType;

    [LoadColumn(6)]
    public float FareAmount;
}