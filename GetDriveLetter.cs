		public static string GetDriveLetter(IntPtr parm)
		{
			Kernel32API.DEV_BROADCAST_HANDLE dev_BROADCAST_HANDLE = (Kernel32API.DEV_BROADCAST_HANDLE)Marshal.PtrToStructure(parm, typeof(Kernel32API.DEV_BROADCAST_HANDLE));
			if (dev_BROADCAST_HANDLE.dbch_devicetype == 6 && dev_BROADCAST_HANDLE.dbch_handle != IntPtr.Zero && USBNotification.Handles.ContainsKey(dev_BROADCAST_HANDLE.dbch_handle))
			{
				return USBNotification.Handles[dev_BROADCAST_HANDLE.dbch_handle].drive;
			}
			return string.Empty;
		}
