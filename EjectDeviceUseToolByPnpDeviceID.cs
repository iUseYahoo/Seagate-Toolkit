public static bool EjectDeviceUseToolByPnpDeviceID(string ejectType, string pnpDeviceID)
		{
			if (string.IsNullOrEmpty(ejectType) || string.IsNullOrEmpty(pnpDeviceID))
			{
				return false;
			}
			try
			{
				string currentPath = PathManager.GetCurrentPath();
				string fileName = Path.Combine(new string[]
				{
					currentPath,
					"eject.exe"
				});
				string text = Guid.NewGuid().ToString();
				if (new Process
				{
					StartInfo = new ProcessStartInfo(),
					StartInfo = 
					{
						FileName = fileName,
						Arguments = string.Concat(new string[]
						{
							"\"",
							text,
							"\" \"",
							ejectType,
							"\" \"",
							pnpDeviceID,
							"\""
						}),
						WindowStyle = ProcessWindowStyle.Hidden
					}
				}.Start())
				{
					int i = 0;
					while (i < 60)
					{
						i++;
						if (MainWindowViewModel.Instance.EjectResults.ContainsKey(text))
						{
							bool result = MainWindowViewModel.Instance.EjectResults[text];
							MainWindowViewModel.Instance.EjectResults.Remove(text);
							return result;
						}
						Thread.Sleep(1000);
					}
				}
			}
			catch (Exception)
			{
			}
			return false;
		}
