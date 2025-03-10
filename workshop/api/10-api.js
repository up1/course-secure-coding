// unsafe-consumption.js
const express = require('express');
const axios = require('axios');
const app = express();

app.use(express.json());

// ❌ Fetching user data from third-party API without validation
app.post('/api/import-user', async (req, res) => {
  const { externalApiUrl } = req.body;

  try {
    const response = await axios.get(externalApiUrl);
    const user = response.data;

    // ❌ Trusts external data blindly
    // Simulating insertion into DB or further processing
    res.json({ message: 'User imported', user });
  } catch (err) {
    res.status(500).send('Failed to import user');
  }
});

app.listen(3000, () => console.log('🚨 Insecure server running on http://localhost:3000'));
