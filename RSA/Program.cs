using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;


namespace Project
{
    class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine("           *****     Welcome, HOW CAN WE HELP U?     *****\n");
            A:
                Console.WriteLine("           *****              Main Menu              *****\n");
                Console.WriteLine("PRESS \"1\" If You want to ENCRYPT your Personal Data\n");
                Console.WriteLine("PRESS \"2\" If You want to DECRYPT your Personal Data\n");
                Console.WriteLine("PRESS \"3\" If you want to GENERATE ENCRYPTION KEY\n\n");
                int x = Convert.ToInt16(Console.ReadLine());
                if (x == 1)
                {
                    Console.WriteLine("PRESS \"1\" If you have ENCRYPTION KEY");
                    Console.WriteLine("PRESS \"0\" To Go Back");
                    x = Convert.ToInt16(Console.ReadLine());
                    if (x == 1)
                    {
                        dataEncrypt objdataEncrypt = new dataEncrypt();
                        Console.WriteLine("Enter encryption key = ");
                        ulong e = Convert.ToUInt64(Console.ReadLine());
                        Console.WriteLine("Enter MOD (n) = ");
                        ulong n = Convert.ToUInt64(Console.ReadLine());
                        Console.WriteLine("Enter file Path");
                        string path = Directory.GetCurrentDirectory() + "/../../../data.txt";
                        objdataEncrypt.Encrypt(e, n, path, x);
                        x = 0;
                        goto A;
                    }
                    else
                        goto A;
                }
                else if (x == 2)
                {
                    dataEncrypt objdataEncrypt = new dataEncrypt();
                    Console.WriteLine("Enter decryption key");
                    ulong e = Convert.ToUInt64(Console.ReadLine());
                    Console.WriteLine("Enter MOD (n) = ");
                    ulong n = Convert.ToUInt64(Console.ReadLine());
                    Console.WriteLine("Ener file Path");
                    string path = Directory.GetCurrentDirectory() + "/../../../data.txt"; //Console.ReadLine();
                    objdataEncrypt.Encrypt(e, n, path, x);
                    x = 0;
                    goto A;
                }
                else if (x == 3)
                {
                    Encryptor objEncryptor = new Encryptor();
                    objEncryptor.KeyGenerator();
                    x = 0;
                    goto A;
                }
                else
                    goto A;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }

    class Encryptor
    {
        uint p, q;
        ulong n, fin, e, d;
        bool x = true, y = true;
        public void KeyGenerator()
        {
            uint temp = 0;
            do
            {
                Console.WriteLine("Enter any two prime numbers");
                Console.WriteLine("Go for Some Real Big Thing like...");
                Console.WriteLine("127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, etc... \n 1009 1013 1019 1021 1031 1033 1039 1049 1051 1061 1063 1069 1087 1091 1093 1097 1103 1109 1117 1123 1129 \n  9803 9811 9817 9829 9833 9839 9851 9857 9859 9871 9883 9887 9901 9907 9923 9931 9941 9949 ");
                p = Convert.ToUInt32(Console.ReadLine());
                q = Convert.ToUInt32(Console.ReadLine());
                temp = p * q;
                x = PrimeChecker(p);
                y = PrimeChecker(q);
            } while (x == false || y == false || temp < 95 || p == q);

            n = p * q; Console.WriteLine("n = " + n);
            fin = (p - 1) * (q - 1); Console.WriteLine("fin = " + fin);
            //1 < e && e < fin
            CoPrime(e, fin); Console.WriteLine("e = " + e);
            for (uint i = 0; i < fin * fin; i++)
            {
                if ((i * e) % fin == 1)
                {
                    d = i;
                    Console.WriteLine("d = " + d);
                    break;
                }
            }
        }

        bool PrimeChecker(uint a)
        {
            bool isPrime = true;
            for (int i = 2; i <= a / 2; ++i)
            {
                if (a % i == 0)
                {
                    isPrime = false;
                    break;
                }
            }
            if (isPrime)
                return (true);
            else
                return (false);
        }

        void CoPrime(ulong ekey, ulong fiN)
        {
            for (ekey = 0; ekey < fiN; ekey++)
            {
                bool isCoPrime = true;
                for (uint i = 2; i < fiN; i++)
                {
                    if ((fiN % i == 0 || i % fiN == 0) && (ekey % i == 0 || i % ekey == 0) && ekey != i)
                    {
                        //Console.WriteLine(i);
                        isCoPrime = false;
                        break;
                    }
                }
                if (isCoPrime)
                {
                    //Console.WriteLine(fiN + " and " + ekey + " are co primes");
                    e = ekey;
                    break;
                }
            }
        }

    }

    class dataEncrypt
    {
        public void Encrypt(ulong e, ulong n, string path, int a)
        {
            // Readfilefunction
            Console.WriteLine("Reading Data from your File.");
            string data = Reading(path);
            // MOD 95 0..94   32..126     
            //RSA method 
            // getting ascii
            Console.WriteLine("The Ascii Values are ....................................");
            int x = data.Count();
            int[] asciiArray = new int[x];
            for (int i = 0; i < x; i++)
            {
                asciiArray[i] = Convert.ToInt16(data[i]);
                Console.Write(asciiArray[i] + " ");
            }
            Console.Write("\n \n");
            Console.WriteLine("Steps that we are secretly doing are:");
            Console.WriteLine("***Mapping***");        
            // mapping    [32,126] ----> [0,94]
            int[] mappedArray = new int[x];
            for (int i = 0; i < x; i++)
            {
                mappedArray[i] = ((asciiArray[i] - 32) * ((94 - 0) / (126 - 32))) + 0;
                //Console.Write(mappedArray[i] + " ");                                             
            }
            Console.Write("\n");                                                             
            // encryption
            Console.WriteLine("***Block Method***");                                            
            ulong[] blocks;
            if (a == 1)
            {
                blocks = BlockMeToE(mappedArray, n); // block method
                Console.WriteLine("*****Encrypting the Data blocks.*****");
                Console.WriteLine(" Following are the Encrypted Numbers.");
            }
            else
            {
                blocks = BlockMeToD(mappedArray, n);
                Console.WriteLine("Decrypting the Data blocks.");
                Console.WriteLine(" Following are the Decrypted Numbers.");
            }

            int[] C = new int[blocks.Count()];
            for (int i = 0; i < blocks.Count(); i++)
            {
                C[i] = (int)iCanEncrypt(blocks[i], e, n);
                Console.Write(C[i] + " ");
            }
            Console.Write("\n \n");
            Console.WriteLine("ReBlock, to change it to printable asciis");
            int[] tuDigNum;
            if (a == 1)
            {
                tuDigNum = ReBlockE(C, n);
            }
            else
            {
                tuDigNum = ReBlockD(C, n);
            }
            Console.WriteLine("Mapping Back.");
            // data is now encrypted 
            // map back
            // mapping   [32,126] <---- [0, 94]
            x = tuDigNum.Count();
            int[] newmapArry = new int[tuDigNum.Count()];
            for (int i = 0; i < tuDigNum.Count(); i++)
            {
                newmapArry[i] = ((tuDigNum[i] - 0) * ((94 - 0) / (126 - 32))) + 32;
                //Console.Write(newmapArry[i] + " ");
            }
            Console.Write("\n");
            // get char
            Console.WriteLine("Getting Chars....");
            char[] chars = new char[x];
            for (int i = 0; i < x; i++)
            {
                chars[i] = Convert.ToChar(newmapArry[i]);
                // print string
                Console.Write(chars[i]);
            }
            Console.Write("\n");
            string charsStr = new string(chars);
            Console.WriteLine(Writing(charsStr, path));
        }

        string Reading(string Path)
        {
            string Data = null;
            try
            {
                using (StreamReader sr = new StreamReader(Path))
                {
                    string line;
                    for (int i = 0; (line = sr.ReadLine()) != null; i++)
                    {
                        Data = Data + line;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            Console.WriteLine(Data);
            return (Data);
        }

        string Writing(string Data, string Path)
        {
            try
            {
                using (StreamWriter fw = new StreamWriter /*File.AppendText*/(Path))
                {
                    fw.WriteLine(Data);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be write:");
                Console.WriteLine(e.Message);
            }
            return ("File has been Written");
        }

        ulong[] BlockMeToE(int[] mappedArray, ulong n)
        {
            List<char> outp = new List<char>();
            for (int i = 0; i < mappedArray.Count(); i++)      //Adding zeros to make each digit even
            {
                if (mappedArray[i] < 10)
                {
                    outp.Add(Convert.ToChar(48));           // 48 is ascii of 0
                    outp.Add(Convert.ToChar(mappedArray[i] + 48));
                }
                else
                {
                    outp.Add(Convert.ToChar((mappedArray[i] / 10) + 48));
                    outp.Add(Convert.ToChar((mappedArray[i] % 10) + 48));
                }
            }
            //------------------------------------------------------------------
            int nDigtInPairs = 0;
            ulong pairs = 95;           //Checking which 95 pair is close to n
            while (true)
            {
                if (pairs > n)
                {
                    break;
                }
                pairs = 95 + (pairs * 100);
                nDigtInPairs = nDigtInPairs + 2;
            }
            pairs = pairs / 100;
            //nDigtInPairs = nDigtInPairs - 2;
            //Console.WriteLine("Block Pair number is = " + pairs);       // pair has the 95 pair value close to n
            //Console.WriteLine("Number of digits in Pairs is = " + nDigtInPairs);
            //------------------------------------------------------------------
            int fizzalogic = outp.Count() % nDigtInPairs;
            for (int i = 0; i < fizzalogic; i++) { outp.Insert(0, '0'); } // adding zeros to make it proper blocks 

            char[] block = new char[nDigtInPairs];
            List<string> blockstring = new List<string>();      //making blocks and saving them to induvidual string
            int f = 0;
            for (int i = 0; i < outp.Count(); i++)
            {
                block[f] = outp[i];
                f = f + 1;
                if (f >= nDigtInPairs)
                {
                    blockstring.Add(new string(block));
                    f = 0;
                }
            }
            //------------------------------------------------------------------
            //Console.WriteLine("Blocks are ....................................................");
            string[] hehe = blockstring.ToArray();
            ulong[] blocknums = new ulong[blockstring.Count()];     //each block is converting to number
            for (int i = 0; i < blockstring.Count(); i++)
            {
                blocknums[i] = Convert.ToUInt64(hehe[i]);
                //Console.Write(hehe[i] + " ");
            }
            Console.Write("\n");

            return blocknums;
        }
        ulong[] BlockMeToD(int[] mappedArray, ulong nn)
        {
            string Sn = Convert.ToString(nn);
            int nDigtInPairs = Sn.Count();
            //Console.WriteLine("Block Pair number is = " + nDigtInPairs);       // block pair number is nDigitinPairs =to number of digits in n

            List<char> outp = new List<char>();
            for (int i = 0; i < mappedArray.Count(); i++)      //Adding zeros to make each digit even
            {
                if (mappedArray[i] < 10)
                {
                    outp.Add(Convert.ToChar(48));           // 48 is ascii of 0
                    outp.Add(Convert.ToChar(mappedArray[i] + 48));
                }
                else
                {
                    outp.Add(Convert.ToChar((mappedArray[i] / 10) + 48));
                    outp.Add(Convert.ToChar((mappedArray[i] % 10) + 48));
                }
            }
            //------------------------------------------------------------------
            int fizzalogic = outp.Count() % nDigtInPairs; //comented
            ///*may be at the end*/
            for (int i = 0; i < fizzalogic; i++) { outp.Insert(0, '0'); } // adding zeros to make it proper blocks 

            char[] block = new char[nDigtInPairs];
            List<string> blockstring = new List<string>();      //making blocks and saving them to induvidual string
            int f = 0;
            for (int i = 0; i < outp.Count(); i++)
            {
                block[f] = outp[i];
                f = f + 1;
                if (f >= nDigtInPairs)
                {
                    blockstring.Add(new string(block));
                    f = 0;
                }
            }

            //Console.WriteLine("Blocks are ....................................................");  
            string[] hehe = blockstring.ToArray();
            ulong[] blocknums = new ulong[blockstring.Count()];     //each block is converting to number
            for (int i = 0; i < blockstring.Count(); i++)
            {
                blocknums[i] = Convert.ToUInt64(hehe[i]);
                //Console.Write(hehe[i] + " ");
            }
            Console.Write("\n");

            return blocknums;
        }
        int[] ReBlockE(int[] CC, ulong nn)
        {
            string Sn = Convert.ToString(nn);
            int xn = Sn.Count();                 /// xn has nmmber of digigits in n

            List<string> Ctostring = new List<string>();
            for (int i = 0; i < CC.Count(); i++)
            {
                string temp = Convert.ToString(CC[i]);
                char[] array = temp.ToCharArray();
                List<char> outp = new List<char>(array);
                while (outp.Count() < xn)
                {
                    outp.Insert(0, '0');
                }
                char[] hehe = outp.ToArray();
                string s = new string(hehe);
                Ctostring.Add(s);
                //Ctostring.Add(temp);
            }
            string x = string.Concat(Ctostring.ToArray());
            //string.Join(",", Ctostring.ToArray());
            //Console.WriteLine("concatinated String.................................................................");
            //Console.WriteLine(x);
            //Console.WriteLine("converting to two digit Number......................................................");

            List<string> TuDigitBlock = new List<string>();
            for (int i = 0; i < x.Count(); i = i + 2)
            {
                string b;
                string a = Convert.ToString(x[i]);
                try
                {
                    b = Convert.ToString(x[i + 1]);
                }
                catch (Exception e)
                {
                    //Console.WriteLine("writing zeo");
                    b = Convert.ToString('0');
                }
                string two = a + b;
                TuDigitBlock.Add(two);
                //Console.Write(two + " ");
            }
            Console.Write("\n");
            string[] aarray = TuDigitBlock.ToArray();
            int[] tuDigNum = new int[aarray.Count()]; // it has array of two digit numbers
            for (int i = 0; i < aarray.Count(); i++)
            {
                tuDigNum[i] = Convert.ToInt16(aarray[i]);
            }
            return tuDigNum;
        }
        int[] ReBlockD(int[] CC, ulong nn)
        {
            int nDigtInPairs = 0;
            ulong pairs = 95;           //Checking which 95 pair is close to n
            while (true)
            {
                if (pairs > nn)
                {
                    break;
                }
                pairs = 95 + (pairs * 100);
                nDigtInPairs = nDigtInPairs + 2;
            }
            pairs = pairs / 100;
            //nDigtInPairs = nDigtInPairs - 2;
            //Console.WriteLine("Block Pair number is = " + pairs);       // pair has the 95 pair value close to n
            //Console.WriteLine("Number of digits in Pairs is = " + nDigtInPairs);

            List<string> Ctostring = new List<string>();
            for (int i = 0; i < CC.Count(); i++)
            {
                string temp = Convert.ToString(CC[i]);
                char[] array = temp.ToCharArray();
                List<char> outp = new List<char>(array);
                while (outp.Count() < nDigtInPairs)
                {
                    outp.Insert(0, '0');
                }
                char[] hehe = outp.ToArray();
                string s = new string(hehe);
                Ctostring.Add(s);
                //Ctostring.Add(temp);
            }
            string x = string.Concat(Ctostring.ToArray());
            //string.Join(",", Ctostring.ToArray());
            //Console.WriteLine("concatinated String.................................................................");
            //Console.WriteLine(x);
            //Console.WriteLine("converting to two digit Number......................................................");

            List<string> TuDigitBlock = new List<string>();
            for (int i = 0; i < x.Count(); i = i + 2)
            {
                string b;
                string a = Convert.ToString(x[i]);
                try
                {
                    b = Convert.ToString(x[i + 1]);
                }
                catch (Exception e)
                {
                    //Console.WriteLine("writing zeo");
                    b = Convert.ToString('0');
                }
                string two = a + b;
                TuDigitBlock.Add(two);
                //Console.Write(two + " ");
            }
            Console.Write("\n");
            string[] aarray = TuDigitBlock.ToArray();
            int[] tuDigNum = new int[aarray.Count()]; // it has array of two digit numbers
            for (int i = 0; i < aarray.Count(); i++)
            {
                tuDigNum[i] = Convert.ToInt16(aarray[i]);
            }
            return tuDigNum;
        }

        public BigInteger iCanEncrypt(ulong num, ulong e, ulong n)
        {
            BigInteger number = num;
            BigInteger exponent = e;
            BigInteger mod = n;
            BigInteger ans = BigInteger.ModPow(number, exponent, mod);
            return ans;
        }

    }

}
