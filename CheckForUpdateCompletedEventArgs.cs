		internal CheckForUpdateCompletedEventArgs(Exception error, bool cancelled, object userState, bool updateAvailable, Version availableVersion, bool isUpdateRequired, Version minimumRequiredVersion, long updateSize) : base(error, cancelled, userState)
		{
			this._updateAvailable = updateAvailable;
			this._availableVersion = availableVersion;
			this._isUpdateRequired = isUpdateRequired;
			this._minimumRequiredVersion = minimumRequiredVersion;
			this._updateSize = updateSize;
		}
