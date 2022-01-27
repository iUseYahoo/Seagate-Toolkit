private void Reset()
		{
			if (this._initGetSpaceInfo)
			{
				this.FreeBytesAvailable = 0L;
				this.TotalNumberOfBytes = 0L;
				this.TotalNumberOfFreeBytes = 0L;
			}
			if (this._initGetClusterInfo)
			{
				this.BytesPerSector = 0;
				this.NumberOfFreeClusters = 0;
				this.SectorsPerCluster = 0;
				this.TotalNumberOfClusters = 0L;
			}
		}
