namespace PRHawkSkf.Services
{
	public interface IBase64Codec
	{
		/// <summary>
		/// Encodes the string specified in the <paramref name="dataToEncode"/>
		/// parameter.
		/// </summary>
		/// <param name="dataToEncode">
		/// The [string] data to encode.
		/// </param>
		/// <returns>
		/// The string specified in the <paramref name="dataToEncode"/> 
		/// parameter encoded as a Base64 string.
		/// </returns>
		string Encode(string dataToEncode);

		string Decode(string dataToDecode);
	}
}