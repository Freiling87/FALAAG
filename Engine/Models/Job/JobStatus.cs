using Engine.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
	public class JobStatus : INotifyPropertyChanged
	{
		private bool _isCompleted;

		public event PropertyChangedEventHandler PropertyChanged;
		public bool IsCompleted 
		{ 
			get => _isCompleted;
			set
			{
				_isCompleted = value;
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