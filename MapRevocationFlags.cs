internal static uint MapRevocationFlags(X509RevocationMode revocationMode, X509RevocationFlag revocationFlag)
		{
			uint num = 0U;
			if (revocationMode == X509RevocationMode.NoCheck)
			{
				return num;
			}
			if (revocationMode == X509RevocationMode.Offline)
			{
				num |= 2147483648U;
			}
			if (revocationFlag == X509RevocationFlag.EndCertificateOnly)
			{
				num |= 268435456U;
			}
			else if (revocationFlag == X509RevocationFlag.EntireChain)
			{
				num |= 536870912U;
			}
			else
			{
				num |= 1073741824U;
			}
			return num;
		}
