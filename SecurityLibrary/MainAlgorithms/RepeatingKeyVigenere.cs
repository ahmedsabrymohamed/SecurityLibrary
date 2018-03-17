using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        char[] c = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        char[,] arr = new char[26, 26];
        public void fill_arr()
        {
            for(int i = 0; i < 26; i++)
            {
                for(int j = 0; j< 26; j++)
                {
                    int asc = (i + j) % 26;
                    arr[i, j] = c[asc];
                }
            }
        }

        public String check(String plainText , String key)
        {
            int x = 0;
            while(key.Length < plainText.Length)
            {
                key += key[x++];
                x %= key.Length;
            }
            return key;
        }

        public string Analyse(string plainText, string cipherText)
        {
            fill_arr();
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            String str = "";
            for(int i = 0; i < plainText.Length; i++)
            {
                int from = plainText[i] - 97;
                for(int j = 0; j < 26; j++)
                {
                    if(arr[from , j].Equals(cipherText[i]))
                    {
                        str += c[j];
                    }
                }
            }
            String ret = "";
            for(int i = 1; i < str.Length; i++)
            {
                if(Encrypt(plainText , str.Substring(0 , i)).Equals(cipherText))
                {
                    return str.Substring(0, i);
                }
            }
            return "MEOG";
        }

        public string Decrypt(string cipherText, string key)
        {
            fill_arr();
            key = check(cipherText, key);
            String ret = "";
            cipherText = cipherText.ToLower();
            for (int i = 0; i < key.Length; i++)
            {
                int from = (key[i]) - 97;
                for (int j = 0; j < 26; j++)
                {
                    if (arr[from, j].Equals(cipherText[i]))
                    {
                        ret += c[j];
                    }
                }
              }
            return ret;
        }

            public string Encrypt(string plainText, string key)
        {
            fill_arr();
            key = check(plainText , key);
            String ret = "";
            for(int i = 0; i < plainText.Length; i++)
            {
                ret += arr[plainText[i] - 97, key[i] - 97];
            }
            return ret;
        }
    }
}