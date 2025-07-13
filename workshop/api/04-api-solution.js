// secure-server.js
const express = require('express');
const rateLimit = require('express-rate-limit');
const helmet = require('helmet');

const app = express();

// ✅ Basic security headers
app.use(helmet());

// ✅ Limit incoming JSON body size to 10KB
app.use(express.json({ limit: '10kb' }));
app.use(express.urlencoded({ extended: true, limit: '50mb' })); // Increase URL-encoded payload limit

// ✅ Rate limiting: 100 requests per 15 minutes per IP
const limiter = rateLimit({
  windowMs: 15 * 60 * 1000,
  max: 100,
  message: '⚠️ Too many requests from this IP, please try again later.',
});
app.use('/api/', limiter);

// ✅ Simulate paginated resource
const fakeData = Array.from({ length: 1000 }, (_, i) => ({ id: i + 1, value: `Item ${i + 1}` }));

app.get('/api/items', (req, res) => {
  const page = parseInt(req.query.page) || 1;
  const limit = 20;
  const start = (page - 1) * limit;
  const end = start + limit;

  res.json({
    page,
    total: fakeData.length,
    items: fakeData.slice(start, end),
  });
});

app.listen(3000, () => console.log('✅ Secure server running on http://localhost:3000'));
