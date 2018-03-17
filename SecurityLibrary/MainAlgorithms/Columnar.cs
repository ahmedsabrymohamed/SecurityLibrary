using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public int getMaximum(List<int> mylist)
        {
            int maxi = 0;
            for(int i = 0;i < mylist.Count; i++)
            {
                maxi = Math.Max(maxi, mylist[i]);
            }
            return maxi;
        }

        public int findIndex(List<int> lis , int key)
        {
            for(int i = 0; i < lis.Count; i++)
            {
                if(lis[i] == key)
                {
                    return i;
                }
            }
            return -1;
        }

        public string get(int rows, int c, char[,] arr)
        {
            string str = "";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    if (arr[i, j] != '\0')
                        str += arr[i, j];

                }
            }
            return str;
        }

        public List<int> Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }
      

        public string Decrypt(string cipherText, List<int> key)
        {
            int c = 0, x = 0, r = 0;
            bool at = false;
            c = getMaximum(key);
            int div = cipherText.Length / c;
            if (cipherText.Length % c != 0) {
                r = (cipherText.Length / c) + 1; at = true;}
            else
                r = cipherText.Length / c;
            
            char[,] myarr = new char[r, c];
            int[] arr = new int[key.Count];
            for (int i = 0; i < key.Count; i++) arr[i] = key.IndexOf(i + 1);
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < r && x < cipherText.Length; j++)
                {
                    if (at && j == (r - 1) && i > div)
                         myarr[j, arr[i]] = '\0';
                    else
                         myarr[j, arr[i]] = cipherText[x++];
                }
            }
           return  get(r, c, myarr);       
        }

        public string Encrypt(string plainText, List<int> key)
        {
            int maxi = getMaximum(key);
            char[,] arr = new char[maxi, maxi];
            int x = 0;
            string ret = "";
            for (int i = 0; i < maxi; i++)
            { 
                 for(int j = 0; j < maxi && x < plainText.Length; j++)
                {
                    
                    arr[i, j] = plainText[x++];
                }
            }
            for(int i = 0; i < key.Count; i++)
            {

                int next = findIndex(key , i+1);
                for(int j = 0;j < maxi; j++)
                {
                    if(arr[j , next] != '\0')
                    {
                        ret += arr[j, next];
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return ret;
        }
    }
}
