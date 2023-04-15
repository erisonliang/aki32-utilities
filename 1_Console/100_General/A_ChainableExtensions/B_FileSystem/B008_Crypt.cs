using System.Security.Cryptography;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{
    // ref: https://qiita.com/hibara/items/6c7476ceec5a9caf2e81

    // ★★★★★★★★★★★★★★★ main

    /// <summary>
    /// Encrypt file
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="password">password for encryption</param>
    /// <returns></returns>
    public static FileInfo Encrypt(this FileInfo inputFile, FileInfo? outputFile, string password)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);


        // main
        var deriveBytes = new Rfc2898DeriveBytes(password, 8, 1000);
        var salt = deriveBytes.Salt;
        var key = deriveBytes.GetBytes(32);
        var iv = deriveBytes.GetBytes(16);

        using var infs = new FileStream(inputFile.FullName, FileMode.Open, FileAccess.Read);
        using var outfs = new FileStream(outputFile!.FullName, FileMode.Create, FileAccess.Write);

        outfs.Write(salt, 0, 8);

        using var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Key = key;
        aes.IV = iv;

        using ICryptoTransform encryptor = aes.CreateEncryptor();
        using var cs = new CryptoStream(outfs, encryptor, CryptoStreamMode.Write);

        var buffer = new byte[1024];
        int len;
        while ((len = infs.Read(buffer, 0, buffer.Length)) > 0)
            cs.Write(buffer, 0, len);


        // post process
        return outputFile!;
    }

    /// <summary>
    /// Decrypt file
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="outputFile">when null, automatically set</param>
    /// <param name="password">password for decryption</param>
    /// <returns></returns>
    public static FileInfo Decrypt(this FileInfo inputFile, FileInfo? outputFile, string password)
    {
        // preprocess
        UtilPreprocessors.PreprocessOutFile(ref outputFile, inputFile.Directory!, inputFile.Name);


        // main
        using var infs = new FileStream(inputFile.FullName, FileMode.Open, FileAccess.Read);
        byte[] salt = new byte[8];
        infs.Read(salt, 0, 8);
        var deriveBytes = new Rfc2898DeriveBytes(password, salt, 1000);

        byte[] key = deriveBytes.GetBytes(32);
        byte[] iv = deriveBytes.GetBytes(16);

        using var outfs = new FileStream(outputFile!.FullName, FileMode.Create, FileAccess.Write);

        var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.KeySize = 256;
        aes.BlockSize = 128;
        aes.Key = key;
        aes.IV = iv;

        using ICryptoTransform decryptor = aes.CreateDecryptor();
        using var cs = new CryptoStream(infs, decryptor, CryptoStreamMode.Read);

        byte[] buffer = new byte[1024];
        int len;
        while ((len = cs.Read(buffer, 0, buffer.Length)) > 0)
            outfs.Write(buffer, 0, len);


        // post process
        return outputFile!;
    }

    // ★★★★★★★★★★★★★★★ loop sugar

    /// <summary>
    /// Encrypt
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="password">password for encryption</param>
    /// <returns></returns>
    public static DirectoryInfo Encrypt_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, string password)
        => inputDir.Loop(outputDir, (inF, outF) => inF.Encrypt(outF, password));

    /// <summary>
    /// Decrypt
    /// </summary>
    /// <param name="inputDir"></param>
    /// <param name="outputDir">when null, automatically set</param>
    /// <param name="password">password for decryption</param>
    /// <returns></returns>
    public static DirectoryInfo Decrypt_Loop(this DirectoryInfo inputDir, DirectoryInfo? outputDir, string password)
        => inputDir.Loop(outputDir, (inF, outF) => inF.Decrypt(outF, password));


    // ★★★★★★★★★★★★★★★

}
