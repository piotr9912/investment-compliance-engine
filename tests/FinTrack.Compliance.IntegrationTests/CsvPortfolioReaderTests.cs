namespace FinTrack.Compliance.IntegrationTests;

using System.IO;
using Xunit;
using FluentAssertions;
using FinTrack.Compliance.Infrastructure.Services;

public class CsvPortfolioReaderTests : System.IDisposable
{
    private readonly string _tempFilePath;

    public CsvPortfolioReaderTests()
    {
        _tempFilePath = Path.GetTempFileName() + ".csv";
        
        var csvContent = 
@"AssetId,Quantity,Price,Currency
AAPL,10,150.50,USD
MSFT,20,310.25,USD";

        File.WriteAllText(_tempFilePath, csvContent);
    }

    [Fact]
    public void ReadFromCsv_ShouldCorrectlyLoadPositionsAndCalculateTotalValue()
    {
        // Arrange
        var reader = new CsvPortfolioReader();

        // Act
        var portfolio = reader.ReadFromCsv(_tempFilePath, "P-CSV-01", "Imported Portfolio");

        // Assert
        portfolio.Should().NotBeNull();
        portfolio.Id.Should().Be("P-CSV-01");
        portfolio.Positions.Should().HaveCount(2);
        portfolio.GetTotalValue().Should().Be(7710.00m);
    }

    public void Dispose()
    {
        if (File.Exists(_tempFilePath))
        {
            File.Delete(_tempFilePath);
        }
    }
}
