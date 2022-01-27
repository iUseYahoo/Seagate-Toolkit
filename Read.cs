public override int Read(byte[] buffer, int bufferOffset, long sourceOffset, int count)
			{
				int num = (int)Math.Min((long)count, this._size - sourceOffset);
				if (num < 0)
				{
					throw new ArgumentException(Resources.GetString("Ex_InvalidCopyRequest"));
				}
				this._file.Seek(this._address + sourceOffset, SeekOrigin.Begin);
				return this._file.Read(buffer, bufferOffset, num);
			}
