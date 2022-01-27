using System;
using CommonUtils;

namespace ServiceControl
{
	// Token: 0x02000002 RID: 2
	internal class Program
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static void Main(string[] args)
		{
			PathManager.Init();
			if (args.Length != 0 && args[0] == "/u")
			{
				ServiceManager.StopAndUninstallService();
				return;
			}
			if (!ServiceManager.IsServiceAvailable())
			{
				ServiceManager.InstallAndStartService();
			}
		}
	}
}
