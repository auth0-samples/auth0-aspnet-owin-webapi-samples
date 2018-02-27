# Authentication

This sample demonstartes how you can validate an RS256 token in Auth0 by downloading the certificate for the public key and storing it locally as part of your project. The JWT middleware will then load and read this local certificate and use that to verify the signature for JSON Web Tokens.

## Running the example

In order to run the example you need to just start a server. What we suggest is doing the following:

1. Download the certificate for your Auth0 tenant from `https://YOUR_AUTH0_DOMAIN/cer` and save it as a file named `auth0.cer` in this project folder
1. Make sure `web.config` contains your credentials. You can find your credentials in the settings section of your Auth0 API.
1. Hit F5 to start local web development server.

Go to `http://localhost:58105/api/ping` and you'll see the app running :).