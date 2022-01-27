private string GetStringUsingEncoding(WebRequest request, byte[] data)
		{
			Encoding encoding = null;
			int num = -1;
			string text;
			try
			{
				text = request.ContentType;
			}
			catch (NotImplementedException)
			{
				text = null;
			}
			catch (NotSupportedException)
			{
				text = null;
			}
			if (text != null)
			{
				text = text.ToLower(CultureInfo.InvariantCulture);
				string[] array = text.Split(new char[]
				{
					';',
					'=',
					' '
				});
				bool flag = false;
				foreach (string text2 in array)
				{
					if (text2 == "charset")
					{
						flag = true;
					}
					else if (flag)
					{
						try
						{
							encoding = Encoding.GetEncoding(text2);
						}
						catch (ArgumentException)
						{
							break;
						}
					}
				}
			}
			if (encoding == null)
			{
				Encoding[] array3 = new Encoding[]
				{
					Encoding.UTF8,
					Encoding.UTF32,
					Encoding.Unicode,
					Encoding.BigEndianUnicode
				};
				for (int j = 0; j < array3.Length; j++)
				{
					byte[] preamble = array3[j].GetPreamble();
					if (this.ByteArrayHasPrefix(preamble, data))
					{
						encoding = array3[j];
						num = preamble.Length;
						break;
					}
				}
			}
			if (encoding == null)
			{
				encoding = this.Encoding;
			}
			if (num == -1)
			{
				byte[] preamble2 = encoding.GetPreamble();
				if (this.ByteArrayHasPrefix(preamble2, data))
				{
					num = preamble2.Length;
				}
				else
				{
					num = 0;
				}
			}
			return encoding.GetString(data, num, data.Length - num);
		}
