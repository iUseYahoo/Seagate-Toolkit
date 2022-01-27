		public string UsedSpaceUnitSize
		{
			get
			{
				return Utils.UnitSizeToText<long>(this.TotalNumberOfBytes - this.FreeBytesAvailable, this._cultureInfo);
			}
		}
