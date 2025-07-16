# Workshop Dynamic Application Security Testing (DAST) for REST API
* [OWASP ZAP](https://www.zaproxy.org/)
* OpenAPI
* Docker
* [Juice shop](https://github.com/juice-shop/juice-shop)

## Step 1 :: Run target API server
```
$docker compose up -d petclinic
$docker compose up -d juice_shop
$docker compose ps
```

## Step 2 :: Run ZAP
```
$docker compose up zap
```

See report in folder 'reports/'