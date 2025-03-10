// inventory-secure.js
const express = require('express');
const helmet = require('helmet');
const app = express();

const isProduction = process.env.NODE_ENV === 'production';

app.use(helmet());
app.use(express.json());

// ✅ Public API - Documented
app.get('/api/users', (req, res) => {
  res.json([{ id: 1, username: 'alice' }]);
});

// ✅ Dev-only route – only active in non-production environments
if (!isProduction) {
  app.get('/dev-test', (req, res) => {
    res.send('This is a dev test endpoint (non-production only)');
  });
}

// ✅ Legacy APIs should be explicitly deprecated or removed
// Example: This route is disabled in prod
if (!isProduction) {
  app.get('/v1/legacy-api', (req, res) => {
    res.send('Legacy API (deprecated)');
  });
}

app.listen(3000, () => {
  console.log(`✅ Secure server running in ${isProduction ? 'PROD' : 'DEV'} mode`);
});
