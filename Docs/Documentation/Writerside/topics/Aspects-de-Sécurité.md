# Aspects de S&#233;curit&#233;

## Authentification

Afin de pouvoir accéder à l'application en tant qu'administrateur, il faut s'authentifier. Pour cela, il faut entrer un nom d'utilisateur et un mot de passe. Si les informations sont correctes, l'utilisateur est redirigé vers la page d'accueil.
Dans le projet actuel, il y a un seul utilisateur avec les identifiants suivants :

- Nom d'utilisateur : `root`
- Mot de passe : `manager`

## Jwt (JSON Web Token)

Afin de sécuriser les requêtes, j'ai ajouté un système de token. Lors de l'authentification, un token est généré et stocké dans le local storage du navigateur. Ce token est ensuite envoyé dans le header de chaque requête. Si le token est valide, la requête est traitée, sinon une erreur est renvoyée.
Ceci est géré côté API, dans le fichier `Program.cs` et grâce au controller `AuthController`.

## Communication sécurisée

Afin de sécuriser les communications entre le client et le serveur, j'ai mis en place le protocole HTTPS.
J'ai également ajouté des politiques CORS directement depuis Azure pour limiter les requêtes à l'API.
