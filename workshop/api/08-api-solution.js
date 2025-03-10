// secure-server.js
const express = require('express');
const helmet = require('helmet');
const cors = require('cors');
const app = express();

// ✅ Secure HTTP headers
app.use(helmet());

// ✅ CORS - allow only trusted origins
app.use(cors({
  origin: ['https://yourdomain.com'], // 🔒 allow only specific origin(s)
  methods: ['GET', 'POST'],
}));

app.use(express.json());

// ✅ Generic error handler
app.get('/api/error', (req, res, next) => {
  try {
    throw new Error('Something bad happened!');
  } catch (err) {
    next(err);
  }
});

// Centralized error handling middleware
app.use((err, req, res, next) => {
  console.error('Error:', err.message); // ✅ log internally
  res.status(500).send('Internal Server Error'); // ✅ generic message
});

app.listen(3000, () => console.log('✅ Secure server running on http://localhost:3000'));
