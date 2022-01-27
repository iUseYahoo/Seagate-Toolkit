		internal static string EncodeHexString(byte[] sArray)
		{
			return X509Utils.EncodeHexString(sArray, 0U, (uint)sArray.Length);
		}
