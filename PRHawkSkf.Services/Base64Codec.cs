using System;
using System.Diagnostics;
using System.Text;


namespace PRHawkSkf.Services
{
	public class Base64Codec : IBase64Codec
	{
		/// <summary>
		/// Encodes the string specified in the <paramref name="dataToEncode"/>
		/// parameter.
		/// </summary>
		/// <param name="dataToEncode">
		/// The [string] data to encode. If this string is 'null or 
		/// whitespace,' it is simply returned.
		/// </param>
		/// <returns>
		/// The string specified in the <paramref name="dataToEncode"/> 
		/// parameter encoded as a Base64 string.
		/// </returns>
		public string Encode(string dataToEncode)
		{
			string result;

			if (string.IsNullOrWhiteSpace(dataToEncode))
			{
				return dataToEncode;
			}

			try
			{
				byte[] plainTextBytes = Encoding.UTF8.GetBytes(dataToEncode);

				result = Convert.ToBase64String(plainTextBytes);
			}
			catch (EncoderFallbackException eFbEx)
			{
				// TODO: At least put in a Trace.Writeline() and write to the event log
				// in a 'real' app, proper logging should be put here.
				Debug.WriteLine(eFbEx);

				throw;
			}
			catch (Exception oEx)
			{
				// Same as above
				Debug.WriteLine(oEx);

				throw;
			}

			return result;
		}

		/// <summary>
		/// Decodes the Base64 encoded string specified in the 
		/// <paramref name="dataToDecode"/> parameter.
		/// </summary>
		/// <param name="dataToDecode">
		/// The data to decode. If this string is 'null or whitespace,' 
		/// it is simply returned.
		/// </param>
		/// <returns>
		/// The decoded version of the Base64 encoded string specified in 
		/// the <paramref name="dataToDecode"/> parameter.
		/// </returns>
		public string Decode(string dataToDecode)
		{
			string result;

			if (string.IsNullOrWhiteSpace(dataToDecode))
			{
				return dataToDecode;
			}

			try
			{
				var encodedTextBytes = Convert.FromBase64String(dataToDecode);

				result = Encoding.UTF8.GetString(encodedTextBytes);
			}
			catch (EncoderFallbackException eFbEx)
			{
				// TODO: At least put in a Trace.Writeline() and write to the event log
				// in a 'real' app, proper logging should be put here.
				Debug.WriteLine(eFbEx);

				throw;
			}
			catch (Exception oEx)
			{
				// Same as above
				Debug.WriteLine(oEx);

				throw;
			}

			return result;
		}
	}
}
