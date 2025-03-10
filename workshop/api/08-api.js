// insecure-server.js
const express = require('express');
const app = express();

app.use(express.json());

// Insecure CORS (open to all origins)
app.use((req, res, next) => {
  res.setHeader('Access-Control-Allow-Origin', '*'); // ❌ open to everyone
  res.setHeader('Access-Control-Allow-Methods', '*');
  res.setHeader('Access-Control-Allow-Headers', '*');
  next();
});

// Verbose error route
app.get('/api/error', (req, res) => {
  throw new Error('Something bad happened!'); // ❌ unhandled
});

app.listen(3000, () => console.log('🚨 Insecure server running on http://localhost:3000'));
