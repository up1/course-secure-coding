// ssrf-protected.js
const express = require('express');
const axios = require('axios');
const dns = require('dns');
const { URL } = require('url');
const app = express();

app.use(express.json());

// ✅ Allowlist of trusted domains
const ALLOWED_HOSTS = ['example.com', 'jsonplaceholder.typicode.com'];

function isHostAllowed(hostname) {
  return ALLOWED_HOSTS.includes(hostname);
}

// Resolve DNS and check IPs
function isPrivateIP(ip) {
  return (
    ip.startsWith('10.') || // Class A
    ip.startsWith('192.168.') || // Class C
    ip.startsWith('172.') || // Class B
    ip.startsWith('127.') || // Loopback
    ip.startsWith('169.254') || // Link-local
    ip === '::1'
  );
}

app.post('/api/fetch-url', async (req, res) => {
  const { url } = req.body;

  try {
    const parsedUrl = new URL(url);
    const hostname = parsedUrl.hostname;

    if (!isHostAllowed(hostname)) {
      return res.status(403).send('❌ Host not allowed');
    }

    // DNS resolution check
    dns.lookup(hostname, async (err, address) => {
      if (err || isPrivateIP(address)) {
        return res.status(403).send('❌ Forbidden IP address');
      }

      try {
        const response = await axios.get(url);
        res.send(response.data);
      } catch (error) {
        res.status(500).send('Failed to fetch content');
      }
    });
  } catch (err) {
    res.status(400).send('Invalid URL');
  }
});

app.listen(3000, () => console.log('✅ SSRF-protected server running on http://localhost:3000'));
