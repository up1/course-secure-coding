// secure-server.js
const express = require('express');
const bcrypt = require('bcrypt');
const jwt = require('jsonwebtoken');
const app = express();

const JWT_SECRET = 'your-super-secret-key'; // 🔐 Store in .env in real apps

// Hashed password using bcrypt.hashSync('password123', 10)
const users = [
  { id: 1, username: 'user01', password: '$2b$10$/DfAJokKVHsqgRZ9D84luemkUeGYQU7hBzUs8Q3oZsrhrrNafaUj.' },
];

app.use(express.json());

app.post('/login', async (req, res) => {
  const { username, password } = req.body;
  const user = users.find(u => u.username === username);

  if (!user) return res.status(401).send('Invalid credentials');
  const isPasswordValid = await bcrypt.compare(password, user.password);
  if (!isPasswordValid) return res.status(401).send('Invalid password credentials');

  // ✅ Generate secure JWT
  const token = jwt.sign({ userId: user.id, username: user.username }, JWT_SECRET, {
    expiresIn: '1h',
  });

  res.json({ token });
});

// ✅ Protected route using JWT
app.get('/profile', (req, res) => {
  const authHeader = req.headers['authorization'];
  if (!authHeader) return res.status(401).send('No token provided');

  const token = authHeader.split(' ')[1];
  try {
    const decoded = jwt.verify(token, JWT_SECRET);
    res.json({ message: 'Access granted', user: decoded });
  } catch (err) {
    res.status(401).send('Invalid or expired token');
  }
});

app.listen(3000, () => console.log('✅ Secure server running on http://localhost:3000'));
