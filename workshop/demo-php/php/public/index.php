<?php

require dirname(__DIR__) . '/vendor/autoload.php';

use Psr\Http\Message\ResponseInterface as Response;
use Psr\Http\Message\ServerRequestInterface as Request;
use Slim\Factory\AppFactory;
use Firebase\JWT\JWT;

$app = AppFactory::create();
$secretKey = 'super-secret-key'; // Use .env in real apps

// ✅ Passwords are hashed (simulating DB)
$users = [
    ['id' => 1, 'username' => 'alice', 'password' => password_hash('password123', PASSWORD_BCRYPT)],
    ['id' => 2, 'username' => 'bob', 'password' => password_hash('letmein', PASSWORD_BCRYPT)],
];

// Simulated auth middleware using header "X-User-ID"
$app->add(function (Request $request, $handler) use (&$users) {
    $userId = (int)($request->getHeaderLine('X-User-ID'));
    if (!isset($users[$userId])) {
        $response = new \Slim\Psr7\Response();
        return $response->withStatus(401)->withBody(\Slim\Psr7\Stream::createFromString('Unauthorized'));
    }
    $request = $request->withAttribute('user', $users[$userId]);
    return $handler->handle($request);
});

// Add handle 404
$app->addRoutingMiddleware();
$errorMiddleware = $app->addErrorMiddleware(true, true, true);

// ❌ No check to ensure the requested ID belongs to the logged-in user
$app->get('/api/users/{id}/not-secure', function (Request $request, Response $response, $args) use (&$users) {
    $id = (int)$args['id'];
    if (!isset($users[$id])) {
        $response->getBody()->write("User not found");
        return $response->withStatus(404);
    }

    $user = $users[$id];
    $response->getBody()->write(json_encode($user));
    return $response->withHeader('Content-Type', 'application/json');
});

$app->get('/api/users/{id}/secure', function (Request $request, Response $response, $args) use (&$users) {
    $requestedId = (int)$args['id'];
    $authUser = $request->getAttribute('user');

    // ✅ Only allow access to own user data
    if ($requestedId !== $authUser['id']) {
        $response->getBody()->write("Forbidden: You can't access other users' data.");
        return $response->withStatus(403);
    }

    $user = $users[$requestedId];
    $response->getBody()->write(json_encode($user));
    return $response->withHeader('Content-Type', 'application/json');
});


// ✅ Secure login with password_verify() + JWT
$app->post('/login', function (Request $request, Response $response) use ($users, $secretKey) {
    $params = (array)$request->getParsedBody();

    // Input validation from OWASP standardation
    if (!isset($params['username']) || !isset($params['password'])) {
        $response->getBody()->write("Username and password are required.");
        return $response->withStatus(400);
    }

    $username = trim($params['username']);
    $password = $params['password'];

    // Validate username length and characters
    if (strlen($username) < 3 || strlen($username) > 50 || !preg_match('/^[a-zA-Z0-9_]+$/', $username)) {
        $response->getBody()->write("Invalid username format.");
        return $response->withStatus(400);
    }

    // Validate password length
    if (strlen($password) < 8 || strlen($password) > 128) {
        $response->getBody()->write("Password must be between 8 and 128 characters.");
        return $response->withStatus(400);
    }

    // Check if user exists and password is correct
    foreach ($users as $user) {
        if ($user['username'] === $username && password_verify($password, $user['password'])) {
            $payload = [
                'sub' => $user['id'],
                'username' => $user['username'],
                'iat' => time(),
                'exp' => time() + 3600 // Token valid for 1 hour
            ];

            $token = JWT::encode($payload, $secretKey, 'HS256');

            $response->getBody()->write(json_encode([
                'message' => 'Login successful',
                'token' => $token,
            ]));
            return $response->withHeader('Content-Type', 'application/json');
        }
    }

    $response->getBody()->write("Invalid credentials");
    return $response->withStatus(401);
});

$app->run();