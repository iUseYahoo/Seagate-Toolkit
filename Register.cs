private bool Register(string device, string volSn = null, Log _log = null)
		{
			if (USBNotification.windowHandle == IntPtr.Zero)
			{
				return false;
			}
			Kernel32API.DEV_BROADCAST_HANDLE dev_BROADCAST_HANDLE = default(Kernel32API.DEV_BROADCAST_HANDLE);
			dev_BROADCAST_HANDLE.dbch_devicetype = 6;
			dev_BROADCAST_HANDLE.dbch_size = Marshal.SizeOf<Kernel32API.DEV_BROADCAST_HANDLE>(dev_BROADCAST_HANDLE);
			this.directoryHandle = this.CreateFileHandle(device);
			dev_BROADCAST_HANDLE.dbch_handle = this.directoryHandle;
			dev_BROADCAST_HANDLE.dbch_eventguid = Guid.NewGuid();
			if (this.directoryHandle != IntPtr.Zero)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(dev_BROADCAST_HANDLE.dbch_size);
				Marshal.StructureToPtr<Kernel32API.DEV_BROADCAST_HANDLE>(dev_BROADCAST_HANDLE, intPtr, true);
				this.notificationHandle = Kernel32API.RegisterDeviceNotification(USBNotification.windowHandle, intPtr, 0);
				this.drive = device;
				if (volSn == null)
				{
					this.volNumber = Utility.GetVolumnSN(this.drive);
				}
				else
				{
					this.volNumber = volSn;
				}
				if (this.notificationHandle != IntPtr.Zero)
				{
					if (!USBNotification.Handles.ContainsKey(this.notificationHandle) && !USBNotification.Handles.ContainsKey(this.directoryHandle))
					{
						USBNotification.Handles.Add(this.directoryHandle, this);
					}
					return true;
				}
			}
			return false;
		}
