		public string TotalSizeUnitSize
		{
			get
			{
				return Utils.UnitSizeToText<long>(this.TotalNumberOfBytes, this._cultureInfo);
			}
		}
