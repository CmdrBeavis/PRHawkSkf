
using System.Configuration;


namespace PRHawkSkf.Services
{
	public class WebConfigReader : IWebConfigReader
	{
		/// <summary>
		/// Holds onto a reference of the <c>AppSettingsReader</c> class.
		/// </summary>
		private readonly AppSettingsReader _appSettingsReader;

		/// <summary>
		/// Initializes a new instance of the <see cref="WebConfigReader"/> class.
		/// </summary>
		public WebConfigReader()
		{
			// AppSettingsReader is a system library thing (with no interface), 
			// so not sure if there would be any point to injecting it into 
			// this class.
			_appSettingsReader = new AppSettingsReader();
		}

		/// <summary>
		/// Gets the specified appSettings value.
		/// </summary>
		/// <typeparam name="T">
		/// The Type of value to return.
		/// </typeparam>
		/// <param name="keyName">
		/// A string containing the name of the appSettings key, who's value 
		/// is to be returned.
		/// </param>
		/// <returns>
		/// A value of Type T.
		/// </returns>
		public T GetAppSetting<T>(string keyName)
		{
			var result = (T) _appSettingsReader.GetValue(keyName, typeof(T));

			return result;
		}
	}
}
