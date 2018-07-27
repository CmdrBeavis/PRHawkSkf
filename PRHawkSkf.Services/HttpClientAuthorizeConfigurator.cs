using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace PRHawkSkf.Services
{
	/// <summary>
	/// Add appropriate Authorization header(s) to a given <see cref="HttpClient"/>
	/// instance.
	/// </summary>
	public class HttpClientAuthorizeConfigurator : IHttpClientAuthorizeConfigurator
	{
		private readonly IBase64Codec _base64Codec;


		/// <summary>
		/// Initializes a new instance of the 
		/// <see cref="HttpClientAuthorizeConfigurator"/> class.
		/// </summary>
		public HttpClientAuthorizeConfigurator(
			IBase64Codec base64CodecInst)
		{
			_base64Codec = base64CodecInst ?? throw new ArgumentNullException(nameof(base64CodecInst));
		}

		/// <summary>
		/// Adds the basic authorization header value.
		/// </summary>
		/// <param name="clientToUpdate">The client to update.</param>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">clientToUpdate</exception>
		/// <exception cref="ArgumentException">
		/// Parameter must not be null, empty or whitespace - userName
		/// or
		/// Parameter must not be null, empty or whitespace - password
		/// </exception>
		public bool AddBasicAuthorizationHeaderValue(
			HttpClient clientToUpdate,
			string userName,
			string password)
		{
			if (clientToUpdate == null)
			{
				throw new ArgumentNullException(nameof(clientToUpdate));
			}

			if (string.IsNullOrWhiteSpace(userName))
			{
				throw new ArgumentException(
					"Parameter must not be null, empty or whitespace",
					nameof(userName));
			}

			// Of course we could get really into it and allow the injection 
			// of a password complexity validator, but I'm afraid I've already
			// gotten too carried away.
			if (string.IsNullOrWhiteSpace(password))
			{
				throw new ArgumentException(
					"Parameter must not be null, empty or whitespace",
					nameof(password));
			}

			try
			{
				var encodedAuthParameter = _base64Codec.Encode(userName + ":" + password);

				// TODO: (?) I COULD check to see if the header value has already been added.

				// add our Auth Token to the Header
				clientToUpdate.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Basic", encodedAuthParameter);
			}
			catch (Exception e)
			{
				// TODO: probably want real logging here in ACTUAL production code (Trace.Writeline might do in the meantime.)
				Console.WriteLine(e);

				throw;
			}

			return true;
		}
	}
}
