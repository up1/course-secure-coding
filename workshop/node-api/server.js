const express = require('express');
const swaggerUi = require('swagger-ui-express')
const swaggerFile = require('./swagger_output.json')
const person = require('./routers/personRouter')

const app = express();

app.use(express.json());
app.use('/', person.personRouter)
app.use('/doc', swaggerUi.serve, swaggerUi.setup(swaggerFile))

app.listen(3000, () => {
  console.log(`Running on 3000`);
});
