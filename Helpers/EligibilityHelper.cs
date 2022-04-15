using ProCare.API.PBM.Repository.DTO.Eligibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using ProCare.API.PBM.Messages.Request.Eligibility;

namespace ProCare.API.PBM.Repository.Helpers
{
    public static class EligibilityHelper
    {
        public static bool IsValidPersonAndRelationship(int personCode, int relationshipCode, out string errorString)
        {
            bool isValid = true;
            errorString = "";

            if (personCode < 3)
            {
                if (personCode == 1 && relationshipCode != 1)
                {
                    isValid = false;
                    errorString = "When PERSON = 01, RELCD MUST be 1!";
                }

                if (relationshipCode > 2)
                {
                    isValid = false;
                    errorString = "When PERSON < 03, Relationship MUST be 1 or 2!";
                }
            }

            return isValid;
        }

        public static bool IsValidSpouseChange(MemberDTO oldMember, EligibilityMember requestMember, out string errorString)
        {
            bool isValid = true;
            errorString = "";

            if (!string.IsNullOrWhiteSpace(oldMember.SYSID) && oldMember.DOB != DateTime.Parse(requestMember.DOB) &&
                !oldMember.FNAME.ToUpper().Equals(requestMember.FirstName.ToUpper()))
            {
                errorString = "Invalid Spouse Change";
                isValid = false;
            }

            return isValid;
        }

        public static bool IsValidSpouseAge(DateTime? dob, int minimumSpouseAge, out string errorString)
        {
            bool isValid = true;
            errorString = "";

            if (dob.HasValue && dob > DateTime.Now.AddYears(-minimumSpouseAge))
            {
                errorString = String.Format("Spouse less than {0} years of Age", minimumSpouseAge);
                isValid = false;
            }

            return isValid;
        }

        public static bool IsNonDuplicateCardID(MemberDTO oldMember, EligibilityMember requestMember, out string errorString)
        {
            bool isDuplicate = false;

            errorString = "";

            if (!string.IsNullOrWhiteSpace(oldMember.LNAME) && !oldMember.LNAME.Equals(requestMember.LastName.ToUpper()))
            {
                if (!string.IsNullOrWhiteSpace(oldMember.FNAME) && !oldMember.FNAME.Equals(requestMember.FirstName.ToUpper()))
                {
                    if (!string.IsNullOrWhiteSpace(oldMember.SEX) && !oldMember.SEX.Equals(requestMember.Gender.ToUpper()))
                    {
                        if (!(oldMember.DOB == DateTime.Parse(requestMember.DOB)))
                        {
                            isDuplicate = true;
                            errorString = "Duplicate Card ID for different person";
                        }
                    }
                }
            }

            return isDuplicate;
        }

        public static bool IsValidDOB(DateTime dob, bool dobPresent, out string warningMessage, out string errorMessage)
        {
            warningMessage = "";
            errorMessage = "";
            bool valid = true;

            if (dob < DateTime.Now.AddYears(-120) || dob > DateTime.Now.AddDays(1))
            {
                errorMessage = "Missing/Invalid Date of Birth";
                valid = false;
            }

            if (!dobPresent)
            {
                if (dob == DateTime.MinValue)
                {
                    warningMessage = "Missing/Invalid Date of Birth";
                    errorMessage = "";
                    valid = true;
                }
            }

            return valid;
        }

        public static DateTime ForceValidDateTime(string dateString)
        {
            return DateTime.TryParse(dateString, out DateTime dt) ? dt : DateTime.MinValue;
        }

        public static DateTime ForceValidDateTime(string dateString, DateTime defaultValue)
        {
            return DateTime.TryParse(dateString, out DateTime dt) ? dt : defaultValue;
        }

        public static bool IsValidEffectiveDate(DateTime effectiveDate, out string errorMessage)
        {
            errorMessage = "";
            bool valid = true;

            if (effectiveDate < DateTime.Now.AddYears(-120) || effectiveDate > DateTime.Now.AddYears(120))
            {
                errorMessage = "M/I Effective Date";
                valid = false;
            }

            return valid;
        }

        public static bool IsValidTerminationDate(DateTime terminationDate, DateTime effectiveDate, out string errorMessage)
        {
            errorMessage = "";
            bool valid = true;

            if (terminationDate != DateTime.MinValue)
            {
                if (terminationDate < DateTime.Now.AddYears(120))
                {
                    if (terminationDate < effectiveDate)
                    {
                        valid = false;
                    }
                }
                else
                {
                    valid = false;
                }
            }

            if (!valid)
            {
                errorMessage = "M/I Termination Date";
            }

            return valid;
        }

        private static Stream generateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static List<string> ValidateXML(string rawData, string xsd)
        {
            List<string> errors = new List<string>();
            XmlSchema compiledSchema = null;

            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add("", XmlReader.Create(generateStreamFromString(xsd)));

            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                compiledSchema = schema;
            }

            var settings = new XmlReaderSettings();
            settings.Schemas.Add(compiledSchema);

            XDocument doc = XDocument.Load(generateStreamFromString(rawData));
            doc.Validate(schemaSet, (sender, args) =>
            {
                if (args.Severity == XmlSeverityType.Warning)
                {
                    errors.Add("Warning: Matching schema not found.  No validation occurred." + args.Message);
                }
                else
                {
                    errors.Add("Validation error: " + args.Message);
                }
            });

            Stream result = generateStreamFromString(rawData);
            XmlReader vreader = XmlReader.Create(result, settings);

            while (vreader.Read())
            {

            }

            vreader.Close();

            return errors;
        }
    }
}
