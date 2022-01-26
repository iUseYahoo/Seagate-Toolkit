		public static bool Eject(IntPtr handle)
		{
			bool result = false;
			if (EjectDevice.DismountVolume(handle))
			{
				EjectDevice.PreventRemovalOfVolume(handle, false);
				result = EjectDevice.AutoEjectVolume(handle);
			}
			EjectDevice.CloseHandle(handle);
			return result;
		}
