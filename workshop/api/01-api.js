// server.js
const express = require("express");
const app = express();

const users = [
  { id: 1, name: "Alice", email: "alice@example.com", role: "user" },
  { id: 2, name: "Bob", email: "bob@example.com", role: "user" },
];

app.use(express.json());

// Simulated authentication (user id from request header)
function fakeAuth(req, res, next) {
  const userId = parseInt(req.headers["x-user-id"]);
  req.user = users.find((u) => u.id === userId);
  next();
}

// ❌ No authorization check here
app.get("/api/users/:id", fakeAuth, (req, res) => {
  const user = users.find((u) => u.id === parseInt(req.params.id));
  if (!user) return res.status(404).send("User not found");
  res.json(user);
});

app.listen(3000, () =>
  console.log("🚀 Server running on http://localhost:3000")
);
