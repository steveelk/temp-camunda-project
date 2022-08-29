using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LAFDalApp.Admin
{
    public class PBKDF2SHA256
    {
        ////usage
        ////usage
        ////usage
        //string salt = PBKDF2SHA256.GenerateSalt(_minSaltSize, _maxSaltSize);
        //string finalPass = PBKDF2SHA256.PBKDF2SHA256GetString(_pbkdf2DkLen, password, salt, _pbkdf2IteractionCount);

        public static string GenerateSalt(int minSaltSize, int maxSaltSize)
        {
            byte[] saltBytes;

            // Generate a random number to determine the salt size.
            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);

            // Allocate a byte array, to hold the salt.
            saltBytes = new byte[saltSize];

            // Initialize the cryptographically secure random number generator.
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
                rng.Dispose();
            }

            return Convert.ToBase64String(saltBytes);
        }

        public static string PBKDF2SHA256GetString(int dklen, string password, string salt, int iterationCount)
        {
            byte[] dk = null;
            byte[] bytePassword = Encoding.Unicode.GetBytes(password);
            byte[] bytesalt = Encoding.Unicode.GetBytes(salt);

            using (var hmac = new System.Security.Cryptography.HMACSHA256(bytePassword))
            {
                int hashLength = hmac.HashSize / 8;

                if ((hmac.HashSize & 7) != 0)
                    hashLength++;

                int keyLength = dklen / hashLength;

                if ((long)dklen > (0xFFFFFFFFL * hashLength) || dklen < 0)
                    throw new ArgumentOutOfRangeException("dklen");

                if (dklen % hashLength != 0)
                    keyLength++;

                byte[] extendedkey = new byte[bytesalt.Length + 4];

                Buffer.BlockCopy(bytesalt, 0, extendedkey, 0, bytesalt.Length);

                using (var ms = new System.IO.MemoryStream())
                {
                    for (int i = 0; i < keyLength; i++)
                    {
                        extendedkey[bytesalt.Length] = (byte)(((i + 1) >> 24) & 0xFF);
                        extendedkey[bytesalt.Length + 1] = (byte)(((i + 1) >> 16) & 0xFF);
                        extendedkey[bytesalt.Length + 2] = (byte)(((i + 1) >> 8) & 0xFF);
                        extendedkey[bytesalt.Length + 3] = (byte)(((i + 1)) & 0xFF);

                        byte[] u = hmac.ComputeHash(extendedkey);

                        Array.Clear(extendedkey, bytesalt.Length, 4);
                        byte[] f = u;

                        for (int j = 1; j < iterationCount; j++)
                        {
                            u = hmac.ComputeHash(u);
                            for (int k = 0; k < f.Length; k++)
                            {
                                f[k] ^= u[k];
                            }
                        }

                        ms.Write(f, 0, f.Length);
                        Array.Clear(u, 0, u.Length);
                        Array.Clear(f, 0, f.Length);

                        u = f = null;
                    }

                    dk = new byte[dklen];
                    ms.Position = 0;
                    ms.Read(dk, 0, dklen);
                    ms.Position = 0;

                    for (long i = 0; i < ms.Length; i++)
                    {
                        ms.WriteByte(0);
                    }

                    Array.Clear(extendedkey, 0, extendedkey.Length);

                    extendedkey = null;
                    ms.Dispose();
                }

                hmac.Dispose();
            }

            return Convert.ToBase64String(dk);
        }
    }
}
