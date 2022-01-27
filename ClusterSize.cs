		public long ClusterSize
		{
			get
			{
				return (long)(this.SectorsPerCluster * this.BytesPerSector);
			}
		}
