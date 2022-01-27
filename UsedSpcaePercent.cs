		public string UsedSpacePercent
		{
			get
			{
				return DiskSpaceInfo.PercentCalculate((double)(this.TotalNumberOfBytes - this.FreeBytesAvailable), 0.0, (double)this.TotalNumberOfBytes).ToString("0.##", this._cultureInfo) + "%";
			}
		}
