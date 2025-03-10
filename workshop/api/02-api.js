// server.js
const express = require('express');
const app = express();

const users = [
  { id: 1, username: 'alice', password: 'password123' }, // plain text password 😱
];

app.use(express.json());

app.post('/login', (req, res) => {
  const { username, password } = req.body;
  const user = users.find(u => u.username === username && u.password === password);

  if (!user) {
    return res.status(401).send('Invalid credentials');
  }

  // ❌ Returns session-like object with no token or expiration
  res.json({ message: 'Logged in!', userId: user.id });
});

app.listen(3000, () => console.log('🚨 Insecure login server running on http://localhost:3000'));
