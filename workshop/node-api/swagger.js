const swaggerAutogen = require('swagger-autogen')()

const doc = {
    securityDefinitions: {
        apiKeyAuth:{
            type: "apiKey",
            in: "header",       // can be "header", "query" or "cookie"
            name: "X-API-KEY",  // name of the header, query parameter or cookie
            description: "any description..."
        }
    }
}

const outputFile = './swagger_output.json'
const endpointsFiles = ['./routers/personRouter.js']

swaggerAutogen(outputFile, endpointsFiles, doc);