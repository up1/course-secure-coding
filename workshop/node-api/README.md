# NodeJS and Swagger/OpenAPI
* [Production Best Practices: Security](https://expressjs.com/en/advanced/best-practice-security.html)

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

## Working with Docker

Build image
```
$docker compose build
```

Generate by [syft](https://github.com/anchore/syft)
```
$syft node-api:1.0 -o cyclonedx-json@1.6
$syft node-api:1.0 --output cyclonedx-json@1.6=docker-sbom.json
```

Generate SBOM from Docker image
```
$docker sbom --version

$docker sbom node-api:1.0 --format spdx-json --output docker-sbom.json
```