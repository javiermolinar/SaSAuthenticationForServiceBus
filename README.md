# SaSAuthenticationForServiceBus
Example of how to authenticate on Service Bus using a SaS token

#What is a SAS Token

Is a token provided by clients to claim access to Service Bus resources.

#How to generate it

SharedAccessSignature sig=<signature-string>&se=<expiry>&skn=<keyName>&sr=<URL-encoded-resourceURI>

A SAS token is valid for all resources under the <resourceURI>

