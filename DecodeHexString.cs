internal static byte[] DecodeHexString(string s)
		{
			string text = Utils.DiscardWhiteSpaces(s);
			uint num = (uint)(text.Length / 2);
			byte[] array = new byte[num];
			int num2 = 0;
			int num3 = 0;
			while ((long)num3 < (long)((ulong)num))
			{
				array[num3] = (byte)((int)X509Utils.HexToByte(text[num2]) << 4 | (int)X509Utils.HexToByte(text[num2 + 1]));
				num2 += 2;
				num3++;
			}
			return array;
		}
