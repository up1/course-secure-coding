# Workshop Dynamic Application Security Testing (DAST) for REST API
* OWASP ZAP
* OpenAPI
* Docker


## Step 1 :: Run target API server
```
$docker compose up -d petclinic
$docker compose ps
```

## Step 2 :: Run ZAP
```
$docker compose up zap
```