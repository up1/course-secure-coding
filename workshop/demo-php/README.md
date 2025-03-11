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
```
$curl -X POST http://localhost:8000/login \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "username=alice&password=password123"
```