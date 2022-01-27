public bool IsUpdateRequired
		{
			get
			{
				this.RaiseExceptionIfUpdateNotAvailable();
				return this._isUpdateRequired;
			}
		}
