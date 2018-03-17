using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }
        int[] chars = new int[26];
        char[,] arr = new char[5, 5];
        class point
        {
            int i, j;
            public point() { }
            public point(int i, int j)
            {
                this.i = i;
                this.j = j;
            }
            public int getI() { return i; }
            public int getJ() { return j; }
        }

        /*
         *  a function to find the position of the char in arr[5][5] and return
         *  an object of Point class.
         */
        point find(char mychar)
        {
            point p = new point();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (arr[i, j] == mychar)
                    {
                        // char is found , return it.
                        return p = new point(i, j);
                    }
                }
            }
            return p;
        }

        // this function construct arr[5][5] based on PlayFair algorithm..
        public void construct_array(String key)
        {

            int x = 0;
            bool outside = false;
            // adjust the array..
            int i = 0, j = 0;
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    if (x < key.Length && chars[key[x] - 97] == 0)
                    {
                        if (key[x] == 'i')
                        {
                            chars[9] = 1; /// not i and j must be in one cell in matrix
                        }
                        arr[i, j] = key[x];
                        chars[key[x++] - 97] = 1;

                        if (x == key.Length)
                        {
                            outside = true;
                            j++;
                            break;
                        }
                    }
                    else if (chars[key[x++] - 97] == 1)
                        j--;
                    else
                        x++;

                }
                if (outside)
                    break;
            }
            for (int k = 0; k < 26; k++)
            {
                if (k == 9) continue;
                if (chars[k] == 0)
                {
                    int nextChar = 65 + k;
                    arr[i % 5, j % 5] = Char.ToLower((char)nextChar);
                    if (j % 5 == 4)
                        i++;
                    j++;
                }
            }
        }
        public string Decrypt(string cipherText, string key)
        {
              construct_array(key);    
              char current, next = ' ';
              String ret = "";
              for (int k = 0; k < cipherText.Length; k += 2)
              {
                  current = Char.ToLower(cipherText[k]);
                  if (k < cipherText.Length - 1)
                  {
                      if (cipherText[k + 1] == cipherText[k]){
                          next = 'x';
                          k--;
                      }
                      else{
                          next = cipherText[k + 1];
                      }
                  }
                  if (k + 1 == cipherText.Length){
                      next = 'x';
                      k--;
                  }
                  point x1 = find(Char.ToLower(current));
                  point x2 = find(Char.ToLower(next));

                  if (x1.getJ() == x2.getJ())
                  {
                      if(x1.getI() == 0){
                          ret += arr[4, x1.getJ()];
                      }
                      else {
                              ret += arr[x1.getI()-1, x1.getJ()];
                      }
                    
                      if(x2.getI() == 0){
                          ret += arr[4, x2.getJ()];
                      }
                      else{
                              ret += arr[(x2.getI() - 1), x2.getJ()];
                      }

                  }
                  else if (x1.getI() == x2.getI())
                  {
                      if (x1.getJ() == 0)
                    {
                              ret += arr[x1.getI(), 4];
                      }
                      else
                      {
                          if (arr[(x1.getI()), (x1.getJ() - 1)] != 'x')
                              ret += arr[(x1.getI()), (x1.getJ()- 1)];
                      }

                      if (x2.getJ() == 0)
                      {
                              ret += arr[x2.getI(), 4];
                      }
                      else
                      {
                              ret += arr[x2.getI(), (x2.getJ()- 1)];
                      }


                  }
                  else
                  {
                      if(arr[x2.getI(), x1.getJ()] == 'x')
                      {
                          ret += arr[x1.getI(), x2.getJ()];
                         // ret += 'x';
                          continue;
                      }

                     // if (arr[x1.getI(), x2.getJ()] != 'x')
                          ret += arr[x1.getI(), x2.getJ()];
                      if (arr[x2.getI(), x1.getJ()] != 'x')
                          ret += arr[x2.getI(), x1.getJ()];

                  }
              }

             return ret.ToUpper();
        }

        public string Encrypt(string plainText, string key)
        {
            
            // construct the arr[5][5] based on PlayFair algorithm.
            construct_array(key);
            char current, next = ' ';
            // this is the String that will contains the encrpted text.
            String ret = ""; 
            
          for (int k = 0; k < plainText.Length; k+=2)
          {
                current = plainText[k];
                if(k < plainText.Length - 1)
                {
                    // if there's 2 same adjacent characters seperate them by adding char 'x' in the middle.
                    if (plainText[k + 1] == plainText[k])
                    {
                        next = 'x';
                        k--;
                    }
                    else
                    {
                        next = plainText[k + 1];
                    }
                   
                }
                // if plainText.Length is odd so the last character with paired with char 'x'
                if(k + 1 == plainText.Length)
                {
                     next = 'x';
                    k--;
                }
                // the next 2 character positions in arr[5][5]
                point x1 = find(current);
                point x2 = find(next);

                // here's the 3 conditions of PlayFair algorithm
                if (x1.getJ() == x2.getJ())
                {
                    ret += arr[(x1.getI() + 1) % 5, x1.getJ() % 5];
                    ret += arr[(x2.getI() + 1) % 5, x2.getJ() % 5];
                }
                else if (x1.getI() == x2.getI())
                {
                    ret += arr[(x1.getI()), (x1.getJ() + 1) % 5];
                    ret += arr[x2.getI(), (x2.getJ() + 1) % 5];
                }
                else {
                    ret += arr[x1.getI(), x2.getJ()];
                    ret += arr[x2.getI(), x1.getJ()];
                }
            }

            return ret.ToUpper();

        }

    }
}
