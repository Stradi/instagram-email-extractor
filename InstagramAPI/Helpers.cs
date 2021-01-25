using System.Net;
using System.Text.Json;

using Sodium;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.Text;

namespace InstagramAPI {
  public static class Helpers {
    public static string SerializeCookieContainer(CookieContainer cookieContainer) {
      return JsonSerializer.Serialize(cookieContainer, typeof(CookieContainer));
    }

    public static CookieContainer DeserializeCookieContainer(string serialized) {
      return (CookieContainer)JsonSerializer.Deserialize(serialized, typeof(CookieContainer));
    }

    public static CookieContainer CloneCookieContainer(CookieContainer original, string url) {
      CookieContainer newCookieContainer = new CookieContainer();
      newCookieContainer.Add(original.GetCookies(new System.Uri(url)));

      return newCookieContainer;
    }

    public static string GenerateEncPassword(string password, string publicKey, string keyId, string version) {
      var time = DateTime.UtcNow.ToTimestamp(); // Unix timestamp
      var keyBytes = publicKey.HexToBytes(); // Convert a hex string to a byte array
      var key = new byte[32];
      new Random().NextBytes(key);
      var iv = new byte[12];
      var tag = new byte[16];
      var plainText = Encoding.UTF8.GetBytes(password); // ToBytes = Encoding.UTF8.GetBytes
      
      var cipherText = new byte[plainText.Length];

      using (var cipher = new AesGcm(key)) {
        cipher.Encrypt(nonce: iv,
          plaintext: plainText,
          ciphertext: cipherText,
          tag: tag,
          associatedData: Encoding.UTF8.GetBytes(time.ToString())); // GetBytes = Encoding.UTF8.GetBytes
      };

      var encryptedKey = SealedPublicKeyBox.Create(key, keyBytes);
      var bytesOfLen = BitConverter.GetBytes(((short)encryptedKey.Length)); // ToBytes = BitConverter.GetBytes(short);
      var info = new byte[] { 1, byte.Parse(keyId) };
      var bytes = info.Concat(bytesOfLen).Concat(encryptedKey).Concat(tag).Concat(cipherText); // Concat means that concat two array

      // expected: #PWD_INSTAGRAM_BROWSER:10:1595671654:ARBQAFWLYGkTT9UU0dyUCkaGTRFu0PH5Ph5s86DUAbZ+B9xon8cKmnqQGaUo7bB4NHCMKQRY69b9LwaJZ1rDw1OFM0LEGtI+KbDuDC0QnfJM6o1no0XPOl73RJoUZ/OfN5nE2q/IdqX0NFinS0faRf8=
      var str = $"#PWD_INSTAGRAM_BROWSER:{version}:{time}:{Convert.ToBase64String(bytes)}"; // ToBase64 = Convert.ToBase64String
      return str;
    }

    public static byte[] HexToBytes(this string hex) {
      return Enumerable.Range(0, hex.Length / 2)
        .Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16))
        .ToArray();
    }

    public static T[] Concat<T>(this T[] x, T[] y) {
      var z = new T[x.Length + y.Length];
      x.CopyTo(z, 0);
      y.CopyTo(z, x.Length);
      return z;
    }

    private static readonly DateTime _jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static long ToTimestamp(this DateTime d) {
      return (long)(d.ToUniversalTime() - _jan1St1970).TotalSeconds;
    }
  }
}