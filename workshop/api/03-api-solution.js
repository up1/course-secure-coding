// secure-server.js
const express = require('express');
const _ = require('lodash');
const app = express();

let users = [
  { id: 1, username: 'alice', email: 'alice@example.com', role: 'user' },
  { id: 2, username: 'bob', email: 'bob@example.com', role: 'user' },
];

app.use(express.json());

function fakeAuth(req, res, next) {
  const userId = parseInt(req.headers['x-user-id']);
  req.user = users.find(u => u.id === userId);
  next();
}

app.patch('/api/users/:id', fakeAuth, (req, res) => {
  const targetUser = users.find(u => u.id === parseInt(req.params.id));
  if (!targetUser) return res.status(404).send('User not found');

  // ✅ Only allow safe fields to be updated
  const allowedFields = ['email'];
  const safeUpdates = _.pick(req.body, allowedFields);

  Object.assign(targetUser, safeUpdates);
  res.json({ message: 'User safely updated', user: targetUser });
});

app.listen(3000, () => console.log('✅ Secure server running at http://localhost:3000'));
