		public bool UpdateAvailable
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._updateAvailable;
			}
		}
