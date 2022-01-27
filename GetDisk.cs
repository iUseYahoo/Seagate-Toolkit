public static DasBasicPart GetDisk(ManagementBaseObject moDisk)
		{
			try
			{
				return new DasBasicPart
				{
					DeviceID = (moDisk["DeviceID"] as string),
					Model = (moDisk["Model"] as string),
					Description = (moDisk["Description"] as string),
					Name = (moDisk["Name"] as string),
					Caption = (moDisk["Caption"] as string),
					PNPDeviceID = (moDisk["PNPDeviceID"] as string),
					InterfaceType = (moDisk["InterfaceType"] as string),
					MediaType = (moDisk["MediaType"] as string),
					FirmwareRevision = (moDisk["FirmwareRevision"] as string),
					SerialNumber = (moDisk["SerialNumber"] as string),
					Index = Convert.ToInt32(moDisk["Index"]),
					Size = Convert.ToInt64(moDisk["Size"]),
					Partitions = Convert.ToInt32(moDisk["Partitions"])
				};
			}
			catch (Exception ex)
			{
				Device._log.E("GetDisk ex: {0}", new object[]
				{
					ex.ToString()
				});
			}
			return null;
		}
