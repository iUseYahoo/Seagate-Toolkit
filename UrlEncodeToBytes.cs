		private static byte[] UrlEncodeToBytes(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			byte[] bytes = e.GetBytes(str);
			return WebClient.UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false);
		}
