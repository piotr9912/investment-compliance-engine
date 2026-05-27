namespace FinTrack.Compliance.Domain.Entities;

using FinTrack.Compliance.Domain.ValueObjects;

public class Portfolio
{
    public string Id { get; }
    public string Name { get; }
    private readonly List<Position> _positions = new();
    public IReadOnlyCollection<Position> Positions => _positions.AsReadOnly();

    public Portfolio(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public void AddPosition(string assetId, decimal quantity, Money marketPrice)
    {
        var existing = _positions.FirstOrDefault(p => p.AssetId == assetId);
        if (existing != null) return;
        
        _positions.Add(new Position(assetId, quantity, marketPrice));
    }

    public decimal GetTotalValue()
    {
        return _positions.Sum(p => p.GetValue().Amount);
    }

    public decimal GetAssetValue(string assetId)
    {
        return _positions.Where(p => p.AssetId == assetId).Sum(p => p.GetValue().Amount);
    }
}

public class Position
{
    public string AssetId { get; }
    public decimal Quantity { get; }
    public Money MarketPrice { get; }

    public Position(string assetId, decimal quantity, Money marketPrice)
    {
        AssetId = assetId;
        Quantity = quantity;
        MarketPrice = marketPrice;
    }

    public Money GetValue() => new(Quantity * MarketPrice.Amount, MarketPrice.Currency);
}
