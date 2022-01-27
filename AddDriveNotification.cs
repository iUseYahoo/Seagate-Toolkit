public static void AddDriveNotification(string drive, string volSn, Log _log)
		{
			try
			{
				if (string.IsNullOrEmpty(volSn))
				{
					volSn = Utility.GetVolumnSN(drive);
				}
				foreach (IntPtr key in USBNotification.Handles.Keys)
				{
					USBNotification usbnotification = USBNotification.Handles[key];
					if (string.Compare(usbnotification.drive, drive, true) == 0 && string.Compare(usbnotification.volNumber, volSn) == 0)
					{
						return;
					}
				}
				new USBNotification().Register(drive, volSn, _log);
			}
			catch (Exception ex)
			{
				_log.E("AddDriveNotification ex: {0}", new object[]
				{
					ex.ToString()
				});
			}
		}
