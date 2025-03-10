// server.js
const express = require('express');
const app = express();

let users = [
  { id: 1, username: 'alice', role: 'user' },
  { id: 2, username: 'bob', role: 'admin' },
];

app.use(express.json());

// Simulated auth middleware
function fakeAuth(req, res, next) {
  const userId = parseInt(req.headers['x-user-id']);
  req.user = users.find(u => u.id === userId);
  next();
}

// ❌ No role check! Anyone can delete users!
app.delete('/api/users/:id', fakeAuth, (req, res) => {
  const userId = parseInt(req.params.id);
  users = users.filter(u => u.id !== userId);
  res.send(`User ${userId} deleted`);
});

app.listen(3000, () => console.log('🚨 Insecure server running on http://localhost:3000'));
