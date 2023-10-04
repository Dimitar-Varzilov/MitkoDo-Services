using TasksAPI.Enums;
using TasksAPI.Models;

namespace TasksAPI
{
	public static class Utilities
	{
		public static ToDoStatusEnum CalculateTaskStatus(ToDo toDo)
		{
			return CalculateTaskStatus(toDo.StartDate, toDo.DueDate, toDo.SubTasks);
		}

		public static ToDoStatusEnum CalculateTaskStatus(DateTime StartDate, DateTime DueDate, ICollection<SubTask> SubTasks)
		{
			DateTime now = DateTime.Now;
			bool isAnySubtaskUncompleted = SubTasks.Any(subTask => subTask.IsCompleted == false);
			if (DueDate < now && isAnySubtaskUncompleted)
				return ToDoStatusEnum.Uncompleted;
			if (StartDate > now)
				return ToDoStatusEnum.Upcoming;
			if (DueDate < now && !isAnySubtaskUncompleted)
				return ToDoStatusEnum.Completed;
			return ToDoStatusEnum.Running;
		}

		public static bool IsTaskCompleted(ToDo toDo)
		{
			DateTime now = DateTime.Now;
			return toDo.SubTasks.All(subTask => CalculateSubTaskStatus(subTask) == true) && toDo.DueDate > now;
		}

		public static bool CalculateSubTaskStatus(SubTask SubTask)
		{
			bool result = SubTask.Notes.Count >= SubTask.NotesCountToBeCompleted && SubTask.Pictures.Count >= SubTask.PicturesCountToBeCompleted;
			return result;
		}

		public static bool IsTaskActive(ToDo toDo)
		{
			DateTime now = DateTime.Now;
			bool result = toDo.StartDate < now && toDo.DueDate > now;
			return result;
		}

		public static List<Guid> GenerateGuids(int count)
		{
			var guids = new List<Guid>();

			for (int i = 0; i < count; i++)
			{
				guids.Add(Guid.NewGuid());
			}

			File.WriteAllLines("D:\\coding\\Diadraw\\MitkoDo\\20Guids.txt", guids.ConvertAll(x => x.ToString()));

			return guids;
		}
	}
}
