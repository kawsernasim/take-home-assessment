# Coterie Take-Home Assessment

## Overview
This repo is the deliverable for Coterie Insurance Take-Home Assessment: https://github.com/ci-engineering/take-home-assessment

The architecture follows a layered, testable, and extensible design, aligned with enterprise standards.

---

## High-Level Architecture

### Layers
- **API Layer (Controller)**: Entry point and delegates request to the `QuoteHandler`. 
- **Handler Layer**: Coordinates mapping, validation, service execution, and constructs appropriate success/failure responses.
- **Service Layer**: Implements the quote calculation and further business logic based as outlined.
- **Mapper Layer**: Converts and validates incoming DTOs to strongly-typed domain models.
- **Middleware**: Global error handler to standardize exception responses.

>The focus was on achieving a clean separation of concerns, making the codebase easy to maintain and extend.
---

## Testing Strategy

Unit tests have been implemented for the Mapper Layer and Service Layer. The tests cover several scenarios, including but not limited to: valid, invalid input, few edge cases. Due to the time limitations, unit tests could not be implemented for the Handler Layer.

Test files are located under `Coterie.UnitTests/` in respective subfolders.

---

## 🚀 Running the API

```bash

# Run the API
dotnet run --project Coterie.Api

# Run Tests
dotnet test
```
>**Note: Due to limited native support of .NET 5.0 on Apple Silicone machines, this API was developed using .NET 6.0 as the target framework. However, it is compatible with .NET 5.0 and has been tested on a .NET 5.0 environment.**

Example Request:
```json
{
  "business": "Plumber",
  "revenue": 6000000,
  "states": ["TX", "OH", "FL"]
}
```

Example Response:
```json
{
  "business": "Plumber",
  "revenue": 6000000,
  "premiums": [
    {
      "premium": 11316,
      "state": "TX"
    },
    {
      "premium": 12000,
      "state": "OH"
    },
    {
      "premium": 14400,
      "state": "FL"
    }
  ],
  "isSuccessful": true,
  "transactionId": "27373db4-56c3-4383-a2e1-f55c77b4aa3f"
}
```

---

## Goals Met
- Clean, layered architecture
- Separation of concerns
- Request validation and error handling using FluentValidation
- Proper business logic encapsulation in the service layer
- Strongly-typed domain models
- Test-driven and modular implementation

---

## What I'd Enhance With More Time

Dynamic configuration management
  - Although I have outlined the quote calculation factors in the `appsettings.json`, I would implement a more dynamic configuration management system (e.g. configuration service) to allow for easier updates and changes to the factors without requiring code changes.

More Granular Error Model
  - Error handling could be improved by creating a more granular error model that provides more context about the errors encountered during processing. This would help in debugging and provide better feedback to the API consumers.
  - Exception handling using the middleware layer is a good start but could be enhanced by creating custom exceptions for different error scenarios.

Separate each layer into its own project
  - Separating each layer into its own project would improve modularity and maintainability. This would also allow for better versioning and deployment strategies.

Rule-Based Engine Abstraction
  - Move premium logic to a pluggable strategy or rules engine to support future product types and rules.

Async + Logging + Telemetry
  - Add structured logging and Telemetry tracing.
  - Make service/handler layers async-ready.

Expand Test Coverage
  - Add more unit tests to cover edge cases and ensure robustness.

Improve Response Model/DTOs
  - Due to time constraints, the response model is not fully aligned with the request model. I would ensure that the response model is consistent and provides all necessary information.
---

Feel free to reach out with any questions — thank you for reviewing my solution!
