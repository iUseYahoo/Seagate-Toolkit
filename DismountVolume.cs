		private static bool DismountVolume(IntPtr handle)
		{
			uint num;
			return EjectDevice.DeviceIoControl(handle, 589856U, IntPtr.Zero, 0U, IntPtr.Zero, 0U, out num, IntPtr.Zero);
		}
