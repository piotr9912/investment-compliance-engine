namespace FinTrack.Compliance.Infrastructure.Services;

using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using FinTrack.Compliance.Application.Interfaces;
using FinTrack.Compliance.Domain.Entities;
using FinTrack.Compliance.Domain.ValueObjects;

public record CsvPositionRow(string AssetId, decimal Quantity, decimal Price, string Currency);

public class CsvPortfolioReader : IPortfolioReader
{
    public Portfolio ReadFromCsv(string filePath, string portfolioId, string portfolioName)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Financial data file not found at: {filePath}");

        var portfolio = new Portfolio(portfolioId, portfolioName);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ","
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);
        
        var records = csv.GetRecords<CsvPositionRow>();

        foreach (var row in records)
        {
            var marketPrice = new Money(row.Price, row.Currency);
            portfolio.AddPosition(row.AssetId, row.Quantity, marketPrice);
        }

        return portfolio;
    }
}

