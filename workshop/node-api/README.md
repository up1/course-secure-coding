# NodeJS and Swagger/OpenAPI

## Install
* [Swagger UI Express](https://www.npmjs.com/package/swagger-ui-express)
* [swagger-autogen](https://www.npmjs.com/package/swagger-autogen)
```
$npm install
```

## Generate swagger
```
$npm run swagger-autogen
```
* Create swagger documentation in file `swagger_output.json`

## Start API server
```
$npm start
```

List of URLs
* API documentation with Swagger
  * http://localhost:3000/doc/

## API Testing
* [OFFAT - OFFensive Api Tester](https://github.com/OWASP/OFFAT)
* [Install Python](https://www.python.org/)
```
$pip install "offat[api]"

$offat -f swagger_output.json -o output.html -of html
```