namespace FinTrack.Compliance.Domain.Rules;

using FinTrack.Compliance.Domain.Entities;

public interface IComplianceRule
{
    string Id { get; }
    string Description { get; }
    bool Evaluate(Portfolio portfolio, out string violationMessage);
}
