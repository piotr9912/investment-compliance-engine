namespace FinTrack.Compliance.Domain.Tests;

using Xunit;
using FluentAssertions;
using FinTrack.Compliance.Domain.Entities;
using FinTrack.Compliance.Domain.ValueObjects;
using FinTrack.Compliance.Domain.Rules;

public class MaxConcentrationRuleTests
{
    [Fact]
    public void Evaluate_ShouldReturnTrue_WhenAssetConcentrationIsWithinAllowedLimit()
    {
        // Arrange
        var portfolio = new Portfolio("P-1", "Tech Growth Portfolio");
        portfolio.AddPosition("AAPL", 10, new Money(10, "USD")); // 100 USD
        portfolio.AddPosition("MSFT", 90, new Money(10, "USD")); // 900 USD

        var rule = new MaxConcentrationRule("RULE-01", "AAPL", 0.15m); // Limit 15%

        // Act
        var result = rule.Evaluate(portfolio, out var violationMessage);

        // Assert
        result.Should().BeTrue();
        violationMessage.Should().BeEmpty();
    }

    [Fact]
    public void Evaluate_ShouldReturnFalseAndMessage_WhenAssetConcentrationExceedsLimit()
    {
        // Arrange
        var portfolio = new Portfolio("P-1", "Tech Growth Portfolio");
        portfolio.AddPosition("AAPL", 40, new Money(10, "USD")); // 400 USD
        portfolio.AddPosition("MSFT", 60, new Money(10, "USD")); // 600 USD

        var rule = new MaxConcentrationRule("RULE-01", "AAPL", 0.30m); // Limit 30%

        // Act
        var result = rule.Evaluate(portfolio, out var violationMessage);

        // Assert
        result.Should().BeFalse();
        violationMessage.Should().Contain("Violated");
        violationMessage.Should().Contain("AAPL");
    }
}
