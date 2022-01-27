public static RemovableMediaAction GetRemovableMediaAction(RemovableMedia targetRM, RemovableMedia previousRM)
		{
			RemovableMediaAction result = RemovableMediaAction.Null;
			if (targetRM == null || previousRM == null)
			{
				return result;
			}
			if (targetRM.Size == null && previousRM.Size != null)
			{
				result = RemovableMediaAction.Remove;
			}
			else if (targetRM.Size != null && previousRM.Size == null)
			{
				result = RemovableMediaAction.Connect;
			}
			return result;
		}
