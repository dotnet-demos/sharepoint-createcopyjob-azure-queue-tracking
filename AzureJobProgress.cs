using System.Security.Cryptography;

class AzureJobProgress
    {
        public string Label { get; set; }
        public string JobId { get; set; }
        public string IV { get; set; }
        public string Content { get; set; }

        public string Decrypt(byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = Convert.FromBase64String(IV);
                aes.Mode = CipherMode.CBC;
                using (ICryptoTransform decipher = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Content)))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decipher, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }