using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace SHA256
{
	public class Program
	{
		static void Main(string[] args)
        {
            //Input
            /*Console.Write("Input: ");
            string input = Console.ReadLine();*/

            if (args.Length > 2)
            {
                throw new ArgumentException("Cannot provide more than one argument");
            }

            Console.WriteLine(SHA256(args.Length == 0 ? "" : args[0]));
            return;

        }

        private static void SHA256WithAnimation(string input)
        {
            //ASCII and binary
            int[] asciiInput = input.Select(c => Convert.ToInt32(c)).ToArray();
            Console.WriteLine($"Bytes: [{string.Join(", ", asciiInput)}]");
            string message = string.Join(null, asciiInput.Select(n => ToBinary(n)));
            int length = message.Length;
            Console.WriteLine($"Message: {message}");
            Console.ReadLine();

            //Original message
            int cursorTop = Console.CursorTop;
            Console.WriteLine($"Padding ({message.Length} bits)");
            Console.WriteLine("-------");
            Console.WriteLine($"Message: {message}");
            WaitForEnter();

            //Message with added 1
            Console.CursorTop = cursorTop;
            message += 1;
            Console.WriteLine($"Padding ({message.Length} bits)");
            Console.WriteLine("-------");
            Console.WriteLine($"Message: {message}");
            WaitForEnter();

            //Message with padded zero's
            Console.CursorTop = cursorTop;
            message += MultiplyChar('0', -(message.Length % 512) + 448);
            Console.WriteLine($"Padding ({message.Length} bits)");
            Console.WriteLine("-------");
            Console.WriteLine($"Message: {message}");
            WaitForEnter();

            //Message with 64 bit length
            Console.CursorTop = cursorTop;
            message += ToBinary(length, 64);
            Console.WriteLine($"Padding ({message.Length} bits)");
            Console.WriteLine("-------");
            Console.WriteLine($"Message: {message}");

            long[] KLongs =
            {
                0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
                0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
                0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
                0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
                0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
                0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
                0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
                0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
            };

            string[] K = KLongs.Select(l => ToBinary(l, 32).Substring(0, 32)).ToArray();
            int[] primes = File.ReadAllLines("PrimeNumbers.txt").Select(p => Convert.ToInt32(p)).ToArray();

            //Display K - Constants
            Console.WriteLine();
            Console.WriteLine("K - Constants");
            Console.WriteLine("-------------");
            WaitForEnter();
            for (int i = 0; i < K.Length; ++i)
            {
                Console.WriteLine($"K{PadRight(i)} = {K[i]}");

                if (i < K.Length - 1)
                {
                    Console.WriteLine($"K{PadRight(i + 1)} = {(char)0x221A}{primes[i + 1]} = {K[i + 1]}");
                }

                if (i < K.Length - 2)
                {
                    Console.WriteLine($"K{PadRight(i + 2)} = {(char)0x221A}{primes[i + 2]} = {Math.Pow(primes[i + 2], 1 / 3.0).ToString().Substring(3)}");
                }

                if (i < K.Length - 3)
                {
                    Console.WriteLine($"K{PadRight(i + 3)} = {(char)0x221A}{primes[i + 3]} = {Math.Pow(primes[i + 3], 1 / 3.0)}");
                }

                WaitForEnter();
                DeleteLines(Math.Min(3, K.Length - i - 1));
            }

            long[] HLong = { 0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a, 0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19 };
            string[] H = HLong.Select(l => ToBinary(l, 32).Substring(0, 32)).ToArray();

            //Display initial hash constants
            Console.WriteLine();
            Console.WriteLine("Initial Hash Constants");
            Console.WriteLine("----------------------");
            for (int i = 0; i < H.Length; ++i)
            {
                Console.WriteLine($"H{PadRight(i)} = {H[i]}");

                if (i < H.Length - 1)
                {
                    Console.WriteLine($"H{PadRight(i + 1)} = {(char)0x221A}{primes[i + 1]} = {H[i + 1]}");
                }

                if (i < H.Length - 2)
                {
                    Console.WriteLine($"H{PadRight(i + 2)} = {(char)0x221A}{primes[i + 2]} = {Math.Sqrt(primes[i + 2]).ToString().Substring(3)}");
                }

                if (i < H.Length - 3)
                {
                    Console.WriteLine($"H{PadRight(i + 3)} = {(char)0x221A}{primes[i + 3]} = {Math.Sqrt(primes[i + 3])}");
                }

                WaitForEnter();
                DeleteLines(Math.Min(3, H.Length - i - 1));
            }

            string a, b, c, d, e, f, g, h;
            string T1, T2;

            string[] blocks = SplitString(message, 512).ToArray();
            for (int i = 0; i < blocks.Length; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"Block {i}");
                Console.WriteLine("-------");

                a = H[0];
                b = H[1];
                c = H[2];
                d = H[3];
                e = H[4];
                f = H[5];
                g = H[6];
                h = H[7];

                //Display the values of the a...h variables
                Console.WriteLine("a = H0");
                Console.WriteLine("b = H1");
                Console.WriteLine("c = H2");
                Console.WriteLine("d = H3");
                Console.WriteLine("e = H4");
                Console.WriteLine("f = H5");
                Console.WriteLine("g = H6");
                Console.WriteLine("h = H7");
                WaitForEnter();
                DeleteLines(8);

                //Display the values of the a...h variables with binary values
                Console.WriteLine($"a = H0 = {H[0]}");
                Console.WriteLine($"b = H1 = {H[1]}");
                Console.WriteLine($"c = H2 = {H[2]}");
                Console.WriteLine($"d = H3 = {H[3]}");
                Console.WriteLine($"e = H4 = {H[4]}");
                Console.WriteLine($"f = H5 = {H[5]}");
                Console.WriteLine($"g = H6 = {H[6]}");
                Console.WriteLine($"h = H7 = {H[7]}");
                WaitForEnter();
                DeleteLines(8);

                //Display the values of the a...h variables with binary values
                Console.WriteLine($"a = {H[0]}");
                Console.WriteLine($"b = {H[1]}");
                Console.WriteLine($"c = {H[2]}");
                Console.WriteLine($"d = {H[3]}");
                Console.WriteLine($"e = {H[4]}");
                Console.WriteLine($"f = {H[5]}");
                Console.WriteLine($"g = {H[6]}");
                Console.WriteLine($"h = {H[7]}");
                WaitForEnter();
                Console.WriteLine();

                string[] words = new string[64];
                Console.WriteLine("Words");
                Console.WriteLine("-----");

                var First16Words = SplitString(blocks[i], 32).ToArray();

                for (int j = 0; j < 16; ++j)
                {
                    words[j] = First16Words[j];
                    Console.WriteLine($"W{j.ToString().PadRight(2, ' ')} {words[j]}");
                    WaitForEnter();
                }

                for (int j = 16; j < 64; ++j)
                {
                    //Point out what is being added
                    cursorTop = Console.CursorTop;
                    Console.SetCursorPosition(36, Console.CursorTop - 2);
                    Console.Write($" ==> σ1({words[j - 2]})");
                    Console.SetCursorPosition(36, Console.CursorTop - 5);
                    Console.Write($" ==> {words[j - 7]}");
                    Console.SetCursorPosition(36, Console.CursorTop - 8);
                    Console.Write($" ==> σ0({words[j - 15]})");
                    Console.SetCursorPosition(36, Console.CursorTop - 1);
                    Console.Write($" ==> {words[j - 16]}");

                    Console.SetCursorPosition(0, cursorTop);
                    words[j] = AddMany(SmallSigma1(words[j - 2]), words[j - 7], SmallSigma0(words[j - 15]), words[j - 16]);
                    Console.WriteLine($"W{j.ToString().PadRight(2, ' ')} {words[j]} = σ1(i - 2) + (i - 7) + σ0(i - 15) + (i - 16)");
                    WaitForEnter();

                    //Remove explinations
                    cursorTop = Console.CursorTop;
                    Console.SetCursorPosition(36, cursorTop - 1);
                    ClearCurrentConsoleLine(36);
                    Console.SetCursorPosition(36, Console.CursorTop - 2);
                    ClearCurrentConsoleLine(36);
                    Console.SetCursorPosition(36, Console.CursorTop - 5);
                    ClearCurrentConsoleLine(36);
                    Console.SetCursorPosition(36, Console.CursorTop - 8);
                    ClearCurrentConsoleLine(36);
                    Console.SetCursorPosition(36, Console.CursorTop - 1);
                    ClearCurrentConsoleLine(36);
                    Console.SetCursorPosition(0, cursorTop);
                }
                Console.WriteLine();

                //Calculate variables
                for (int j = 0; j < words.Length; j++)
                {
                    Console.WriteLine($"Comppresion - Word {j}");
                    Console.WriteLine("--------------------");

                    //Display calculation of state registers
                    Console.WriteLine($"T1 = h + Σ1(e) + Ch(e, f, g) + K{j} + W{j}");
                    Console.WriteLine($"T2 = Σ0(a) + Maj(a, b, c)");
                    Console.WriteLine("h  = g");
                    Console.WriteLine("g  = f");
                    Console.WriteLine("f  = e");
                    Console.WriteLine("e  = d + T1");
                    Console.WriteLine("d  = c");
                    Console.WriteLine("c  = b");
                    Console.WriteLine("b  = a");
                    Console.WriteLine("a  = T1 + T2");
                    WaitForEnter();
                    DeleteLines(10);

                    T1 = AddMany(h, BigSigma1(e), Ch(e, f, g), K[j], words[j]);
                    T2 = AddMany(BigSigma0(a), Maj(a, b, c));
                    h = g;
                    g = f;
                    f = e;
                    e = Add(d, T1);
                    d = c;
                    c = b;
                    b = a;
                    a = Add(T1, T2);

                    Console.WriteLine($"T1 = h + Σ1(e) + Ch(e, f, g) + K{j.ToString().PadLeft(2, '0')} + W{j.ToString().PadLeft(2, '0')} = {T1}");
                    Console.WriteLine($"T2 = Σ0(a) + Maj(a, b, c)                = {T2}");
                    Console.WriteLine($"h  = g                                   = {h}");
                    Console.WriteLine($"g  = f                                   = {g}");
                    Console.WriteLine($"f  = e                                   = {f}");
                    Console.WriteLine($"e  = d + T1                              = {e}");
                    Console.WriteLine($"d  = c                                   = {d}");
                    Console.WriteLine($"c  = b                                   = {c}");
                    Console.WriteLine($"b  = a                                   = {b}");
                    Console.WriteLine($"a  = T1 + T2                             = {a}");
                    WaitForEnter();
                    DeleteLines(10);

                    Console.WriteLine($"T1 = {T1}");
                    Console.WriteLine($"T2 = {T2}");
                    Console.WriteLine($"h  = {h}");
                    Console.WriteLine($"g  = {g}");
                    Console.WriteLine($"f  = {f}");
                    Console.WriteLine($"e  = {e}");
                    Console.WriteLine($"d  = {d}");
                    Console.WriteLine($"c  = {c}");
                    Console.WriteLine($"b  = {b}");
                    Console.WriteLine($"a  = {a}");

                    WaitForEnter();

                    if (j != words.Length - 1)
                    {
                        DeleteLines(12);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("New Hash Values");
                Console.WriteLine("---------------");

                Console.WriteLine("H[0] = H[0] + a");
                Console.WriteLine("H[1] = H[1] + b");
                Console.WriteLine("H[2] = H[2] + c");
                Console.WriteLine("H[3] = H[3] + d");
                Console.WriteLine("H[4] = H[4] + e");
                Console.WriteLine("H[5] = H[5] + f");
                Console.WriteLine("H[6] = H[6] + g");
                Console.WriteLine("H[7] = H[7] + h");
                WaitForEnter();
                DeleteLines(8);

                H[0] = Add(H[0], a);
                H[1] = Add(H[1], b);
                H[2] = Add(H[2], c);
                H[3] = Add(H[3], d);
                H[4] = Add(H[4], e);
                H[5] = Add(H[5], f);
                H[6] = Add(H[6], g);
                H[7] = Add(H[7], h);

                for (int j = 0; j < H.Length; j++)
                {
                    Console.WriteLine($"H{j} {H[j]}");
                }
                WaitForEnter();
            }

            Console.WriteLine();
            Console.WriteLine("Convert Hash To Hexadecimal");
            Console.WriteLine("---------------------------");
            H = H.Select(h => BinaryToHexa(h)).ToArray();
            string hash = "";
            for (int i = 0; i < H.Length; i++)
            {
                Console.WriteLine($"H{i} {H[i]}");
                hash += H[i];
            }
            WaitForEnter();

            Console.WriteLine();
            Console.WriteLine("Final Hash");
            Console.WriteLine("----------");

            Console.WriteLine($"Hash: {H[0]} + {H[0]} +  {H[1]} +  {H[2]} +  {H[3]} +  {H[4]} +  {H[5]} +  {H[6]} +  {H[7]}");
            WaitForEnter();
            DeleteLines(1);
            Console.WriteLine($"Hash: {hash}");
        }

        public static string SHA256(string input)
        {
            int[] asciiInput = input.Select(c => Convert.ToInt32(c)).ToArray();
            string message = string.Join(null, asciiInput.Select(n => ToBinary(n)));
            int length = message.Length;
            message += 1 + MultiplyChar('0', -((message.Length + 1) % 512) + 448) + ToBinary(length, 64); ;

            long[] KLongs =
            {
                0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
                0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
                0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
                0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
                0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
                0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
                0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
                0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
            };

            string[] K = KLongs.Select(l => ToBinary(l, 32).Substring(0, 32)).ToArray();
            long[] HLong = { 0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a, 0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19 };
            string[] H = HLong.Select(l => ToBinary(l, 32).Substring(0, 32)).ToArray();

            string a, b, c, d, e, f, g, h;
            string T1, T2;

            foreach(string block in SplitString(message, 512))
            {
                a = H[0];
                b = H[1];
                c = H[2];
                d = H[3];
                e = H[4];
                f = H[5];
                g = H[6];
                h = H[7];

                string[] words = new string[64];
                var First16Words = SplitString(block, 32).ToArray();

                for (int j = 0; j < 16; ++j)
                {
                    words[j] = First16Words[j];
                }

                for (int j = 16; j < 64; ++j)
                {
                    words[j] = AddMany(SmallSigma1(words[j - 2]), words[j - 7], SmallSigma0(words[j - 15]), words[j - 16]);
                }

                for (int j = 0; j < words.Length; j++)
                {
                    T1 = AddMany(h, BigSigma1(e), Ch(e, f, g), K[j], words[j]);
                    T2 = AddMany(BigSigma0(a), Maj(a, b, c));
                    h = g;
                    g = f;
                    f = e;
                    e = Add(d, T1);
                    d = c;
                    c = b;
                    b = a;
                    a = Add(T1, T2);
                }

                H[0] = Add(H[0], a);
                H[1] = Add(H[1], b);
                H[2] = Add(H[2], c);
                H[3] = Add(H[3], d);
                H[4] = Add(H[4], e);
                H[5] = Add(H[5], f);
                H[6] = Add(H[6], g);
                H[7] = Add(H[7], h);
            }

            H = H.Select(h => BinaryToHexa(h)).ToArray();
            string hash = "";
            for (int i = 0; i < H.Length; i++)
            {
                hash += H[i];
            }

            return hash;
        }


        public static void ClearCurrentConsoleLine(int start = 0)
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(start, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth - start));
            Console.SetCursorPosition(start, currentLineCursor);
        }

        private static string PadRight(int i)
        {
            return i.ToString().PadRight(2, ' ');
        }

        private static void DeleteLines(int numLines)
        {
            for(int i = 0; i < numLines; ++i)
            {
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                ClearCurrentConsoleLine();
            }
        }

        private static void WaitForEnter()
        {
            //return;
            Console.ReadLine();
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
        }

        public static string BinaryToHexa(string binary)
        {
            if (binary.Length % 4 != 0)
            {
                throw new ArgumentException("Binary length must be a multiple of 4");
            }

            string hexa = "";
			int dec;
            foreach (string fourBits in SplitString(binary, 4))
            {
				dec = FourBitsToDec(fourBits);
				if(dec < 10)
                {
					hexa += dec;
                }
                else
                {
					hexa += (char)Convert.ToInt32(dec + 87);
                }
			}

			return hexa;
        }

        public static int FourBitsToDec(string fourBits)
        {
			if(fourBits.Length != 4)
            {
				throw new ArgumentException("Length must be 4");
			}

			int dec = 0;
			for(int i = 0; i < fourBits.Length; ++i)
            {
				dec += (int)((Convert.ToInt32(fourBits[i]) - '0') * Math.Pow(2, 3 - i));
            }

			return dec;
		}

		public static IEnumerable<string> SplitString(string text, int intervalLength)
        {
			for(int i = 0; i < text.Length/intervalLength; ++i)
            {
				yield return text.Substring(intervalLength * i, intervalLength);
            }

			if(text.Length % intervalLength == 0)
            {
				yield break;
            }

			yield return text.Substring(text.Length - text.Length % intervalLength, text.Length % intervalLength);
        }

		public static string AddMany(params string[] nums)
        {
			string current = nums[0];

			for(int i = 1; i < nums.Length; ++i)
            {
				current = Add(current, nums[i]);
            }

			return current;
        }

		public static string Add(string num1, string num2)
        {
			if(num1.Length != num2.Length)
            {
				throw new ArgumentException("Strings must be the same length");
            }

			string result = "";
			int carry = 0;

			for(int i = num1.Length - 1; i >= 0; --i)
            {
				int sum = ToInt(num1[i]) + ToInt(num2[i]) + carry;
				result = sum % 2 + result;

				carry = sum / 2;
            }

			return result;
        }

		public static string MultiplyChar(char c, int n)
        {
			if(n <= 0)
            {
				return string.Empty;
            }

			return string.Join(null, Enumerable.Range(0, n).Select(p => c));
        }

        private static void ConvertPrimesToRandomBinaryStrings()
        {
            foreach (var prime in File.ReadAllLines("PrimeNumbers.txt").Select(p => Convert.ToInt32(p)))
            {
                var cubeRoot = Math.Pow(prime, 1 / 3.0);
                var fracPart = cubeRoot - (int)cubeRoot;
                var intFracPart = (long)(fracPart * Math.Pow(10, 10));
                if (intFracPart > 4294967295)
                {
                    intFracPart /= 10;
                }

                Console.WriteLine(ToBinary(intFracPart, 32));
            }
        }

        public static string Maj(string x, string y, string z) => BitToStringWraper(x, y, z, Maj);

		public static string Ch(string x, string y, string z) => BitToStringWraper(x, y, z, Ch);

		public static string BitToStringWraper(string x, string y, string z, Func<int, int, int, int> function)
        {
			if (x.Length! != y.Length || x.Length != z.Length)
			{
				throw new ArgumentException("All 3 strings must be of equal length");
			}

			string result = "";

			for (int i = 0; i < x.Length; ++i)
			{
				result += $"{function(ToInt(x[i]), ToInt(y[i]), ToInt(z[i]))}";
			}

			return result;
		}

        public static int ToInt(char c) => Convert.ToInt32(c) - '0';

        public static int Maj(int x, int y, int z)
		{
			if (!IsBit(x, y, z))
			{
				throw new ArgumentException("Arguments must be bits");
			}

			return (x & y) ^ (x & z) ^ (y & z);
		}

		public static int Ch(int x, int y, int z)
		{
			if (!IsBit(x, y, z))
			{
				throw new ArgumentException("Arguments must be bits");
			}

			return (x & y) ^ (Comp(x) & z);
		}

		public static bool IsBit(params int[] bits)
		{
			foreach (int bit in bits)
			{
				if (bit != 1 && bit != 0)
				{
					return false;
				}
			}

			return true;
		}

		public static int Comp(int x)
		{
			if (!IsBit(x))
			{
				throw new ArgumentException("Argument must be a bit");
			}

			return 1 - x;
		}

		public static string ShR(string binNum, int n) => MultiplyChar('0', n) + binNum.Substring(0, binNum.Length - n);

        public static string RotR(string binNum, int n)
		{
			if(binNum.Length != 32)
            {
				throw new ArgumentException("Input must be 32 bits");
            }

			return binNum.Substring(binNum.Length - n) + binNum.Substring(0, binNum.Length - n);
		}

		public static string ToBinary(long num, int paddLeft = 8)
		{
			string bin = "";

			while (num > 0)
			{
				bin = $"{(num % 2 == 1 ? 1 : 0)}{bin}";
				num /= 2;
			}

			return bin.PadLeft(paddLeft, '0');
		}

		public static string BigSigma0(string num) => XOR(RotR(num, 2), RotR(num, 13), RotR(num, 22));
		public static string BigSigma1(string num) => XOR(RotR(num, 6), RotR(num, 11), RotR(num, 25));
		public static string SmallSigma0(string num) => XOR(RotR(num, 7), RotR(num, 18), ShR(num, 3));
		public static string SmallSigma1(string num) => XOR(RotR(num, 17), RotR(num, 19), ShR(num, 10));

		public string XOR(string x, string y)
		{
			if (x.Length! != y.Length)
			{
				throw new ArgumentException("Strings must be of equal length");
			}

			string result = "";

			for (int i = 0; i < x.Length; ++i)
			{
				result += $"{x[i] ^ y[i]}";
			}

			return result;
		}

		public static string XOR(string x, string y, string z) => BitToStringWraper(x, y, z, (x, y, z) => x ^ y ^ z);
        private static void GenaratePrimes()
        {
			StreamWriter outputFile = new StreamWriter("PrimeNumbers.txt");
			for(int i = 3; i <= 311; i += 2)
            {
                if (CheckPrime(i))
                {
					outputFile.WriteLine(i);
				}
            }

			outputFile.Close();
		}
        private static bool CheckPrime(int i)
        {
            for(int f = 2; f <= (int)Math.Sqrt(i); ++f)
            {
				if(i % f == 0)
                {
					return false;
                }
            }

			return true;
        }
    }
}
