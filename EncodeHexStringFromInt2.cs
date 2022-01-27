		internal static string EncodeHexStringFromInt(byte[] sArray)
		{
			return X509Utils.EncodeHexStringFromInt(sArray, 0U, (uint)sArray.Length);
		}
