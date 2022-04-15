using ProCare.Common.Data;
using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class PBMSchedulerServiceTaskDTO : ILoadFromDataReader
    {
        public int PBMSchedulerServiceTaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime TerminationDate { get; set; }
        public bool Enabled { get; set; }
        public int TaskTimerInSeconds { get; set; }
        public string TaskWindowStartTime { get; set; }
        public string TaskWindowEndTime { get; set; }
        public string APIEndPoint { get; set; }
        public int APITimeoutInSeconds { get; set; }
        public string ClientGuid { get; set; }
        public string ClientID { get; set; }


        public void LoadFromDataReader(IDataReader reader)
        {
            PBMSchedulerServiceTaskId = reader.GetInt32orDefault("PBMSchedulerServiceTaskID");
            TaskName = reader.GetStringorDefault("TaskName");
            Description = reader.GetStringorDefault("Description");
            EffectiveDate = reader.GetDateTimeorDefault("EffectiveDate", DateTime.MinValue);
            TerminationDate = reader.GetDateTimeorDefault("TerminationDate", DateTime.MaxValue);
            Enabled = reader.GetBooleanSafe("Enabled");
            TaskTimerInSeconds = reader.GetInt32orDefault("TaskTimerInSeconds");
            TaskWindowStartTime = reader.GetStringorDefault("TaskWindowStartTime");
            TaskWindowEndTime = reader.GetStringorDefault("TaskWindowEndTime");
            APIEndPoint = reader.GetStringorDefault("APIEndPoint");
            APITimeoutInSeconds = reader.GetInt32orDefault("APITimeoutInSeconds");
            ClientGuid = reader.GetStringorNull("ClientGuid");
            ClientID = reader.GetStringorNull("ClientID");
        }
    }
}
