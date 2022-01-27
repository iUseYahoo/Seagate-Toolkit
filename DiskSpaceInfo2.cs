		[SecurityCritical]
		public DiskSpaceInfo(string drivePath)
		{
			if (Utils.IsNullOrWhiteSpace(drivePath))
			{
				throw new ArgumentNullException("drivePath");
			}
			drivePath = ((drivePath.Length == 1) ? (drivePath + Path.VolumeSeparatorChar.ToString()) : Path.GetPathRoot(drivePath, false));
			if (Utils.IsNullOrWhiteSpace(drivePath))
			{
				throw new ArgumentException(Resources.InvalidDriveLetterArgument, "drivePath");
			}
			this.DriveName = Path.AddTrailingDirectorySeparator(drivePath, false);
		}
