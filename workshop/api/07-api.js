// ssrf-vulnerable.js
const express = require('express');
const axios = require('axios');
const app = express();

app.use(express.json());

// ❌ SSRF-vulnerable endpoint
app.post('/api/fetch-url', async (req, res) => {
  const { url } = req.body;

  try {
    const response = await axios.get(url); // 🧨 Dangerous!
    res.send(response.data);
  } catch (err) {
    res.status(500).send('Failed to fetch URL');
  }
});

app.listen(3000, () => console.log('🚨 SSRF-vulnerable server running on http://localhost:3000'));
