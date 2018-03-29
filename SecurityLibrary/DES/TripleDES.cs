using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class TripleDES : ICryptographicTechnique<string, List<string>>
    {
        public string Decrypt(string cipherText, List<string> key)
        {
            DES algorithm = new DES();
            string plainText1 = algorithm.Decrypt(cipherText, key[0]);
            string plainText2 = algorithm.Encrypt(plainText1, key[1]);
            plainText1 = algorithm.Decrypt(plainText2, key[0]);
            return plainText1;
        }

        public string Encrypt(string plainText, List<string> key)
        {
            DES algorithm = new DES();
            string cipher1 = algorithm.Encrypt(plainText, key[0]);
            string cipher2 = algorithm.Decrypt(cipher1, key[1]);
            cipher1 = algorithm.Encrypt(cipher2, key[0]);
            return cipher1;
        }

        public List<string> Analyse(string plainText,string cipherText)
        {
            throw new NotSupportedException();
        }

    }
}
