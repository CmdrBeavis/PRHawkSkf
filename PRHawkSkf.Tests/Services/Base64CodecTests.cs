using System;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PRHawkSkf.Services;


namespace PRHawkSkf.Tests.Services
{
	[TestClass]
	public class Base64CodecTests
	{
		[TestMethod]
		public void Base64CodecInstantiation_CreatesValidInstance_ShouldPass()
		{
			// Arrange
			var base64Codec = new Base64Codec();

			// Assert
			Assert.IsNotNull(base64Codec);
			Assert.IsInstanceOfType(base64Codec, typeof(Base64Codec));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Base64CodecEncodeCall_WithNullOrWhiteSpParam_ShouldThrow()
		{
			// Arrange
			var base64Codec = new Base64Codec();

			// Act
			var encodedText = base64Codec.Encode(" ");

			// should have thrown
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Base64CodecDecodeCall_WithNullOrWhiteSpParam_ShouldThrow()
		{
			// Arrange
			var base64Codec = new Base64Codec();

			// Act
			var encodedText = base64Codec.Decode(" ");

			// should have thrown
		}

		[TestMethod]
		public void Base64Codec_GivenValidStringEncodeMethodWorks_ShouldPass()
		{
			// Arrange
			var base64Codec = new Base64Codec();
			string expectedText = "Hello .NET Nuts and bolts";

			// Act
			var encodedText = base64Codec.Encode(expectedText);

			// is it a Base64 string
			encodedText = encodedText.Trim();
			bool isBase64 = (!string.IsNullOrWhiteSpace(encodedText) &&
			                 encodedText.Length % 4 == 0 &&
			                 Regex.IsMatch(encodedText, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None));

			// Assert
			Assert.IsTrue(isBase64);
		}

		[TestMethod]
		public void Base64Codec_GivenBase64StringDecodeMethodWorks_ShouldPass()
		{
			// Arrange
			var base64Codec = new Base64Codec();
			string expectedText = "Hello .NET Nuts and bolts";

			// Act
			var encodedText = base64Codec.Encode(expectedText);
			var decodedText = base64Codec.Decode(encodedText);

			// Assert
			Assert.AreEqual(decodedText, expectedText);
		}
	}
}
