// safe-consumption.js
const express = require('express');
const axios = require('axios');
const { z } = require('zod');
const app = express();

app.use(express.json());

// ✅ Zod schema for user validation
const userSchema = z.object({
  username: z.string().min(1).max(50),
  email: z.string().email(),
  role: z.enum(['user', 'admin']),
  isActive: z.boolean(),
});

app.post('/api/import-user', async (req, res) => {
  const { externalApiUrl } = req.body;

  try {
    const response = await axios.get(externalApiUrl);
    const parsed = userSchema.safeParse(response.data);

    if (!parsed.success) {
      return res.status(400).send('Invalid user data from external source');
    }

    // ✅ Data is validated and safe to use
    res.json({ message: 'User safely imported', user: parsed.data });
  } catch (err) {
    res.status(500).send('Failed to fetch external data');
  }
});

app.listen(3000, () => console.log('✅ Secure server running on http://localhost:3000'));
