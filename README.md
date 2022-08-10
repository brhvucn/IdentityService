# Identity Service
This is a simple identity service that has been developed as a part of the education for the Bachelor in Software Development at University College Nordjylland (UCN)

## The Core
The core project contains code to `generate` and to `validate` a token. The `GenerateToken()` method should be used primarily from the API and the `VerifyToken()` would typically be moved into a service (or middleware) and validate the incoming requests. The `ClaimsReader` is used to read the claims from a verified token.
