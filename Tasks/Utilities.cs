using Tasks.Enums;
using Tasks.Models;

namespace Tasks
{
	public static class Utilities
	{
		public static CustomTaskStatus CalculateTaskStatus(CustomTask customTask)
		{
			CustomTask customTask1 = customTask;
			DateTime now = DateTime.Now;
			bool isAnySubtaskUncompleted = customTask.SubTasks.Any(subTask => subTask.IsCompleted == false);
			if (customTask1.DueDate < now && isAnySubtaskUncompleted)
				return CustomTaskStatus.Uncompleted;
			if (customTask1.StartDate > now)
				return CustomTaskStatus.Upcoming;
			if (customTask1.DueDate < now && !isAnySubtaskUncompleted)
				return CustomTaskStatus.Completed;
			return CustomTaskStatus.Running;
		}

		public static CustomTaskStatus CalculateTaskStatus(DateTime StartDate, DateTime DueDate, ICollection<SubTask> SubTasks)
		{
			DateTime now = DateTime.Now;
			bool isAnySubtaskUncompleted = SubTasks.Any(subTask => subTask.IsCompleted == false);
			if (DueDate < now && isAnySubtaskUncompleted)
				return CustomTaskStatus.Uncompleted;
			if (StartDate > now)
				return CustomTaskStatus.Upcoming;
			if (DueDate < now && !isAnySubtaskUncompleted)
				return CustomTaskStatus.Completed;
			return CustomTaskStatus.Running;
		}

		public static bool CalculateSubTaskStatus(SubTask SubTask)
		{
			bool result = SubTask.Notes.Count >= SubTask.NotesRequired && SubTask.Pictures.Count >= SubTask.PicsRequired;
			return result;
		}

		public static bool IsTaskActive (CustomTask customTask)
		{
			DateTime now = DateTime.Now;
			bool result = customTask.StartDate < now && customTask.DueDate > now;
			return result;
		}
	}
}
