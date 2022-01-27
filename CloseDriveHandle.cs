public static void CloseDriveHandle(IntPtr parm)
		{
			Kernel32API.DEV_BROADCAST_HANDLE dev_BROADCAST_HANDLE = (Kernel32API.DEV_BROADCAST_HANDLE)Marshal.PtrToStructure(parm, typeof(Kernel32API.DEV_BROADCAST_HANDLE));
			if (dev_BROADCAST_HANDLE.dbch_devicetype == 6 && dev_BROADCAST_HANDLE.dbch_handle != IntPtr.Zero && USBNotification.Handles.ContainsKey(dev_BROADCAST_HANDLE.dbch_handle))
			{
				USBNotification usbnotification = USBNotification.Handles[dev_BROADCAST_HANDLE.dbch_handle];
				if (usbnotification.directoryHandle != IntPtr.Zero)
				{
					Kernel32API.CloseHandle(usbnotification.directoryHandle);
					usbnotification.directoryHandle = IntPtr.Zero;
				}
			}
		}
