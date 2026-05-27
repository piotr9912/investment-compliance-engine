namespace FinTrack.Compliance.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty.");
            
        if (currency.Length != 3)
            throw new ArgumentException("Currency must be a 3-letter ISO code.");

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public static Money operator *(Money money, decimal multiplier)
        => new(money.Amount * multiplier, money.Currency);
}
