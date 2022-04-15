using ProCare.API.Core.Helpers;
using ProCare.Common.Data;

using System;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class NoteDTO : ILoadFromDataReader
    {
        public string ID { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string CreateBy { get; set; }
        public string CreateDateTime { get; set; }
        public string NoteText { get; set; }
        
        public void LoadFromDataReader(IDataReader reader)
        {
            ID = reader.GetStringorDefault("SysID");
            Description = reader.GetStringorDefault("DESC");
            Type = reader.GetStringorDefault("Type");
            CreateBy = reader.GetStringorDefault("ChangedBy");
            CreateDateTime = DateTimeHelper.ConvertDateToString( reader.GetDateTimeorDefault("Date", DateTime.MinValue))+" "+ reader.GetStringorDefault("Time");
            NoteText = reader.GetStringorDefault("Text");
           
        }
    }
}
