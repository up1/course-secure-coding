# Workshop with HTTPs
* Nginx
* Certbot
  * Let's Encrypt Client
* Docker


## Step 1 :: Install web
```
$docker compose up -d web
$docker compose ps
```

Check your web server
```
$curl http://localhost:3000/hello
```

## Step 2 :: Install Nginx
```
$docker compose up -d nginx
$docker compose ps
```

Check your server
```
$curl http://localhost/hello
```

## Step 3 :: Install [certbot](https://certbot.eff.org/)
```
$docker compose up -d certbot
$docker compose ps

# Detail of letsencrypt
$docker-compose exec nginx ls -la /etc/letsencrypt
```

## Step 4 :: in certbot
* Change from `--staging` to `--force-renewal`

## Step 5 :: Working with HTTPS
```
$docker compose down
$docker compose up -d web
$docker compose up -d nginx
```

Check https in your domain
```
$curl https://your-domain/hello
```