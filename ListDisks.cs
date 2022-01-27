public List<Device> Disks
		{
			get
			{
				if (this._disks == null)
				{
					this._disks = new List<Device>();
					if (this.DiskNumbers != null)
					{
						DiskDeviceClass diskDeviceClass = new DiskDeviceClass();
						foreach (int num in this.DiskNumbers)
						{
							foreach (Device device in diskDeviceClass.Devices)
							{
								if (device.DiskNumber == num)
								{
									this._disks.Add(device);
								}
							}
						}
					}
				}
				return this._disks;
			}
		}
