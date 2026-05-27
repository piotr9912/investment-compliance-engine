namespace FinTrack.Compliance.Domain.Rules;

using FinTrack.Compliance.Domain.Entities;

public class MaxConcentrationRule : IComplianceRule
{
    private readonly string _targetAssetId;
    private readonly decimal _maxPercentageAllowed;

    public MaxConcentrationRule(string id, string targetAssetId, decimal maxPercentageAllowed)
    {
        Id = id;
        _targetAssetId = targetAssetId;
        _maxPercentageAllowed = maxPercentageAllowed;
    }

    public string Id { get; }
    public string Description => $"Asset {_targetAssetId} cannot exceed {_maxPercentageAllowed * 100}% of portfolio.";

    public bool Evaluate(Portfolio portfolio, out string violationMessage)
    {
        violationMessage = string.Empty;
        var totalValue = portfolio.GetTotalValue();
        
        if (totalValue == 0) return true;

        var assetValue = portfolio.GetAssetValue(_targetAssetId);
        var currentPercentage = assetValue / totalValue;

        if (currentPercentage > _maxPercentageAllowed)
        {
            violationMessage = $"Rule {Id} Violated: Asset {_targetAssetId} is {currentPercentage:P2} of portfolio (Max: {_maxPercentageAllowed:P2}).";
            return false;
        }

        return true;
    }
}
