using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }


        public static List<int> Transpose(List<int> Key)
        {
            List<int> matCo = new List<int>();
            if (Key.Count == 4)
            {
                matCo.Add(Key[0]);
                matCo.Add(Key[2]);
                matCo.Add(Key[1]);
                matCo.Add(Key[3]);
                return matCo;
            }
            else
            {
                matCo.Add(Key[0]);
                matCo.Add(Key[3]);
                matCo.Add(Key[6]);
                matCo.Add(Key[1]);
                matCo.Add(Key[4]);
                matCo.Add(Key[7]);
                matCo.Add(Key[2]);
                matCo.Add(Key[5]);
                matCo.Add(Key[8]);
                return matCo;
            }

        }
        public static List<int> cofactor(List<int> key)
        {
            List<int> matCo = new List<int>();
            if (key.Count == 4)
            {
                int tmp = key[0];
                key[0] = key[3];
                key[3] = tmp;
                key[1] = -1 * key[1];
                key[2] = -1 * key[2];
                while (key[1] < 0)
                {
                    key[1] = key[1] + 26;
                }
                while (key[2] < 0)
                {
                    key[2] = key[2] + 26;
                }
                return key;
            }
            else if (key.Count == 9)
            {
                int a1 = key[4] * key[8] - key[5] * key[7];
                int a2 = -1 * (key[3] * key[8] - key[5] * key[6]);
                int a3 = key[3] * key[7] - key[4] * key[6];
                int a4 = -1 * (key[1] * key[8] - key[2] * key[7]);
                int a5 = key[0] * key[8] - key[6] * key[2];
                int a6 = -1 * (key[0] * key[7] - key[6] * key[1]);
                int a7 = key[1] * key[5] - key[4] * key[2];
                int a8 = -1 * (key[0] * key[5] - key[3] * key[2]);
                int a9 = key[4] * key[0] - key[3] * key[1];

                while (a1 < 0)
                {
                    a1 = a1 + 26;
                }

                while (a2 < 0)
                {
                    a2 = a2 + 26;
                }

                while (a3 < 0)
                {
                    a3 = a3 + 26;
                }
                while (a4 < 0)
                {
                    a4 = a4 + 26;
                }
                while (a5 < 0)
                {
                    a5 = a5 + 26;
                }
                while (a6 < 0)
                {
                    a6 = a6 + 26;
                }
                while (a7 < 0)
                {
                    a7 = a7 + 26;
                }
                while (a8 < 0)
                {
                    a8 = a8 + 26;
                }
                while (a9 < 0)
                {
                    a9 = a9 + 26;
                }
                matCo.Add(a1 % 26);
                matCo.Add(a2 % 26);
                matCo.Add(a3 % 26);
                matCo.Add(a4 % 26);
                matCo.Add(a5 % 26);
                matCo.Add(a6 % 26);
                matCo.Add(a7 % 26);
                matCo.Add(a8 % 26);
                matCo.Add(a9 % 26);
                return matCo;
            }
            else
                return null;
        }
        public static int det(List<int> key)
        {

            if (key.Count == 4)
            {
                return key[0] * key[3] - key[1] * key[2];
            }
            else if (key.Count == 9)
            {
                return key[0] * ((key[4] * key[8]) - (key[5] * key[7])) - key[1] * ((key[3] * key[8]) - (key[6] * key[5])) + key[2] * ((key[3] * key[7]) - (key[4] * key[6]));
            }
            else
                return 0;
        }

        public static int extended(int b)
        {
            int a1, a2, a3, b1, b2, b3, q, ta1, ta2, ta3, tb1, tb2, tb3;
            a1 = 1;
            a2 = 0;
            a3 = 26;
            b1 = 0;
            b2 = 1;
            b3 = b;
            q = 0;
            bool w = true;
            while (w)
            {
                q = a3 / b3;
                tb3 = b3;
                b3 = a3 % b3;
                ta1 = a1;
                ta2 = a2;
                a3 = tb3;
                a1 = b1;
                a2 = b2;
                b1 = ta1 - (q * b1);
                b2 = ta2 - q * b2;
                if (b3 == 1 || b3 == 0)
                    break;
            }
            int a = b2;
            while (a < 0)
            {
                a = a + 26;
            }
            a = a % 26;
            return a;
        }


        public List<int> Decrypt(List<int> cipher, List<int> key)
        {

            List<int> cipher2 = new List<int>();
            int detr = det(key);
            if (detr < 0)
            {
                while (detr < 0)
                {
                    detr = detr + 26;
                }
            }
            else
                detr = detr % 26;
            detr = extended(detr);
            key = cofactor(key);
            if (key.Count == 9)
                key = Transpose(key);


            if (key.Count == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    key[i] = (key[i] * detr) % 26;
                }
                for (int i = 0; i < cipher.Count; i += 2)
                {
                    int newC1 = cipher[i] * key[0] + cipher[i + 1] * key[1];
                    int newC2 = cipher[i] * key[2] + cipher[i + 1] * key[3];
                    while (newC1 < 0)
                    {
                        newC1 = newC1 + 26;
                    }

                    while (newC2 < 0)
                    {
                        newC2 = newC2 + 26;
                    }
                    cipher2.Add(newC1 % 26);
                    cipher2.Add(newC2 % 26);
                }

            }
            else if (key.Count == 9)
            {
                for (int i = 0; i < 9; i++)
                {
                    key[i] = (key[i] * detr) % 26;
                }

                for (int i = 0; i < cipher.Count; i += 3)
                {
                    int newC1 = cipher[i] * key[0] + cipher[i + 1] * key[1] + cipher[i + 2] * key[2];
                    int newC2 = cipher[i] * key[3] + cipher[i + 1] * key[4] + cipher[i + 2] * key[5];
                    int newC3 = cipher[i] * key[6] + cipher[i + 1] * key[7] + cipher[i + 2] * key[8];
                    cipher2.Add(newC1 % 26);
                    cipher2.Add(newC2 % 26);
                    cipher2.Add(newC3 % 26);
                }
            }
            return cipher2;
        }


        public List<int> Encrypt(List<int> plain, List<int> key)
        {
            List<int> cipher2 = new List<int>();
            if (key.Count == 4)
            {
                for (int i = 0; i < plain.Count; i += 2)
                {
                    int newC1 = plain[i] * key[0] + plain[i + 1] * key[1];
                    int newC2 = plain[i] * key[2] + plain[i + 1] * key[3];
                    cipher2.Add(newC1 % 26);
                    cipher2.Add(newC2 % 26);
                }
            }
            else if (key.Count == 9)
            {
                for (int i = 0; i < plain.Count; i += 3)
                {
                    int newC1 = plain[i] * key[0] + plain[i + 1] * key[1] + plain[i + 2] * key[2];
                    int newC2 = plain[i] * key[3] + plain[i + 1] * key[4] + plain[i + 2] * key[5];
                    int newC3 = plain[i] * key[6] + plain[i + 1] * key[7] + plain[i + 2] * key[8];
                    cipher2.Add(newC1 % 26);
                    cipher2.Add(newC2 % 26);
                    cipher2.Add(newC3 % 26);
                }
            }
            return cipher2;
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }

    }
}
