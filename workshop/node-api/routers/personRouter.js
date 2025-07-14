const express = require('express');
const router = express.Router();

router.get("/healthcheck", (req, res) => {
  res.status(200).send("OK");
});

router.get("/person", (req, res) => {
  // Here you would typically fetch the list of persons from a database
  res.status(200).json([{ id: 1, name: "John Doe" }]);
});

router.get("/person/:id", (req, res) => {
  const userId = req.params.id;
  res.status(200).json({ id: userId, name: "John Doe" });
});

router.post("/person", (req, res) => {
  const newPerson = req.body;
  // Here you would typically save the new person to a database
  res.status(201).json({ message: "Person created", person: newPerson });
});

module.exports = {
  personRouter: router
};
