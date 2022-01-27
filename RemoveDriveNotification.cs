public static void RemoveDriveNotification(string driveLetter, string volSn, Log _log)
		{
			if (string.IsNullOrEmpty(driveLetter))
			{
				return;
			}
			try
			{
				if (string.IsNullOrEmpty(volSn))
				{
					volSn = Utility.GetVolumnSN(driveLetter);
				}
				IntPtr intPtr = IntPtr.Zero;
				foreach (IntPtr intPtr2 in USBNotification.Handles.Keys)
				{
					USBNotification usbnotification = USBNotification.Handles[intPtr2];
					if (string.Compare(usbnotification.drive, driveLetter, true) == 0 && string.Compare(usbnotification.volNumber, volSn) == 0)
					{
						Kernel32API.CloseHandle(usbnotification.directoryHandle);
						usbnotification.Unregister(_log);
						intPtr = intPtr2;
					}
				}
				if (intPtr != IntPtr.Zero)
				{
					USBNotification.Handles.Remove(intPtr);
				}
			}
			catch (Exception arg)
			{
				if (_log != null)
				{
					_log.E(string.Format("RemoveDriveNotification ex: {0}", arg), new object[0]);
				}
			}
		}
