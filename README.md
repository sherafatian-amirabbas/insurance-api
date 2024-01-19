T1:
  - Necessary unit tests for edge cases added coverring the business logic.
  - Bug fixed.
  - Unit Tests passed
  - Any refactoring exluded in this stage.

T2:
  - HttpClient enhanced.
  - ExceptionHandler middleware added to map technical and business exceptions to http response.
  - Microsoft.Extensions.Logging is used to log exceptions and http requests/responses.
  - A RequestId is generated and attached to logs and exceptions to follow up errors in later stages.
  - A model-centric architecture (Hexagonal) is applied to refactor. By this architecture we can focus on the business logic and not distracted by implementation details. The benefit is that business logic, which is crucial and necessary, can be started right away while decision regarding details (such as database) can be deferred. This also heavily makes the unit tests focused. The architecture consist of assemblies: Insurance.Contracts (models and abstractions), Insurance.Application (business logics), Insurance.Infrastructure (DataApi proxy as a plugin), Insurance.Common (shared services across the projects), and Insurance.Api (a client facing application).
  - Inversion of Control applied by the use of .Net dependency injection.
  - A minimal CircuitBreaker per DataApi endpoints impemented: it's a minimal implementation without a retry mechanism. 
  - The unit tests completed/refactored and 'moq' is applied to mock the implementation details when writing tests against business logic. 