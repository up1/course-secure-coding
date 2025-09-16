# Demo secure and unsecure code
* REST API with .NET 8
* Web application with Next.js
* Running with Docker compose
* CI/CD with GitLab CI

## Reference from 
* OWASP Top 10 for Web
* OWASP Top 10 for API

## 1. Build and run API project with .NET 8

Create project
```
$dotnet new webapi -n api
$cd api
```

Add dependencies
```
$dotnet add package Microsoft.EntityFrameworkCore.InMemory
$dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

Build image and rin container
```
$docker compose build api
$docker compose up -d api
```
