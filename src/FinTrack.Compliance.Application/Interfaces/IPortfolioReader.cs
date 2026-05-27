namespace FinTrack.Compliance.Application.Interfaces;

using FinTrack.Compliance.Domain.Entities;

public interface IPortfolioReader
{
    Portfolio ReadFromCsv(string filePath, string portfolioId, string portfolioName);
}
