using System.Security.Cryptography;

namespace AuthenticationAPI.Services
{
	public class PasswordUtils
	{
		public static string HashPassword(string password)
		{

			byte[] salt = new byte[32];
			RandomNumberGenerator rng = RandomNumberGenerator.Create();
			rng.GetBytes(salt);

			byte[] hash = HashPasswordWithSalt(password, salt, 10000, 256);
			byte[] allBytes = new byte[32 + hash.Length];
			Buffer.BlockCopy(salt, 0, allBytes, 0, 32);
			Buffer.BlockCopy(hash, 0, allBytes, 32, hash.Length);
			string result = Convert.ToBase64String(allBytes);
			return result;
		}

		private static byte[] HashPasswordWithSalt(string password, byte[] salt, int iterations, int hashByteSize)
		{
			Rfc2898DeriveBytes pbkdf2 = new(password, salt, iterations, HashAlgorithmName.SHA256);
			return pbkdf2.GetBytes(hashByteSize);
		}

		public static bool VerifyPassword(string password, string storedPassword)
		{
			byte[] allBytes = Convert.FromBase64String(storedPassword);
			byte[] salt = new byte[32];
			Buffer.BlockCopy(allBytes, 0, salt, 0, 32);
			byte[] hashBytes = new byte[allBytes.Length - 32];
			Buffer.BlockCopy(allBytes, 32, hashBytes, 0, hashBytes.Length);
			byte[] newHash = HashPasswordWithSalt(password, salt, 10000, hashBytes.Length);

			return SlowEquals(hashBytes, newHash);
		}

		private static bool SlowEquals(byte[] a, byte[] b)
		{
			uint diff = (uint)a.Length ^ (uint)b.Length;
			for (int i = 0; i < a.Length && i < b.Length; i++)
			{
				diff |= (uint)(a[i] ^ b[i]);
			}
			return diff == 0;
		}


		//}
		//public class PasswordUtils
		//{
		//	static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		//	{
		//		using (var hmac = new HMACSHA512())
		//		{
		//			passwordSalt = hmac.Key;
		//			passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		//		}
		//	}
		//}
	}
}
