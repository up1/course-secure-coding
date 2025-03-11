# Workshop with PHP
* [Slim framework](https://www.slimframework.com/)
* Docker


## Step 1 :: Build and run php and Slim
```
$docker compose build php
$docker compose up -d php

$docker compose ps        
NAME             IMAGE          COMMAND                  SERVICE   CREATED              STATUS          PORTS
demo-php-php-1   demo-php-php   "docker-php-entrypoi…"   php       About a minute ago   Up 45 seconds   0.0.0.0:9000->9000/tcp
```

## Step 2 :: Run webserver with Nginx
```
$docker compose up -d nginx
$docker compose ps
```

Access from web browser
* http://localhost:8000

## Step 3 :: Broken Object Level Authorization (BOLA)
* Always check if the resource ID matches the authenticated user
* Centralize object access logic when possible
* Consider using RBAC (role-based access control) for more complex permissions
* Use auth middleware to extract identity

```
$curl http://localhost:8000/api/users/2/not-secure -H "X-User-ID: 1"
$curl http://localhost:8000/api/users/2/secure -H "X-User-ID: 1"
```

## Step 4 :: Broken Authentication
* Passwords are stored insecurely (e.g. plain text)
* Sessions/tokens are not securely handled
* Authentication logic is weak or missing
```
$curl -X POST http://localhost:8000/login \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "username=alice&password=password123"
```

## Step 5 :: Broken Object Property Level Authorization
* When a user is allowed to update their profile, but the backend blindly accepts all input, malicious users can modify sensitive properties
* Use whitelist

Working with bad practice
```
$curl -X PATCH http://localhost:8000/api/profile/bad \
  -H "X-User-ID: 1" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "email=alice@safe.com&role=admin"
```

Better
```
$curl -X PATCH http://localhost:8000/api/profile \
  -H "X-User-ID: 1" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "email=alice@safe.com&role=admin"
```