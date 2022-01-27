		public string AvailableFreeSpacePercent
		{
			get
			{
				return DiskSpaceInfo.PercentCalculate((double)(this.TotalNumberOfBytes - (this.TotalNumberOfBytes - this.TotalNumberOfFreeBytes)), 0.0, (double)this.TotalNumberOfBytes).ToString("0.##", this._cultureInfo) + "%";
			}
		}
