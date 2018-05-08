using System.Security.Cryptography;
using System.Text;

namespace PBCore
{

    public static class EncryptionUtils
    {

        public const string defaultPW = "H@rU^@";
        public const string defaultSalt = "@ShitERu";

        /// <summary>
        /// 异步加密字符串
        /// </summary>
        /// <param name="src"></param>
        /// <param name="onComplete"></param>
        /// <param name="pw"></param>
        /// <param name="salt"></param>
        public static void EncryptorAsync(string src, System.Action<string> onComplete, string pw = null, string salt = null)
        {
            Threading.Loom.RunAsync(() =>
            {
                string data = Encryptor(src, pw, salt);
                Threading.Loom.QueueOnMainThread(() =>
                {
                    if (onComplete != null)
                        onComplete.Invoke(data);
                });
            });
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Encryptor(string src, string pw = null, string salt = null)
        {
            RijndaelManaged rjdl = InitRijndael(pw, salt);
            ICryptoTransform encryptor = rjdl.CreateEncryptor();
            byte[] srcBytes = Encoding.ASCII.GetBytes(src);
            byte[] result = encryptor.TransformFinalBlock(srcBytes, 0, srcBytes.Length);
            encryptor.Dispose();
            return System.Convert.ToBase64String(result, 0, result.Length);
        }

        /// <summary>
        /// 异步解密字符串
        /// </summary>
        /// <param name="src"></param>
        /// <param name="onComplete"></param>
        /// <param name="pw"></param>
        /// <param name="salt"></param>
        public static void DecryptorAsync(string src, System.Action<string> onComplete, string pw = null, string salt = null)
        {
            Threading.Loom.RunAsync(() =>
            {
                string data = Decryptor(src, pw, salt);
                Threading.Loom.QueueOnMainThread(() =>
                {
                    if (onComplete != null)
                        onComplete.Invoke(data);
                });
            });
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Decryptor(string src, string pw = null, string salt = null)
        {
            RijndaelManaged rjdl = InitRijndael(pw, salt);
            ICryptoTransform decryptor = rjdl.CreateDecryptor();
            byte[] srcBytes = System.Convert.FromBase64String(src);
            byte[] result = decryptor.TransformFinalBlock(srcBytes, 0, srcBytes.Length);
            decryptor.Dispose();
            return ASCIIEncoding.Default.GetString(result);
        }

        private static RijndaelManaged InitRijndael(string pw, string salt)
        {
            if (string.IsNullOrEmpty(pw))
                pw = defaultPW;
            if (string.IsNullOrEmpty(salt))
                salt = defaultSalt;

            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.KeySize = 192;
            rijndael.BlockSize = 128;

            byte[] bSalt = Encoding.UTF8.GetBytes(salt);
            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(pw, bSalt);
            deriveBytes.IterationCount = 404;
            rijndael.Key = deriveBytes.GetBytes(rijndael.KeySize / 8);
            rijndael.IV = deriveBytes.GetBytes(rijndael.BlockSize / 8);
            return rijndael;
        }

    }
}