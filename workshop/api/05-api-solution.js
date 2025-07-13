// secure-server.js
const express = require('express');
const app = express();

let users = [
  { id: 1, username: 'alice', role: 'user' },
  { id: 2, username: 'bob', role: 'admin' },
];

app.use(express.json());

// Fake auth with user lookup
function fakeAuth(req, res, next) {
  const userId = parseInt(req.headers['x-user-id']);
  const requestedUserId = parseInt(req.params.id);
  if(userId !== requestedUserId) {
    return res.status(403).send('Forbidden');
  }
  req.user = users.find(u => u.id === userId);
  if (!req.user) return res.status(401).send('Unauthorized');
  next();
}

// ✅ Role check middleware
function requireRole(role) {
  return (req, res, next) => {
    if (req.user.role !== role) {
      return res.status(403).send('Access denied. Admins only.');
    }
    next();
  };
}

// ✅ Protected route
app.delete('/api/users/:id', fakeAuth, requireRole('admin'), (req, res) => {
  const userId = parseInt(req.params.id);
  users = users.filter(u => u.id !== userId);
  res.send(`User ${userId} deleted by admin`);
});

app.listen(3000, () => console.log('✅ Secure server running at http://localhost:3000'));
