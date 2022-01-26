		public string Eject(bool allowUI)
		{
			foreach (Device device in this.RemovableDevices)
			{
				if (allowUI)
				{
					Native.CM_Request_Device_Eject_NoUi(device.InstanceHandle, IntPtr.Zero, null, 0, 0);
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder(1024);
					Native.PNP_VETO_TYPE pnp_VETO_TYPE;
					int num = Native.CM_Request_Device_Eject(device.InstanceHandle, out pnp_VETO_TYPE, stringBuilder, stringBuilder.Capacity, 0);
					this._log.I(string.Format("CM_Request_Device_Eject: result->{0} veto->{1}", num, pnp_VETO_TYPE), new object[0]);
					if (num != 0)
					{
						return null;
					}
					return pnp_VETO_TYPE.ToString();
				}
			}
			return null;
		}
