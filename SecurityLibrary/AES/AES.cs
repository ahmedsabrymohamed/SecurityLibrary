using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            int[,] pTMatrix = hexToInt(cipherText);

            int[,] keyMatrix = hexToInt(key);
            
            int[][,] keyGenrated = new int[10][,];

            keyGenrated[0] = generateNewKey(keyMatrix, AES_Constants.S_BOX, 1);
            //view(keyGenrated[0]);

            for (int i = 1; i < 10; i++)
            {
                keyGenrated[i] = generateNewKey(keyGenrated[i - 1], AES_Constants.S_BOX, i + 1);
            }

            pTMatrix = addRoundKey(pTMatrix, keyGenrated[9]);

            for (int i = 8; i >= 0; i--)
            {

                pTMatrix = shiftRowsInvers(pTMatrix);

                pTMatrix = substituteRound(pTMatrix, AES_Constants.S_BOX_INVERS);
                pTMatrix = addRoundKey(pTMatrix, keyGenrated[i]);
                pTMatrix = mixColumns(pTMatrix, AES_Constants.MIX_INV);

            }
            pTMatrix = shiftRowsInvers(pTMatrix);
            pTMatrix = substituteRound(pTMatrix, AES_Constants.S_BOX_INVERS);
            keyMatrix = hexToInt(key);
            pTMatrix = addRoundKey(pTMatrix, keyMatrix);
            string res = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string e = pTMatrix[j, i].ToString("X");
                    if (e.Length < 2)
                        e = "0" + e;
                    res += e;
                }
            }
            System.Console.WriteLine(res);
            return "0x" + res;

        }

          public override string Encrypt(string plainText, string key)
        {
            int[,] cTMatrix = hexToInt(plainText);
            int[,] keyMatrix = hexToInt(key);
            
            cTMatrix = addRoundKey(cTMatrix, keyMatrix);
            for(int i=0;i<9;i++)
            {
                cTMatrix = substituteRound(cTMatrix, AES_Constants.S_BOX);
                cTMatrix = shiftRows(cTMatrix);
                cTMatrix = mixColumns(cTMatrix,AES_Constants.MIX);
                keyMatrix = generateNewKey(keyMatrix,AES_Constants.S_BOX,i+1);
                cTMatrix = addRoundKey(cTMatrix, keyMatrix);
            }
            cTMatrix = substituteRound(cTMatrix, AES_Constants.S_BOX);
            cTMatrix = shiftRows(cTMatrix);
            keyMatrix = generateNewKey(keyMatrix, AES_Constants.S_BOX, 10);
            cTMatrix = addRoundKey(cTMatrix, keyMatrix);
           
            string res = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string e=cTMatrix[j,i].ToString("X");
                    if (e.Length < 2)
                        e = "0" + e;
                    res += e;
                }
            }
           
            return "0x"+res;
        }
        private static int[,] substituteRound(int[,] kXorPt, int[,] subsMatrix)
        {

            int[,] res = new int[4, 4];
             for (int i = 0; i < 4; i++)
                 for (int j = 0; j < 4; j++)    
                     res[i, j] = subsMatrix[((kXorPt[i, j] & 240)>>4), (kXorPt[i, j] & 15)];
                 
             return res;
        }

        private static int[,] shiftRows(int[,] m)
        {
            int temp;
            for (int i = 0; i < 4; i++)
            {
               
                for (int j = 0; j < i; j++)
                {
                    temp = m[i, 0];
                    for (int k = 1; k <4; k++)
                    {
                         m[i, k-1]=m[i, k ];
                    }
                    m[i, 3] = temp;
                }
                


            }
            return m;
        }

       
        private static int[,] mixColumns(int[,] stateMatrix, int[,] constMatrix)
        {
            int[,] res = new int[4, 4];
            int e1 = 0;
            int e2 = 0;
            int fe = 0;



            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    fe = 0;
                    for (int k = 0; k < 4; k++)
                    {

                        e1 = stateMatrix[k, i];
                        e2 = constMatrix[j, k];
                        if (e2 == 0 || e1 == 0)
                        {
                            e1 = 0;
                        }
                        else if (e1 == 0x1)
                        {
                            e1 = e2;
                        }
                        else if (e2 != 0x1)
                        {
                            e1 = AES_Constants.L[((e1 & 240) >> 4), (e1 & 15)];
                            e2 = AES_Constants.L[((e2 & 240) >> 4), (e2 & 15)];
                            e1 = (e1 + e2);
                            if (e1 > 0xFF)
                            {
                                e1 -= 0xFF;
                            }
                            e1 = AES_Constants.E[((e1 & 240) >> 4), (e1 & 15)];
                        }
                        fe ^= e1;

                    }
                    res[j, i] = fe;
                }
            return res;
        }
        private static int[,] addRoundKey(int[,] cT, int[,]  key)
        {
            
            int[,] kXorPt = new int[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    kXorPt[i, j] = cT[i, j] ^ key[i, j];
               
            return kXorPt;
           
        }
        
        private static int[,] hexToInt(string hex)
        {
            int[,] res = new int[4, 4];
            int counter = 0;
            hex = hex.Substring(2);
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++, counter += 2)
                    res[j, i] = Convert.ToInt32(hex.Substring(counter, 2), 16);

            return res;
        }
        private static void view(int[,] m)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    System.Console.Write(m[i, j].ToString("X") + " ");
                System.Console.WriteLine();
            }
        }

       

        private  int[,] generateNewKey(int[,] keyMatrix, int[,] subsMatrix, int roundNum)
        {
            int[,] newKeyMatrix = new int[4, 4];
            int[] lastCol = { keyMatrix[0, 3], keyMatrix[1, 3], keyMatrix[2, 3], keyMatrix[3, 3] };
            int temp = lastCol[0];
            for (int i = 1; i < 4; i++)
                lastCol[i - 1] = lastCol[i];
            lastCol[3] = temp;
            for (int i = 0; i < 4; i++)
                lastCol[i] = subsMatrix[((lastCol[i] & 240) >> 4), (lastCol[i] & 15)];
            lastCol[0] = lastCol[0] ^ AES_Constants.RCON[roundNum];
            for (int i = 0; i < 4; i++)
                newKeyMatrix[i, 0] = keyMatrix[i, 0] ^ lastCol[i];
            // view(keyMatrix);
            // System.Console.WriteLine();
            for (int i = 1; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    newKeyMatrix[j, i] = newKeyMatrix[j, i - 1] ^ keyMatrix[j, i];
            // view(keyMatrix);
            return newKeyMatrix;
        }
        private  int[,] shiftRowsInvers(int[,] m)
        {
            int temp;
            for (int i = 0; i < 4; i++)
            {

                for (int j = 0; j < i; j++)
                {
                    temp = m[i, 3];
                    for (int k = 2; k >= 0; k--)
                    {
                        m[i, k + 1] = m[i, k];
                    }
                    m[i, 0] = temp;
                }



            }

            return m;
        }

        
    
    }
}
