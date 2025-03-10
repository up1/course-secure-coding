// server.js
const express = require('express');
const app = express();

let couponUses = {}; // Tracks coupon usage per user

app.use(express.json());

// Fake auth
function fakeAuth(req, res, next) {
  const userId = req.headers['x-user-id'];
  if (!userId) return res.status(401).send('Unauthorized');
  req.userId = userId;
  next();
}

// ❌ No limits — users can spam this!
app.post('/api/redeem-coupon', fakeAuth, (req, res) => {
  const { coupon } = req.body;

  if (coupon !== 'DISCOUNT2024') {
    return res.status(400).send('Invalid coupon');
  }

  couponUses[req.userId] = (couponUses[req.userId] || 0) + 1;

  res.send(`Coupon redeemed! You have used it ${couponUses[req.userId]} times.`);
});

app.listen(3000, () => console.log('🚨 Insecure coupon server running at http://localhost:3000'));
