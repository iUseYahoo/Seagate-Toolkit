public static RemovableMedia GetRemovableMedia(ManagementBaseObject moDisk)
		{
			if (moDisk == null)
			{
				return null;
			}
			try
			{
				RemovableMedia removableMedia = new RemovableMedia();
				RemovableMedia removableMedia2 = removableMedia;
				object obj = moDisk["Caption"];
				removableMedia2.Caption = ((obj != null) ? obj.ToString() : null);
				RemovableMedia removableMedia3 = removableMedia;
				object obj2 = moDisk["Description"];
				removableMedia3.Description = ((obj2 != null) ? obj2.ToString() : null);
				RemovableMedia removableMedia4 = removableMedia;
				object obj3 = moDisk["DeviceID"];
				removableMedia4.DeviceID = ((obj3 != null) ? obj3.ToString() : null);
				RemovableMedia removableMedia5 = removableMedia;
				object obj4 = moDisk["DriveType"];
				removableMedia5.DriveType = ((obj4 != null) ? obj4.ToString() : null);
				RemovableMedia removableMedia6 = removableMedia;
				object obj5 = moDisk["FileSystem"];
				removableMedia6.FileSystem = ((obj5 != null) ? obj5.ToString() : null);
				if (moDisk["FreeSpace"] != null)
				{
					removableMedia.Size = new long?(Convert.ToInt64(moDisk["FreeSpace"].ToString()));
				}
				else
				{
					removableMedia.Size = null;
				}
				RemovableMedia removableMedia7 = removableMedia;
				object obj6 = moDisk["Name"];
				removableMedia7.Name = ((obj6 != null) ? obj6.ToString() : null);
				RemovableMedia removableMedia8 = removableMedia;
				object obj7 = moDisk["PNPDeviceID"];
				removableMedia8.PNPDeviceID = ((obj7 != null) ? obj7.ToString() : null);
				if (moDisk["Size"] != null)
				{
					removableMedia.Size = new long?(Convert.ToInt64(moDisk["Size"].ToString()));
				}
				else
				{
					removableMedia.Size = null;
				}
				RemovableMedia removableMedia9 = removableMedia;
				object obj8 = moDisk["VolumeDirty"];
				removableMedia9.VolumeDirty = bool.Parse(((obj8 != null) ? obj8.ToString() : null) ?? "False");
				RemovableMedia removableMedia10 = removableMedia;
				object obj9 = moDisk["VolumeName"];
				removableMedia10.VolumeName = ((obj9 != null) ? obj9.ToString() : null);
				RemovableMedia removableMedia11 = removableMedia;
				object obj10 = moDisk["VolumeSerialNumber"];
				removableMedia11.VolumeSerialNumber = ((obj10 != null) ? obj10.ToString() : null);
				return removableMedia;
			}
			catch (Exception ex)
			{
				Device._log.E("GetRemovableMedia ex: {0}", new object[]
				{
					ex.ToString()
				});
			}
			return null;
		}
