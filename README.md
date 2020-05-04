# Azure AD B2C SAML tester web app

## Acknowledgements
This code is based on the [SAMLTester app](https://github.com/azure-ad-b2c/saml-sp-tester).

## Purpose
This application requests a SAML token from B2C using SAML SSO profile, receives it and displays its contents as an xml file. There is **no token validation** included in this app.

## Structure
Logic for token request is embedded in the *Index.cshtml.cs* file. The *SendAsync* method constructs an AuthnRequest and redirects the user's browser to B2C. The request contents are based on the configuration provided in the *appSettings.json* file. The *OnPost* method receives the returned SAML response, deserializes it and provides it to the html page for display to the user.

## Setup
B2C policy files used to configure a B2C tenant to handle SAML requests are provided in the *Policies* folder. The folder also contains
the *conf.json* file. That file needs to be pre-populated with values specific to the target tenant. The [Upload-IEFPolicies script](https://github.com/mrochon/b2cief-upload) may be then
used to upload thyese policies to a particular B2C directory.

*appSettings.json* file's *SAMLProvider* section must be populated with the name of the B2C tenant and journeys used for SAML redirection.

For more information, see [Register a SAML application in Azure AD B2C](https://docs.microsoft.com/azure/active-directory-b2c/connect-with-saml-service-providers)