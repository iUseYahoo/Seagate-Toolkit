		public Version AvailableVersion
		{
			get
			{
				this.RaiseExceptionIfUpdateNotAvailable();
				return this._availableVersion;
			}
		}
