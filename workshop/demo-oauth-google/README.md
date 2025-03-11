# Social authentication
* [Google Console API](https://console.cloud.google.com/apis/dashboard)
  * Enable APIs & Services => Create Credentials => OAuth client ID
    * select Web application
    * Authorized redirect URI use http://localhost:3000/auth/google/callback

## 1. Install dependencies
```
$npm install
```

## 2. Run
```
$node index.js
```

Access from web browser
* http://localhost:3000/