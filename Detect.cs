// From looking at this it looks like its just checking for a StoragePoolDevice as it says lower in the code.
// Looks like it also grabs the DeviceID, mDirStoragePoolDevice

public List<IDeviceInfo> Detect(string deviceId, string sn)
		{
			Log log = this._log;
			if (log != null)
			{
				log.I("Start to Detect StoragePoolDevice，deviceId = " + deviceId + "; SN = " + sn, new object[0]);
			}
			List<IDeviceInfo> list = new List<IDeviceInfo>();
			List<IDeviceInfo> list2 = new List<IDeviceInfo>();
			try
			{
				List<StoragePoolDevice> listStoragePoolDevices = new List<StoragePoolDevice>();
				listStoragePoolDevices = MSStorageSpaceDetector.Instance.FindPhysicalDiskSerialNumbers(sn);
				list2 = MSStorageSpaceDetector.Instance.FindDeviceInfoFromSN(listStoragePoolDevices);
				Log log2 = this._log;
				if (log2 != null)
				{
					log2.I(string.Format("The count of listStorageSpaceDevices is {0}", (list2 != null) ? new int?(list2.Count) : null), new object[0]);
				}
				if (list2 != null && list2.Count > 0)
				{
					Dictionary<string, List<IDeviceInfo>> obj = this.mDirStoragePoolDevice;
					lock (obj)
					{
						if (this.mDirStoragePoolDevice.Keys.Contains(deviceId))
						{
							Log log3 = this._log;
							if (log3 != null)
							{
								log3.I("The SN = " + deviceId + " is exist in mDirStoragePoolDevice", new object[0]);
							}
							foreach (IDeviceInfo deviceInfo in this.mDirStoragePoolDevice[deviceId])
							{
								if (!this.IsExist(deviceInfo, list2))
								{
									Log log4 = this._log;
									if (log4 != null)
									{
										log4.I("The SerialNumber = " + ((deviceInfo != null) ? deviceInfo.SerialNumber : null) + " device has been removed in mDirStoragePoolDevice", new object[0]);
									}
									list.Add(deviceInfo);
								}
							}
							this.mDirStoragePoolDevice[deviceId] = list2;
						}
						else
						{
							Log log5 = this._log;
							if (log5 != null)
							{
								log5.I("Add deviceId = " + deviceId + " to mDirStoragePoolDevice", new object[0]);
							}
							this.mDirStoragePoolDevice.Add(deviceId, list2);
						}
					}
				}
				if (list != null && list.Count > 0 && this.OnStorageSpaceDeviceRemovedEvent != null)
				{
					this.OnStorageSpaceDeviceRemovedEvent(deviceId, list);
				}
			}
			catch (Exception arg)
			{
				Log log6 = this._log;
				if (log6 != null)
				{
					log6.I(string.Format("Exception on Detect StoragePoolDevice, ex = {0}", arg), new object[0]);
				}
			}
			Log log7 = this._log;
			if (log7 != null)
			{
				log7.I(string.Format("Finished to Detect StoragePoolDevice， Find {0} added devices and {1} removed devices", (list2 != null) ? new int?(list2.Count) : null, (list != null) ? new int?(list.Count) : null), new object[0]);
			}
			return list2;
		}
