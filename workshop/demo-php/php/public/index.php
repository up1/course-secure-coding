<?php

require dirname(__DIR__) . '/vendor/autoload.php';

use Psr\Http\Message\ResponseInterface as Response;
use Psr\Http\Message\ServerRequestInterface as Request;
use Slim\Factory\AppFactory;

$app = AppFactory::create();

$users = [
    1 => ['id' => 1, 'name' => 'Alice', 'email' => 'alice@example.com'],
    2 => ['id' => 2, 'name' => 'Bob', 'email' => 'bob@example.com'],
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

$app->run();