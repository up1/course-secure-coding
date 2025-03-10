# Workshop with .NET C#


## 1. Broken Object Level Authorization and Broken Authentication

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
  * 1. Generate token
  * 2. Get order by id