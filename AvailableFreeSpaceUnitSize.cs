		public string AvailableFreeSpaceUnitSize
		{
			get
			{
				return Utils.UnitSizeToText<long>(this.TotalNumberOfFreeBytes, this._cultureInfo);
			}
		}
