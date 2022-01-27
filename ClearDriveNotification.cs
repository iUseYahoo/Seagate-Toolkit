public static void ClearDriveNotification(Log _log)
		{
			try
			{
				_log.I("ClearDriveNotification", new object[0]);
				List<IntPtr> list = new List<IntPtr>();
				foreach (IntPtr intPtr in USBNotification.Handles.Keys)
				{
					USBNotification usbnotification = USBNotification.Handles[intPtr];
					if (string.IsNullOrEmpty(Utility.IsVolumeExist(usbnotification.volNumber)))
					{
						_log.I("vol not exist drive: {0}, vol: {1}", new object[]
						{
							usbnotification.drive,
							usbnotification.volNumber
						});
						if (usbnotification.directoryHandle != IntPtr.Zero)
						{
							Kernel32API.CloseHandle(usbnotification.directoryHandle);
						}
						list.Add(intPtr);
						usbnotification.Unregister(null);
					}
				}
				foreach (IntPtr key in list)
				{
					USBNotification.Handles.Remove(key);
				}
			}
			catch (Exception ex)
			{
				_log.E("RemoveDriveNotification ex: {0}", new object[]
				{
					ex.ToString()
				});
			}
		}
