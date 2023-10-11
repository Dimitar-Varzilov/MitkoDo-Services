namespace EmployeeWorker
{
	using EmployeeWorker.Contracts;
	using System;
	public static class Utilities
	{
		public static bool CalculateTaskActive(ToDo toDo)
		{
			return CalculateTaskActive(toDo.StartDate, toDo.DueDate);
		}

		public static bool CalculateTaskActive(DateTime StartDate, DateTime DueDate)
		{
			DateTime now = DateTime.Now;
			bool result = StartDate < now && DueDate > now;
			return result;
		}
	}
}
