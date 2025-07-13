// secure-server.js
const express = require('express');
const rateLimit = require('express-rate-limit');
const app = express();

let couponUses = {};

app.use(express.json());

// Fake auth
function fakeAuth(req, res, next) {
  const userId = req.headers['x-user-id'];
  if (!userId) return res.status(401).send('Unauthorized');
  req.userId = userId;
  next();
}

// ✅ Rate limit coupon route: max 5 requests per hour per IP
const couponLimiter = rateLimit({
  windowMs: 60 * 60 * 1000, // 1 hour
  max: 5,
  message: 'Too many coupon attempts. Try again later.',
});

// ✅ Enforce 1-time use per user
app.post('/api/redeem-coupon', fakeAuth, couponLimiter, (req, res) => {
  const { coupon } = req.body;

  if (coupon !== 'DISCOUNT2024') {
    return res.status(400).send('Invalid coupon');
  }

  if (couponUses[req.userId]) {
    return res.status(403).send('Coupon already used');
  }

  couponUses[req.userId] = true;
  res.send('Coupon redeemed successfully!');
});

app.listen(3000, () => console.log('✅ Secure coupon server running at http://localhost:3000'));
