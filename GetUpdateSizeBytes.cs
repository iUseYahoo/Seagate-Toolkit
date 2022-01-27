		public long UpdateSizeBytes
		{
			get
			{
				this.RaiseExceptionIfUpdateNotAvailable();
				return this._updateSize;
			}
		}
