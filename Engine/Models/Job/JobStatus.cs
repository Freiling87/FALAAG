using Engine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
	public class JobStatus : BaseNotificationClass
	{
		private bool _isCompleted;

		public bool IsCompleted 
		{ 
			get => _isCompleted;
			set
			{
				_isCompleted = value;
				OnPropertyChanged();
			}
		}
		public Job Job { get; }

		public JobStatus(Job job)
		{
			Job = job;
			IsCompleted = false;
		}
	}
}