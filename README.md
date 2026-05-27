# Investment Compliance Engine (FinTrack.InvestmentCompliance)

A production-ready, high-performance Investment Compliance Engine built with **.NET 9** following **Clean Architecture** and **Domain-Driven Design (DDD)** principles. 

This project simulates a core component of an asset management platform (similar to SimCorp Dimension), where investment portfolios are validated against strict statutory and client-defined investment standards before and after trade execution.

## 🚀 Architectural Highlights & Best Practices

- **Clean Architecture**: Complete decoupling of core financial business logic from infrastructure, UI, and external frameworks.
- **Domain-Driven Design (DDD)**: Rich domain model utilizing `Entities`, `Aggregates`, and `Value Objects` (e.g., thread-safe `Money` structure to prevent raw decimal/double confusion).
- **Strategy Pattern for Rules**: Compliance checks (e.g., Asset Concentration, Leverage Limits, Currency Exposure) are implemented as interchangeable strategies, making the engine highly extensible.
- **Financial Data Ingestion**: Features an extensible data ingestion pipeline with an implemented `CsvPortfolioReader` using `CsvHelper`, showcasing how raw End-of-Day (EoD) transactional files are safely mapped into the rich domain layer.
- **FinTech Grade Precision**: Strict usage of `decimal` data types across all financial calculations to eliminate floating-point rounding errors.
- **Defensive Programming**: Comprehensive input validation and resilient error handling to process malformed market data or corrupted trade feeds.
- **High Performance & Async I/O**: Designed to process massive sets of portfolio positions asynchronously using modern .NET constructs.

## 🛠️ Technology Stack

- **Backend**: .NET 9 Core
- **Data Parsing**: CsvHelper
- **Testing**: xUnit, FluentAssertions (for human-readable assertions), NSubstitute (for mocking)
- **Code Quality**: Strict `.editorconfig` rules, `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` enabled.
- **Resilience**: Polly (for transient fault handling in infrastructure data fetching)

## 📂 Solution Structure

- `src/FinTrack.Compliance.Domain`: The core heartbeat. Contains financial models (`Portfolio`, `Position`, `Asset`) and rule validation algorithms. Zero external dependencies.
- `src/FinTrack.Compliance.Application`: Use cases, abstractions (`IPortfolioReader`), rule execution orchestrators, and interface definitions.
- `src/FinTrack.Compliance.Infrastructure`: Production-ready file parsers (`CsvPortfolioReader`), in-memory data store implementations, and external market-data mock clients.
- `src/FinTrack.Compliance.Api`: REST API endpoints to trigger portfolio compliance checks and fetch audit reports.
- `tests/FinTrack.Compliance.Domain.Tests`: Domain-specific Unit Tests covering core financial limits and calculations.
- `tests/FinTrack.Compliance.IntegrationTests`: End-to-End integration tests validating the data pipeline from a raw file feed up to the final portfolio evaluation state.

## 🚦 How to Run & Test

Prerequisites: .NET 9 SDK installed.

### Clone the repository
```bash
git clone https://github.com
cd investment-compliance-engine
```

### Build the Solution
```bash
dotnet build -c Release
```

### Run All Tests (Unit & Integration)
```bash
dotnet test
```

## 📈 Production Trade-Offs & Scalability Roadmap

In a time-constrained environment, certain conscious compromises were made:
1. **In-Memory Storage**: Current state is maintained in-memory. For a production SimCorp-scale system, this would be backed by a highly concurrent relational database (e.g., PostgreSQL / SQL Server) with distributed caching (Redis) for rapid asset price lookups.
2. **Synchronous Evaluation**: Rules are currently evaluated sequentially per portfolio. In a live environment processing millions of trades daily, this would utilize a reactive pipeline or an actor-model framework (like Proto.Actor or Akka.NET) to execute rules in parallel across distributed cluster nodes.
