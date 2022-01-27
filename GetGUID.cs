		public string Guid
		{
			get
			{
				if (Utils.IsNullOrWhiteSpace(this._guid))
				{
					this._guid = ((!Utils.IsNullOrWhiteSpace(this.FullPath)) ? Volume.GetUniqueVolumeNameForPath(this.FullPath) : null);
				}
				return this._guid;
			}
		}
