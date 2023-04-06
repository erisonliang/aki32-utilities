using System.Data;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    /// <summary>
    /// generate hash
    /// </summary>
    /// <param name="inputPath"></param>
    /// <param name="newFileNameWithoutExtension">"*" will be replaced with old name</param>
    /// <param name="replaceSets">replace strings in original file name</param>
    /// <returns></returns>
    public static string GenerateHash(this string inputString,
        bool usePBKDF2 = true,
        string salt = "",
        int SALT_SIZE = 24,
        int PBKDF2_ITERATION = 10000
        )
    {
        // main

        // generate salt
        if (string.IsNullOrEmpty(salt))
        {
            // obsolete. 
            // var buff = new byte[SALT_SIZE];
            //using (var rng = new RNGCryptoServiceProvider())
            //    rng.GetBytes(buff);
            //salt = Convert.ToBase64String(buff);

            // use this instead.
            var buff = RandomNumberGenerator.GetBytes(SALT_SIZE);
            salt = Convert.ToBase64String(buff);
        }

        // generate hash
        string hash = "";
        if (usePBKDF2)
        {
            var encoder = new UTF8Encoding();
            var b = new Rfc2898DeriveBytes(inputString, encoder.GetBytes(salt), PBKDF2_ITERATION);
            var k = b.GetBytes(32);
            hash = Convert.ToBase64String(k);
        }
        else
        {
            var saltAndPwd = string.Concat(inputString, salt);
            var encoder = new UTF8Encoding();
            var buffer = encoder.GetBytes(saltAndPwd);
            using var csp = SHA256.Create();
            var hashBytes = csp.ComputeHash(buffer);
            hash = Convert.ToBase64String(hashBytes);
        }

        return hash;
    }

}
