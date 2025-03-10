# Workshop with .NET C#
* .NET 9
* In-memory database


## 1. Demo 01 with .NET
* Broken Object Level Authorization
* Broken Authentication
* Broken Object Property Level Authorization

**A login form that**
* Doesn't validate passwords securely
* Issues tokens without proper validation
* Leaks information via error messages

**Then we fix it with**
* Secure password hashing
* Proper JWT configuration
* Safer error handling

Run demo
```
$cd 01-api
$docker compose build api01
$docker compose up -d api01
```

Testing with postman
* Import from file `/postman/api-dotnet.postman_collection.json`

* Broken Object Level Authorization
  * 1. Generate token
  * 2. Get order by id
* Broken Authentication
  * 3. Register
       * Hash password with SHA256
  * 4. Login with secure
* Broken Object Property Level Authorization
  * 5. Update datae from whitelisted fields