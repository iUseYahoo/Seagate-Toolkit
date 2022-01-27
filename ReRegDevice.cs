public static string ReRegisterDrive(IntPtr parm)
		{
			string empty = string.Empty;
			Kernel32API.DEV_BROADCAST_HANDLE dev_BROADCAST_HANDLE = (Kernel32API.DEV_BROADCAST_HANDLE)Marshal.PtrToStructure(parm, typeof(Kernel32API.DEV_BROADCAST_HANDLE));
			if (dev_BROADCAST_HANDLE.dbch_devicetype == 6 && dev_BROADCAST_HANDLE.dbch_handle != IntPtr.Zero && USBNotification.Handles.ContainsKey(dev_BROADCAST_HANDLE.dbch_handle))
			{
				USBNotification usbnotification = USBNotification.Handles[dev_BROADCAST_HANDLE.dbch_handle];
				if (!string.IsNullOrEmpty(usbnotification.drive))
				{
					empty = usbnotification.drive;
				}
			}
			USBNotification.CloseNotifyHandle(parm, null);
			if (!string.IsNullOrEmpty(empty))
			{
				new USBNotification().Register(empty, null, null);
			}
			return empty;
		}
