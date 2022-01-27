		internal override Device CreateDevice(DeviceClass deviceClass, Native.SP_DEVINFO_DATA deviceInfoData, string path, int index, int disknum = -1)
		{
			return new Volume(deviceClass, deviceInfoData, path, index);
		}
