// server.js
const express = require('express');
const app = express();

app.use(express.json()); // No payload size limit! (default of express is 100kb)

// Simulate an expensive operation
app.post('/api/upload', (req, res) => {
  const { data } = req.body;
  // Simulated processing delay
  setTimeout(() => {
    res.send(`Received data with length: ${data?.length}`);
  }, 1000);
});

app.listen(3000, () => console.log('🚨 Vulnerable server running on http://localhost:3000'));
