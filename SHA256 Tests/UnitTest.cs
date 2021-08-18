using NUnit.Framework;
using SHA256;
using System.Collections.Generic;

namespace SHA256_Tests
{
	public class Tests
	{
		private static readonly TestCaseData[] ChInputs = new[]
		{
			new TestCaseData(0,0,0)
			{
				ExpectedResult = 0,
				TestName = "0,0,0"
			},

			new TestCaseData(0,0,1)
			{
				ExpectedResult = 1,
				TestName = "0,0,1"
			},

			new TestCaseData(0,1,0)
			{
				ExpectedResult = 0,
				TestName = "0,1,0"
			},

			new TestCaseData(0,1,1)
			{
				ExpectedResult = 1,
				TestName = "0,1,1"
			},

			new TestCaseData(1,0,0)
			{
				ExpectedResult = 0,
				TestName = "1,0,0"
			},

			new TestCaseData(1,0,1)
			{
				ExpectedResult = 0,
				TestName = "1,0,1"
			},

			new TestCaseData(1,1,0)
			{
				ExpectedResult = 1,
				TestName = "1,1,0"
			},

			new TestCaseData(1,1,1)
			{
				ExpectedResult = 1,
				TestName = "1,1,1"
			},


		};

		[TestCaseSource(nameof(ChInputs))]
		public int ChTests(int x, int y, int z) => Program.Ch(x, y, z);

		private static readonly TestCaseData[] MajInputs = new[]
		{
			new TestCaseData(0,0,0)
			{
				ExpectedResult = 0,
				TestName = "Maj - 0,0,0"
			},

			new TestCaseData(0,0,1)
			{
				ExpectedResult = 0,
				TestName = "Maj - 0,0,1"
			},

			new TestCaseData(0,1,0)
			{
				ExpectedResult = 0,
				TestName = "Maj - 0,1,0"
			},

			new TestCaseData(0,1,1)
			{
				ExpectedResult = 1,
				TestName = "Maj - 0,1,1"
			},

			new TestCaseData(1,0,0)
			{
				ExpectedResult = 0,
				TestName = "Maj - 1,0,0"
			},

			new TestCaseData(1,0,1)
			{
				ExpectedResult = 1,
				TestName = "Maj - 1,0,1"
			},

			new TestCaseData(1,1,0)
			{
				ExpectedResult = 1,
				TestName = "Maj - 1,1,0"
			},

			new TestCaseData(1,1,1)
			{
				ExpectedResult = 1,
				TestName = "Maj - 1,1,1"
			},


		};

		[TestCaseSource(nameof(MajInputs))]
		public int MajTests(int x, int y, int z) => Program.Maj(x, y, z);

		private static readonly TestCaseData[] MajStringInputs = new[]
		{
			new TestCaseData("000", "010", "110")
			{
				ExpectedResult = "010",
				TestName = "MajString1"
			},

			new TestCaseData("101", "010", "110")
			{
				ExpectedResult = "110",
				TestName = "MajString2"
			},

		};

		[TestCaseSource(nameof(MajStringInputs))]
		public string MajStringTests(string x, string y, string z) => Program.Maj(x, y, z);

		private static readonly TestCaseData[] ChStringInputs = new[]
		{
			new TestCaseData("000", "010", "110")
			{
				ExpectedResult = "110",
				TestName = "ChString1"
			},

			new TestCaseData("101", "010", "110")
			{
				ExpectedResult = "010",
				TestName = "ChjString2"
			},

			new TestCaseData("001", "011", "110")
			{
				ExpectedResult = "111",
				TestName = "ChjString3"
			},

		};

		[TestCaseSource(nameof(ChStringInputs))]
		public string ChStringTests(string x, string y, string z) => Program.Ch(x, y, z);

		private static readonly TestCaseData[] RotRInputs = new[]
		{
			new TestCaseData("11000011111100010110111110001100", 7)
			{
				ExpectedResult = "00011001100001111110001011011111",
				TestName = "RotR1"
			},

			new TestCaseData("11011111100111111111011110101111", 14)
			{
				ExpectedResult = "11011110101111110111111001111111",
				TestName = "RotR2"
			},
			

			new TestCaseData("00000100110111010100011111111010", 1)
			{
				ExpectedResult = "00000010011011101010001111111101",
				TestName = "RotR3"
			},
		};

		[TestCaseSource(nameof(RotRInputs))]
		public string RotRTests(string x, int n) => Program.RotR(x, n);

		private static readonly TestCaseData[] ShRInputs = new[]
		{
			new TestCaseData("11000011111100010110111110001100", 7)
			{
				ExpectedResult = "00000001100001111110001011011111",
				TestName = "ShR1"
			},

			new TestCaseData("11011111100111111111011110101111", 14)
			{
				ExpectedResult = "00000000000000110111111001111111",
				TestName = "ShR2"
			},


			new TestCaseData("00000100110111010100011111111010", 1)
			{
				ExpectedResult = "00000010011011101010001111111101",
				TestName = "ShR3"
			},
		};

		[TestCaseSource(nameof(ShRInputs))]
		public string ShRTests(string x, int n) => Program.ShR(x, n);

		private static readonly TestCaseData[] MultiplyCharInputs = new[]
		{
			new TestCaseData('-', 7)
			{
				ExpectedResult = "-------",
				TestName = "MultiplyChar1"
			},

			new TestCaseData('0', 14)
			{
				ExpectedResult = "00000000000000",
				TestName = "MultiplyChar2"
			},
		};

		[TestCaseSource(nameof(MultiplyCharInputs))]
		public string MultiplyCharTests(char x, int n) => Program.MultiplyChar(x, n);

		private static readonly TestCaseData[] XORInputs = new[]
		{
			new TestCaseData("10000111110001011011100010101000", "10000110110100101111000100001011", "11000110111001011111111010001111")
			{
				ExpectedResult = "11000111111100101011011100101100",
				TestName = "XOR"
			},

		};

		[TestCaseSource(nameof(XORInputs))]
		public string XORTests(string x, string y, string z) => Program.XOR(x, y, z);

		private static readonly TestCaseData[] AddInputs = new[]
		{
			new TestCaseData("10000111110001011011100010101000", "10000110110100101111000100001011")
			{
				ExpectedResult = "00001110100110001010100110110011",
				TestName = "Add-Stanard1"
			},

			new TestCaseData("10011101010110001110101110111010", "11000001011011101010110100110101")
			{
				ExpectedResult = "01011110110001111001100011101111",
				TestName = "Add-Stanard2"
			},

			new TestCaseData("11111111111111111111111111111111", "00000000000000000000000000000001")
			{
				ExpectedResult = "00000000000000000000000000000000",
				TestName = "Add-Overflow"
			},

			new TestCaseData("11111111111111111111111111111111", "11111111111111111111111111111111")
			{
				ExpectedResult = "11111111111111111111111111111110",
				TestName = "Add-AllOnes"
			},

			new TestCaseData("1", "0")
			{
				ExpectedResult = "1",
				TestName = "Add-basic1"
			},

			new TestCaseData("1", "1")
			{
				ExpectedResult = "0",
				TestName = "Add-basic2"
			},

			new TestCaseData("10", "01")
			{
				ExpectedResult = "11",
				TestName = "Add-basic3"
			},

			new TestCaseData("10", "10")
			{
				ExpectedResult = "00",
				TestName = "Add-basic4"
			},

            new TestCaseData("010", "010")
            {
                ExpectedResult = "100",
                TestName = "Add-basic5"
            },

			new TestCaseData("11", "11")
			{
				ExpectedResult = "10",
				TestName = "Add-basic6"
			},

		};

		[TestCaseSource(nameof(AddInputs))]
		public string AddTests(string x, string y) => Program.Add(x, y);

		private static readonly TestCaseData[] AddManyInputs = new[]
		{
			new TestCaseData("10000111110001011011100010101000", "10000110110100101111000100001011", "10111001011001010000110111001010")
			{
				ExpectedResult = "11000111111111011011011101111101",
				TestName = "Add-ThreeNums"
			},
		};

		[TestCaseSource(nameof(AddManyInputs))]
		public string AddManyTests(string x, string y, string z) => Program.AddMany(x, y, z);

		private static readonly TestCaseData[] SplitStringInputs = new[]
		{
			new TestCaseData("123456789", 3)
			{
				ExpectedResult = new string[]
				{
					"123",
					"456",
					"789"
				},
				TestName = "Split - Simple"
			},

			new TestCaseData("123456789", 4)
			{
				ExpectedResult = new string[]
				{
					"1234",
					"5678",
					"9"
				},
				TestName = "Split - NotSimple"
			},
		};

		[TestCaseSource(nameof(SplitStringInputs))]
		public IEnumerable<string> SplitStringTests(string x, int l) => Program.SplitString(x, l);

		private static readonly TestCaseData[] FourBitsToDecInputs = new[]
		{
			new TestCaseData("1111")
			{
				ExpectedResult = 15,
				TestName = "FourBitsToDec1"
			},

			new TestCaseData("1010")
			{
				ExpectedResult = 10,
				TestName = "FourBitsToDec2"
			},

			new TestCaseData("0001")
			{
				ExpectedResult = 1,
				TestName = "FourBitsToDec3"
			},

			new TestCaseData("0000")
			{
				ExpectedResult = 0,
				TestName = "FourBitsToDec4"
			},
		};

		[TestCaseSource(nameof(FourBitsToDecInputs))]
		public int FourBitsToDecTests(string x) => Program.FourBitsToDec(x);

		private static readonly TestCaseData[] BinaryToHexaInputs = new[]
		{
			new TestCaseData("11111111")
			{
				ExpectedResult = "ff",
				TestName = "BinaryToHexa1"
			},

			new TestCaseData("10000111110001011011100010101000")
			{
				ExpectedResult = "87c5b8a8",
				TestName = "BinaryToHexa2"
			},

			new TestCaseData("10000110110100101111000100001011")
			{
				ExpectedResult = "86d2f10b",
				TestName = "BinaryToHexa3"
			},
		};

		[TestCaseSource(nameof(BinaryToHexaInputs))]
		public string BinaryToHexaTests(string x) => Program.BinaryToHexa(x);
	}
}