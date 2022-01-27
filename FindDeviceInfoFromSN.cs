private List<IDeviceInfo> FindDeviceInfoFromSN(List<StoragePoolDevice> listStoragePoolDevices)
		{
			Log log = this._log;
			if (log != null)
			{
				log.I(string.Format("Start to FindDeviceInfoFromSN; listStoragePoolDevices.Count = {0}", (listStoragePoolDevices != null) ? new int?(listStoragePoolDevices.Count) : null), new object[0]);
			}
			List<IDeviceInfo> list = new List<IDeviceInfo>();
			try
			{
				if (listStoragePoolDevices != null && listStoragePoolDevices.Count > 0)
				{
					foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice").Get())
					{
						ManagementObject managementObject = new ManagementObject(((ManagementObject)managementBaseObject)["Dependent"].ToString());
						string text = managementObject["DeviceID"].ToString();
						Match match = Regex.Match(text, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
						if (match.Success)
						{
							foreach (StoragePoolDevice storagePoolDevice in listStoragePoolDevices)
							{
								string serialNumber = storagePoolDevice.SerialNumber;
								string value = Utility.ReverseString(serialNumber);
								if (text.EndsWith(serialNumber, StringComparison.CurrentCultureIgnoreCase) || text.EndsWith(value, StringComparison.CurrentCultureIgnoreCase))
								{
									Log log2 = this._log;
									if (log2 != null)
									{
										log2.I("FindDeviceInfoFromSN; Set Device->DeviceID = " + text, new object[0]);
									}
									DeviceInfo deviceInfo = new DeviceInfo();
									string fullSerialNumber = text.Substring(text.Length - serialNumber.Length);
									deviceInfo.DeviceID = text;
									deviceInfo.PNPDeviceID = managementObject["PNPDeviceID"].ToString();
									deviceInfo.VID = (int)Convert.ToUInt16(match.Value.Substring(4, 4), 16);
									deviceInfo.PID = (int)Convert.ToUInt16(match.Value.Substring(13, 4), 16);
									deviceInfo.FullSerialNumber = fullSerialNumber;
									deviceInfo.SerialNumber = Utility.TrimSerialNumber(deviceInfo.FullSerialNumber);
									deviceInfo.Capacity = storagePoolDevice.Size;
									if (DeviceInfoHelper.FillDeviceXmlInfo(deviceInfo, DeviceInfoHelper._deviceXmlInfos))
									{
										Log log3 = this._log;
										if (log3 != null)
										{
											log3.I("FindDeviceInfoFromSN; Add to listDeviceInfos: DeviceID = " + text, new object[0]);
										}
										list.Add(deviceInfo);
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log log4 = this._log;
				if (log4 != null)
				{
					log4.E("Exception on MSStorageSpaceDetector->FindDeviceInfoFromSN; ex: {0}", new object[]
					{
						ex.ToString()
					});
				}
			}
			return list;
		}
