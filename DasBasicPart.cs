public static DasBasicPart GetMemoryCard(string driveID)
		{
			try
			{
				foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("select * from Win32_DiskDrive").Get())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					if (!(driveID != Convert.ToString(managementObject.Properties["DeviceId"].Value)))
					{
						return new DasBasicPart
						{
							DeviceID = driveID,
							Model = (managementObject.Properties["Model"].Value as string),
							Description = (managementObject.Properties["Description"].Value as string),
							Name = (managementObject.Properties["Name"].Value as string),
							Caption = (managementObject.Properties["Caption"].Value as string),
							PNPDeviceID = (managementObject.Properties["PNPDeviceID"].Value as string),
							InterfaceType = (managementObject.Properties["InterfaceType"].Value as string),
							MediaType = (managementObject.Properties["MediaType"].Value as string),
							FirmwareRevision = (managementObject.Properties["FirmwareRevision"].Value as string),
							SerialNumber = (managementObject.Properties["SerialNumber"].Value as string),
							Index = Convert.ToInt32(managementObject.Properties["Index"].Value),
							Size = Convert.ToInt64(managementObject.Properties["Size"].Value),
							Partitions = Convert.ToInt32(managementObject.Properties["Partitions"].Value)
						};
					}
				}
			}
			catch (Exception ex)
			{
				Device._log.E("GetMemoryCard ex: " + ex.ToString(), new object[0]);
			}
			return null;
		}
