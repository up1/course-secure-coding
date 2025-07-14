const express = require("express");
const swaggerUi = require("swagger-ui-express");
const swaggerFile = require("./swagger_output.json");
const person = require("./routers/personRouter");
const helmet = require("helmet");
const cors = require('cors');

const app = express();

app.use(helmet());
app.use(express.json());
app.use(cors());

// Disable OPTIONS method globally
app.use((req, res, next) => {
  if (req.method === 'OPTIONS') {
    return res.status(405).send({ error: "Method Not Allowed" });
  }
  next();
});

app.use("/", person.personRouter);
app.use("/doc", swaggerUi.serve, swaggerUi.setup(swaggerFile));

// Global error handler with method not allowed
app.use((req, res, next) => {
  res.status(405).json({
    error: "Method Not Allowed",
    message: `The method ${req.method} is not allowed for the requested URL ${req.originalUrl}`,
  });
});

app.listen(3000, () => {
  console.log(`Running on 3000`);
});
