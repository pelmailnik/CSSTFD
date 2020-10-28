using System;
using Microsoft.Win32.TaskScheduler;

namespace STFD
{
    public class ShedulerTask
    {
        private const string TaskName = "STFD";
        private string _hours;
        private string _minutes;
        private string _seconds;

        public void SetTime(string hours, string minutes, string seconds)
        {
            _hours = hours;
            _minutes = minutes;
            _seconds = seconds;
        }

        public void ToPlan(int flag)
        {
            // Create a new task definition and assign properties
            TaskDefinition td = TaskService.Instance.NewTask();
            td.RegistrationInfo.Description = "Task for STFD";
            td.Principal.LogonType = TaskLogonType.InteractiveToken;

            td.Actions.Clear();

            if (flag == 0)
            {
                // Add an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction("c:\\windows\\system32\\shutdown.exe", "-s", "c:\\windows\\system32"));

            }

            if (flag == 1)
            {
                td.Actions.Add(new ExecAction("c:\\windows\\system32\\rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0", "c:\\windows\\system32"));
            }

            td.Triggers.Clear();

            // Add a trigger that will fire one time
            td.Triggers.Add(new TimeTrigger
            {
                StartBoundary = GetTime()
            });

            // Register the task in the root folder

            TaskService.Instance.RootFolder.RegisterTaskDefinition(TaskName, td);
        }
        public bool IsTaskExist()
        {
            return TaskService.Instance.RootFolder.Tasks.Exists(TaskName);
        }

        public void DeleteTask()
        {
            if (IsTaskExist())
            {
                TaskService.Instance.RootFolder.DeleteTask(TaskName);
            }
        }

        public DateTime GetTaskTime()
        {
            var task = TaskService.Instance.FindTask(TaskName, false);
            return task.NextRunTime;
        }

        private DateTime GetTime()
        {
            var date = DateTime.Now;

            date = date.AddHours(ConvertValue(_hours));
            date = date.AddMinutes(ConvertValue(_minutes));
            date = date.AddSeconds(ConvertValue(_seconds));

            return date;
        }

        private static double ConvertValue(string value)
        {
            return value == "" ? 0.0 : Convert.ToDouble(value);
        }
    }
}