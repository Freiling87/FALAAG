using System.ComponentModel;

namespace FALAAG.Models
{
	public class JobStatus : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public bool IsCompleted { get; set; }
		public Job Job { get; }

		public JobStatus(Job job)
		{
			Job = job;
			IsCompleted = false;
		}
	}
}