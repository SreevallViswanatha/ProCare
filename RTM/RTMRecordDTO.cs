using System.Collections.Generic;
using System.Data;

namespace ProCare.API.PBM.Repository.DTO
{
    public class RTMRecordDTO 
    {
        public RTMRecordDTO()
        {
            Fields = new List<RTMField>();
        }
        public List<RTMField> Fields { get; set; }

        public void LoadFromDataReader(IDataReader reader, List<string> masterFieldList, List<string> clientFieldList)
        {
            foreach (string columnName in masterFieldList)
            {
                RTMField field = new RTMField();
                field.Name = columnName;
                field.Value = reader[columnName];

                if (clientFieldList.Contains(columnName))
                {
                    field.IncludeField = true;
                }
                else
                {
                    field.IncludeField = false;
                }

                Fields.Add(field);
            }
        }

        public void LoadEmptyFields(List<string> masterFields, List<string> MiscFields)
        {
            foreach (string fieldName in masterFields)
            {
                RTMField field = new RTMField();
                field.Name = fieldName;
                field.Value = string.Empty;

                if (MiscFields.Contains(fieldName))
                {
                    field.IncludeField = true;
                }
                else
                {
                    field.IncludeField = false;
                }

                Fields.Add(field);
            }
        }

    }


}
