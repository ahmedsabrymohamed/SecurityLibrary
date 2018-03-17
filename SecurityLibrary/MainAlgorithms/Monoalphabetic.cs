using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
  
        String alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string Analyse(string plainText, string cipherText)
        {
            char[] arr = new char[26];
            for(int i = 0; i < plainText.Length; i++) { 
                int index = alpha.IndexOf(Char.ToUpper(plainText[i]));
                arr[index] = cipherText[i];
            }
            string UnStr = "";
            for(int i = 0; i < alpha.Length; i++)
            {
                if(cipherText.IndexOf(alpha[i]) == -1){
                    UnStr += Char.ToLower(alpha[i]);
                }
            }
            String str = "";
            int UnstrInd = 0;
            for(int i = 0; i < 26; i++)
            {
                if (arr[i] == '\0')
                    str += UnStr[UnstrInd++];
                else
                    str += Char.ToLower(arr[i]);
            }
        
            return str;
        }

        public string Decrypt(string cipherText, string key)
        {
            String str = "";
            for(int i = 0; i < cipherText.Length; i++)
            {
                int ciperIndex = key.IndexOf(Char.ToLower(cipherText[i]));
                str += alpha[ciperIndex];
            }
            return str;
        }

        public string Encrypt(string plainText, string key)
        {
            String str = "";
            for(int i = 0; i < plainText.Length; i++)
            {
                int plainIndex = alpha.IndexOf(Char.ToUpper(plainText[i]));
                str += key[plainIndex];
            }
            return str;
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        int []characters = new int[26];
        int[] sortCharacters = new int[26];
        char[] statistics = { 'e', 't', 'a', 'o', 'i', 'n', 's', 'r', 'h', 'l', 'd', 'c', 'u', 'm', 'f' , 'p', 'g', 'w', 'y', 'b', 'v', 'k', 'x', 'j', 'q', 'z' };
        public string AnalyseUsingCharFrequency(string cipher)
        { 
            alphaChar[] alpha = new alphaChar[26];
            List<char> mycharList = new List<char>();
            string str = "";
            for (int i = 0; i < cipher.Length; i++)
            {
                characters[Char.ToLower(cipher[i]) - 97]++;
            } 
            for(int i = 0; i < characters.Length; i++)
            {
                alphaChar a = new alphaChar((char)(i + 97), characters[i]);
                alpha[i] = a;
            }
            Array.Sort(alpha);               
            for(int i = 0; i < cipher.Length; i++)
            {
                char nextChar = Char.ToLower(cipher[i]);
                int countValue = characters[nextChar - 97];
                int at = 0;
                mycharList.Clear();
                for(int j = 0; j < 26; j++)
                {
                    if(alpha[j].getCount().Equals(countValue))
                    {       
                        mycharList.Add(alpha[j].getC());
                        at = j;
                    }   
                }
                char stre = addExactCharacter(statistics, mycharList, at, alpha);
                if (stre.Equals(' ')) continue;
                str += stre;
            }
            return str;
        }

        private char addExactCharacter(char[] statistics, List<char> mycharList, int at, alphaChar[] alpha)
        {
            mycharList.Sort();
            char c = mycharList[0];
            char returnedString = ' ';
            for(int i = at; i < 26; i++)
            {
                if (alpha[i].getC().Equals(c))
                {
                    returnedString = statistics[i];
                }
            }
            return returnedString;
        }

        public class alphaChar: IComparable<alphaChar>
        {
            char c;
            int count;
            public char getC()
            {
                return c;
            }
            public int getCount()
            {
                return count;
            }
          public alphaChar(char c , int count)
            {
                this.c = c;
                this.count = count;
            }
            public int CompareTo(alphaChar other)
            {
                return other.count - count;
            }
        }
    }
}
