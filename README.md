# Identity Service
This is a simple identity service that has been developed as a part of the education for the Bachelor in Software Development at University College Nordjylland (UCN)

## The Core
The core project contains code to `generate` and to `validate` a token. The `GenerateToken()` method should be used primarily from the API and the `VerifyToken()` would typically be moved into a service (or middleware) and validate the incoming requests. The `ClaimsReader` is used to read the claims from a verified token.

For using the `ReadClaim()` please refer to the unit tests.

This project also holds sample code for how to enable the middleware for an Web API to check and validate a token. The middleware it self is found in the `JWTTokenValidationMiddleware.cs` and is included as middleware in the `startup.cs` file. Please note that enabling the middleware in this solution would prevent the generation and validation of tokens. That code would instead go in the destination Web API services.
