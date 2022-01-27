private bool IsExist(IDeviceInfo device, List<IDeviceInfo> listCheckDevices)
		{
			return device != null && listCheckDevices != null && listCheckDevices.FirstOrDefault((IDeviceInfo item) => ((item != null) ? item.SerialNumber : null) == device.SerialNumber) != null;
		}
