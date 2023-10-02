using TasksAPI.Enums;

namespace TasksAPI.Models
{
	public struct ToDoStatus
	{
		//public ToDoStatusEnum Status { get; set; }
		//public ToDoStatus(DateTime StartDate, DateTime DueDate, IList<SubTask> SubTasks) 
		//{
		//	(DateTime StartDate, DateTime DueDate, IList<SubTask> SubTasks) switch
		//	DateTime now = DateTime.Now;
		//	bool isAnySubtaskUncompleted = SubTasks.Any(subTask => subTask.IsCompleted == false);
		//	if (DueDate < now && isAnySubtaskUncompleted)
		//		return ToDoStatusEnum.Uncompleted;
		//	if (StartDate > now)
		//		return ToDoStatusEnum.Upcoming;
		//	if (DueDate < now && !isAnySubtaskUncompleted)
		//		return ToDoStatusEnum.Completed;
		//	return ToDoStatusEnum.Running;
		//	Status = result;
		//}
	}
}
