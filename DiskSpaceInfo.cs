		[SecurityCritical]
		public DiskSpaceInfo(string drivePath, bool? spaceInfoType, bool refresh, bool continueOnException) : this(drivePath)
		{
			if (spaceInfoType == null)
			{
				this._initGetSpaceInfo = (this._initGetClusterInfo = true);
			}
			else
			{
				this._initGetSpaceInfo = (!spaceInfoType).Value;
				this._initGetClusterInfo = spaceInfoType.Value;
			}
			this._continueOnAccessError = continueOnException;
			if (refresh)
			{
				this.Refresh();
			}
		}
