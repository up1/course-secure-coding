// server.js
const express = require('express');
const app = express();

const users = [
  { id: 1, username: 'user01', password: 'password123' }, // plain text password 😱
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

// ❌ Public profile endpoint without authentication
app.get('/profile', (req, res) => {
  const userId = parseInt(req.query.userId);
  const user = users.find(u => u.id === userId);  
  if (!user) {
    return res.status(404).send('User not found');
  }
  res.json({ message: 'Profile data', user });
});

// ❌ Forgot password endpoint without security
app.post('/forgot-password', (req, res) => {
  const { username } = req.body;
  const user = users.find(u => u.username === username);

  if (!user) {
    return res.status(404).send('User not found');
  }

  // ❌ Sends plain text password via email (not secure)
  res.send(`Your password is: ${user.password}`);
});

// ❌ reset password endpoint without security, not checking user identity
app.post('/reset-password', (req, res) => {
  const { userId, newPassword } = req.body;

  const user = users.find(u => u.id === userId);

  if (!user) {
    return res.status(404).send('User not found');
  }

  // ❌ Updates password without any security checks
  user.password = newPassword;
  res.send('Password updated successfully');
});

app.listen(3000, () => console.log('🚨 Insecure login server running on http://localhost:3000'));
