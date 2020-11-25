﻿using GraphML.Common;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using System.Configuration;

namespace GraphML.API.Server
{
	public sealed class RestClientFactory : IRestClientFactory
	{
		private readonly string ApiUri;

		private readonly string UserName;
		private readonly string Password;

		public RestClientFactory(IConfiguration config)
		{
			// read out of user secret or environment
			ApiUri = config.API_URI();
			UserName = config.API_USERNAME();
			Password = config.API_PASSWORD();

			if (string.IsNullOrWhiteSpace(ApiUri) ||
			  string.IsNullOrWhiteSpace(UserName) ||
			  string.IsNullOrWhiteSpace(Password)
			  )
			{
				throw new ConfigurationErrorsException("Missing API configuration - check UserSecrets or environment variables");
			}
		}

		public IRestClient GetRestClient()
		{
			var client = new RestClient(ApiUri)
			{
				Authenticator = new HttpBasicAuthenticator(UserName, Password),
				RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
			};

			return client;
		}
	}
}
