// server.js
const express = require('express');
const app = express();

let users = [
  { id: 1, username: 'alice', email: 'alice@example.com', role: 'user' },
  { id: 2, username: 'bob', email: 'bob@example.com', role: 'user' },
];

app.use(express.json());

// Fake authentication middleware
function fakeAuth(req, res, next) {
  const userId = parseInt(req.headers['x-user-id']);
  req.user = users.find(u => u.id === userId);
  next();
}

// ❌ Insecure: Accepts any fields from client
app.post('/api/users/:id', fakeAuth, (req, res) => {
  const targetUser = users.find(u => u.id === parseInt(req.params.id));
  if (!targetUser) return res.status(404).send('User not found');

  Object.assign(targetUser, req.body); // ⚠️ Dangerous!

  res.json({ message: 'User updated', user: targetUser });
});

app.listen(3000, () => console.log('🚨 Vulnerable server running at http://localhost:3000'));
