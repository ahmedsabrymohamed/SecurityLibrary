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
    //Hexadecimal Numbers
 
    public class DES : CryptographicTechnique
    {
        Dictionary<char, Int64> hexBase;
        public override string Decrypt(string cipherText, string key)
        {

            //bool sm = false, sk = false;
            string binCT;
            //if (!(cipherText.Substring(0, 2).ToLower().Equals("0x")))
            //{
            //    sm = true;
            //    binCT = strToBin(cipherText);
            //}
            //else
            //{
            binCT = HexToBin(cipherText);
            //}
            //if (!(key.Substring(0, 2).ToLower().Equals("0x")))
            //    sk = true;
            List<string> nD = new List<string>();
            List<string> nC = new List<string>();
            List<string> nL = new List<string>();
            List<string> nR = new List<string>();
            List<string> nKey48 = new List<string>();
           
            string CTIP = "";
            string binKey64 = HexToBin(key);
            string binKey56 = "";
            string binKey48 = "";

            foreach (int x in DES_Constants.PC1)
            {
                binKey56 += binKey64[x - 1];
            }
            nC.Add(binKey56.Substring(0, 28));
            nD.Add(binKey56.Substring(28));
            for (int i = 0; i < 16; i++)
            {
                Tuple<string, string> keyParts = shift(nC[i], nD[i], DES_Constants.shifts[i]);
                nC.Add(keyParts.Item1);
                nD.Add(keyParts.Item2);


            }
            for (int i = 1; i < 17; i++)
            {
                binKey48 = "";
                foreach (int x in DES_Constants.PC2)
                {

                    binKey56 = nC[i] + nD[i];
                    binKey48 += binKey56[x - 1];


                }
                nKey48.Add(binKey48);
            }

            foreach (int x in DES_Constants.IP)
                CTIP += binCT[x - 1];
            nL.Add(CTIP.Substring(0, 32));
            nR.Add(CTIP.Substring(32));
            Tuple<string, string> textParts;
            for (int i = 0; i < 16; i++)
            {
                textParts = generateNR(nL[i], nR[i], nKey48[15 - i]);
                nL.Add(textParts.Item1);
                nR.Add(textParts.Item2);
            }
            string nRL = nR[16] + nL[16], iP_1 = "";
            foreach (int x in DES_Constants.IP_1)
                iP_1 += nRL[x - 1];
            string result = Convert.ToInt64(iP_1, 2).ToString("X");
            while (result.Length < 16)
                result = "0" + result;
            //if (sm)
            //{
            //    result = hexToStr(result);
            //    return result;
            //}
            return "0x" + result;
        }

        public override string Encrypt(string plainText, string key)
        { // Int64 intAgain = Int64.Parse(plainText.Substring(2), System.Globalization.NumberStyles.HexNumber);

            //bool sm = false, sk = false;
            string binPT;
            //if (!(plainText.Substring(0, 2).ToLower().Equals("0x")))
            //{
            //    sm = true;
            //    binPT = strToBin(plainText);
            //    System.Console.WriteLine(binPT);
            //}
            //else
            //{
            binPT = HexToBin(plainText);
            //}
            //if (!(key.Substring(0, 2).ToLower().Equals("0x")))
            //    sk = true;
            List<string> nD = new List<string>();
            List<string> nC = new List<string>();
            List<string> nL = new List<string>();
            List<string> nR = new List<string>();
            List<string> nKey48 = new List<string>();
           
            string PTIP = "";
            string binKey64 = HexToBin(key);
            string binKey56 = "";
            string binKey48 = "";

            foreach (int x in DES_Constants.PC1)
            {
                binKey56 += binKey64[x - 1];
            }
            nC.Add(binKey56.Substring(0, 28));
            nD.Add(binKey56.Substring(28));
            for (int i = 0; i < 16; i++)
            {
                Tuple<string, string> keyParts = shift(nC[i], nD[i], DES_Constants.shifts[i]);
                nC.Add(keyParts.Item1);
                nD.Add(keyParts.Item2);
                //System.Console.WriteLine("C= "+nC[i]);
                //System.Console.WriteLine("D= " + nD[i]);

            }
            for (int i = 1; i < 17; i++)
            {
                binKey48 = "";
                foreach (int x in DES_Constants.PC2)
                {

                    binKey56 = nC[i] + nD[i];
                    binKey48 += binKey56[x - 1];


                }
                nKey48.Add(binKey48);
                // System.Console.WriteLine(nKey48.);
                // System.Console.WriteLine("");
            }

            foreach (int x in DES_Constants.IP)
                PTIP += binPT[x - 1];
            nL.Add(PTIP.Substring(0, 32));
            nR.Add(PTIP.Substring(32));
            Tuple<string, string> textParts;
            for (int i = 0; i < 16; i++)
            {
                textParts = generateNR(nL[i], nR[i], nKey48[i]);
                nL.Add(textParts.Item1);
                nR.Add(textParts.Item2);
            }
            string nRL = nR[16] + nL[16], iP_1 = "";
            foreach (int x in DES_Constants.IP_1)
                iP_1 += nRL[x - 1];
            // textParts = generateNR(nL[0], nR[0], nKey48[0]);
            System.Console.WriteLine(Convert.ToInt64(iP_1, 2).ToString("X"));
            //System.Console.WriteLine("");
            //System.Console.WriteLine(nR[16]);
            string result = Convert.ToInt64(iP_1, 2).ToString("X");
            // System.Console.WriteLine(result+"   $");
            //if (sm)
            //{
            //    result = hexToStr(result);
            //    return result;
            //}
            return "0x" + result;
        }
        private string HexToBin(string hex)
        {

           
            string bin = "";
            hex = hex.Substring(2).ToUpper();
            int size = hex.Length;
            string res;
            foreach (char x in hex)
            {
                DES_Constants.hexBase.TryGetValue(x, out res);
                bin += res;
            }
            return bin;
        }

        private  Tuple<string, string> generateNR(string nL, string nR, string nKey)
        {
            // System.Console.WriteLine(nKey);
            // System.Console.WriteLine("");

            string eNR = "";
            foreach (int x in DES_Constants.E)
            {
                eNR += nR[x - 1];
            }
            //  System.Console.WriteLine(eNR);
            string xOR = "";
            for (int i = 0; i < 48; i++)
                xOR += (eNR[i].Equals(nKey[i]) ? '0' : '1');
            // System.Console.WriteLine(xOR);
            string f = genrateFromS_Box(xOR), nRnew = "";
            //System.Console.WriteLine(nL);
            for (int i = 0; i < 32; i++)
                nRnew += (f[i].Equals(nL[i]) ? '0' : '1');
            // System.Console.WriteLine(nRnew);
            return new Tuple<string, string>(nR, nRnew);
        }
        private  string genrateFromS_Box(string xOR)
        {

            string res;
            string sBox_R = "";
            int row, col;
            for (int i = 0, j = 0; i < 8; j += 6, i++)
            {
                // System.Console.WriteLine(row + ":" + col + ":" + Convert.ToString(S_BOXES[i, 16 * row + col], 2));
                row = Convert.ToInt32("00" + xOR[j] + xOR[j + 5], 2);
                col = Convert.ToInt32(xOR.Substring(j + 1, 4), 2);
                DES_Constants.hexBase.TryGetValue(DES_Constants.S_BOXES[i, 16 * row + col].ToString("X")[0], out res);
                sBox_R += res;
            }
            string f = "";
            foreach (int x in DES_Constants.P)
                f += sBox_R[x - 1];
            //System.Console.WriteLine(f);
            return f;

        }
        private  Tuple<string, string> shift(string nC, string nD, int shifts)
        {

            for (int i = 0; i < shifts; i++)
            {
                nC = nC.Substring(1) + nC[0];
                nD = nD.Substring(1) + nD[0];
            }
            Tuple<string, string> keyParts = new Tuple<string, string>(nC, nD);
            return keyParts;
        }

        private static string hexToStr(string hex)
        {
            string res = "";

            for (int i = 0; i < hex.Length; i += 2)
            {
                res += (char)Convert.ToInt32(hex.Substring(i, 2), 16);
            }
            //view(hex);
            // System.Console.WriteLine(res+"    %");
            return res;


        }
        private static string strToBin(string str)
        {
            string bin = "";
            string l;
            foreach (char x in str)
            {
                l = Convert.ToString(x, 2);
                while (l.Length < 8)
                    l = "0" + l;
                bin += l;
            }
            //view(hex);
            //System.Console.WriteLine();
            return bin;


        }
    
    }
}
