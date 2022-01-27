// Kernel32API 
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint UnregisterDeviceNotification(IntPtr hHandle);
    
//inside the exe code
		private void Unregister(Log _log = null)
		{
			if (this.notificationHandle != IntPtr.Zero)
			{
				Kernel32API.UnregisterDeviceNotification(this.notificationHandle);
				this.notificationHandle = IntPtr.Zero;
			}
		}
