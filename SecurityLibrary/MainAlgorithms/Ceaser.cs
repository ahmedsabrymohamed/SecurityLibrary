using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        String alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int num = 26;
        public string Encrypt(string plainText, int key)
        {   
            String ans = "";
            for(int i = 0; i < plainText.Length; i++)
            {
                int index = alpha.IndexOf(Char.ToUpper(plainText[i]));
                ans += alpha[(index + key) % 26];
            }
            return ans;
        }

        public string Decrypt(string cipherText, int key)
        {
            String ans = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                int index = alpha.IndexOf(Char.ToUpper(cipherText[i]));
                ans += alpha[((index - key) + num) % 26];
            }
            return ans;
           
        }

        public int Analyse(string plainText, string cipherText)
        {
            int plainIndex = alpha.IndexOf(char.ToUpper(plainText[0]));
            int ciperIndex = alpha.IndexOf(char.ToUpper(cipherText[0]));
            int key = 0;
            if (plainIndex <= ciperIndex)
                key = Math.Abs(plainIndex - ciperIndex);
            else
                key = 25 - ciperIndex;
            return key;
        }
    }
}
