using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        char[,] arrr;

        public int Analyse(string plainText, string cipherText)
        {
           cipherText = cipherText.ToLower();
            
            for(int i =  1; i <= 10000; i++)
            {
                String cip = Encrypt(plainText, i);
                cip = cip.ToLower();
               
                if (cip.Equals(cipherText))
                {
                    return i;
                }

            }
            return 0;
        }
    
        public string Decrypt(string cipherText, int key)
        {
            int x = 0;
            string str = "";
            Encrypt(cipherText, key);
            for (int i = 0; i < key; i++)
            {
                for(int j = 0;j < cipherText.Length; j++)
                {
                    if(arrr[i,j] != '\0' && x < cipherText.Length)
                    {
                        arrr[i, j] = cipherText[x++];
                    }
                }
            }

            for(int i = 0; i < cipherText.Length; i++)
            {
                for(int j = 0; j < key; j++)
                {
                    if (arrr[j, i] == '\0') break;
                    str += arrr[j, i];
                }
            }
            return str;
        }

        public string Encrypt(string plainText, int key)
        {

            arrr = new char[plainText.Length, plainText.Length];
            int x = 0;

            for(int i = 0; i < plainText.Length; i++)
            {
                for(int j = 0; j < key; j++)
                {
                    if (x == plainText.Length) break;
                       arrr[j , i] = plainText[x++];
                }
            }

            String ret = "";
            for(int i = 0; i < key; i++)
            {
                for(int j = 0; j < plainText.Length; j++)
                {
                    if (arrr[i, j] == '\0') continue;
                    ret += arrr[i, j];
                }
            }
            return ret;
        }

    }
}
