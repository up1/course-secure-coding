# Secure API coding with NodeJS
* [OWASP Top 10 API Security Risks – 2023](https://github.com/up1/course-secure-coding/wiki/Workshop-::-OWASP-Top-10-API-Security-Risks-%E2%80%93-2023)
* [Security Vulnerability Guide](https://docs.levo.ai/vulnerabilities/v1/guide)
* Example code
* Testing


## 1. Broken Object Level Authorization (BOLA)
```
$npm i
$node 01-api.js
```

List of APIs
* http://localhost:3000/api/users/1
* http://localhost:3000/api/users/2

## 2. Broken Authentication
```
$npm i
$node 02-api.js
```

List of APIs
* POST http://localhost:3000/login
* GET http://localhost:3000/profile?userId=1

## 3. Broken Object Property Level Authorization
```
$npm i
$node 03-api.js
```

List of APIs
* POST http://localhost:3000/api/users/1

## 4. Unrestricted Resource Consumption
```
$npm i
$node 04-api.js
```

List of APIs
* POST http://localhost:3000/api/upload

## 5. Broken Function Level Authorization
```
$npm i
$node 05-api.js
```

List of APIs
* DELETE http://localhost:3000/api/users/1

## 6. Unrestricted Access to Sensitive Business Flow
```
$npm i
$node 06-api.js
```

List of APIs
* POST http://localhost:3000/api/redeem-coupon
* GET http://localhost:3000/metrics


## 8. Security Misconfiguration
```
$npm i
$node 08-api.js
```

List of APIs
* GET http://localhost:3000/api/error