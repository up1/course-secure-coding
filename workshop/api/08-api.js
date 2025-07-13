// insecure-server.js
const express = require('express');
const cors = require('cors');
const app = express();

app.use(express.json());

// Misconfiguration 1: Allowing all origins in CORS
app.use(cors());

// Misconfiguration 2: Exposing server information
app.use((req, res, next) => {
    res.setHeader('X-Powered-By', 'Express 4.21.2');
    res.setHeader('Server', 'Apache/2.4.1');
    next();
});

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
