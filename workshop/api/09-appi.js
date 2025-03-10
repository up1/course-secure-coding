// inventory-vulnerable.js
const express = require('express');
const app = express();

app.use(express.json());

// Public production API
app.get('/api/users', (req, res) => {
  res.json([{ id: 1, username: 'alice' }]);
});

// ❌ Dev-only test route accidentally deployed to prod
app.get('/dev-test', (req, res) => {
  res.send('This is a dev test endpoint');
});

// ❌ Old v1 endpoint still live
app.get('/v1/legacy-api', (req, res) => {
  res.send('Legacy API response');
});

app.listen(3000, () => console.log('🚨 Insecure inventory server running on http://localhost:3000'));
