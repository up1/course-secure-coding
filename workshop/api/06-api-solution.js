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

// Add prometheus middleware to log coupon usage
// This is a placeholder; in a real app, you would use a proper monitoring library
const prometheus = require('prom-client');
const register = new prometheus.Registry();
const couponUsageCounter = new prometheus.Counter({
  name: 'coupon_usage_total',
  help: 'Total number of coupon usages',
  labelNames: ['userId'],   
});

// Initialize the Prometheus client
register.registerMetric(couponUsageCounter);

// Add a route to expose the metrics
app.get("/metrics", async (req, res, next) => {
  res.setHeader("Content-type", register.contentType);
  res.send(await register.metrics());
  next();
});

// ✅ Enforce 1-time use per user
app.post('/api/redeem-coupon', fakeAuth, couponLimiter, (req, res) => {
  const { coupon } = req.body;

  couponUsageCounter.inc({ userId: req.userId }); 

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
