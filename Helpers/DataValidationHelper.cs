using Newtonsoft.Json;
using ProCare.API.PBM.Repository;
using ProCare.API.PBM.Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProCare.API.PBM.Messages.Shared.Enums;

namespace ProCare.API.PBM.Helpers
{
    public static class DataValidationHelper
    {
        private static string invalidMemberException = "Invalid member.  Active member could not be found.";

        public static void ValidatePharmacy(IPharmacyRepository pharmacyRepository, string clientConnectionString, string requestID, string templateID)
        {
            string pharmacyID = !string.IsNullOrWhiteSpace(requestID) ? requestID : templateID;

            if (!string.IsNullOrWhiteSpace(pharmacyID))
            {
                bool pharmacyValid = IsValidPharmacy(pharmacyRepository, clientConnectionString, pharmacyID, out string errorMessage);

                if (!pharmacyValid)
                {
                    throw new ArgumentException(errorMessage);
                }
            }
        }

        public static async Task<string> GetEnrolleeId(IPbmRepository pbmRepository, string clientConnectionString, MemberIDType memberIdType, string planId, string memberId, string person, string memberEnrolleeId)
        {
            string enrolleeId = memberEnrolleeId;

            //Get member EnrolleeID if MemberEnrolleeID not provided
            if (string.IsNullOrWhiteSpace(enrolleeId))
            {
                enrolleeId = await GetEnrolleeId(pbmRepository, clientConnectionString, memberIdType, planId, memberId).ConfigureAwait(false);

                enrolleeId = await pbmRepository.GetMemberEnrolleeIdWithPerson(clientConnectionString, planId, enrolleeId, person)
                                             .ConfigureAwait(false);
            }
            if (string.IsNullOrWhiteSpace(enrolleeId))
            {
                throw new ArgumentException(invalidMemberException);
            }

            return enrolleeId; 
        }

        public static async Task<List<DatasetEnrolleeIDDTO>> GetDatasetEnrolleeIds(IPbmRepository pbmRepository, List<DatasetDTO> datasets, string planId, string memberId, string person)
        {
            Task<List<DatasetEnrolleeIDDTO>> t = Task.Run(() =>
            {
                List<DatasetEnrolleeIDDTO> dbResults = new List<DatasetEnrolleeIDDTO>();

                datasets.ForEach(dataset =>
                {
                    try
                    {
                        string enrolleeId = GetEnrolleeId(pbmRepository, ClientConfigHelper.GetDatasetConnectionString(dataset), dataset.MemberIdType, planId, memberId, person).Result;

                        if (!string.IsNullOrWhiteSpace(enrolleeId))
                        {
                            dbResults.Add(new DatasetEnrolleeIDDTO(dataset, enrolleeId));
                        }
                    }
                    catch (Exception) { }
                });

                return dbResults;
            }
            );

            List<DatasetEnrolleeIDDTO> results = await t.ConfigureAwait(false);

            return results;
        }

        public static async Task<string> GetEnrolleeId(IPbmRepository pbmRepository, string clientConnectionString, MemberIDType memberIdType, string planId, string memberId)
        {
            List<string> enrolleeIds = await GetEnrolleeIds(pbmRepository, clientConnectionString, memberIdType, planId, memberId)
                                             .ConfigureAwait(false);

            return enrolleeIds.First();
        }

        public static async Task<List<string>> GetEnrolleeIds(IPbmRepository pbmRepository, string clientConnectionString, MemberIDType memberIdType, string planId, string memberId)
        {
            List<string> enrolleeIds = await pbmRepository.GetMemberEnrolleeId(clientConnectionString, memberIdType, planId, memberId)
                                             .ConfigureAwait(false);

            if (!enrolleeIds.Any())
            {
                throw new ArgumentException(invalidMemberException);
            }

            return enrolleeIds;
        }

        public static async Task<List<string>> GetEnrolleeIds(IPbmRepository pbmRepository, string clientConnectionString, MemberIDType memberIdType, string planId, string memberId, string memberEnrolleeId)
        {
            List<string> enrolleeIds = new List<string>();

            //Get member EnrolleeID if MemberEnrolleeID not provided
            if (string.IsNullOrWhiteSpace(memberEnrolleeId))
            {
                enrolleeIds = await GetEnrolleeIds(pbmRepository, clientConnectionString, memberIdType, planId, memberId)
                                             .ConfigureAwait(false);
            }
            else
            {
                enrolleeIds.Add(memberEnrolleeId);
            }

            if (!enrolleeIds.Any())
            {
                throw new ArgumentException(invalidMemberException);
            }

            return enrolleeIds;
        }


        public static async Task<string> GetEnrolleeId(IPbmRepository pbmRepository, string clientConnectionString, MemberIDType memberIdType, string planId, string memberId, string memberEnrolleeId)
        {
            string enrolleeId = memberEnrolleeId;

            //Get member EnrolleeID if MemberEnrolleeID not provided
            if (string.IsNullOrWhiteSpace(enrolleeId))
            {
                enrolleeId = await GetEnrolleeId(pbmRepository, clientConnectionString, memberIdType, planId, memberId)
                                             .ConfigureAwait(false);
            }

            if (string.IsNullOrWhiteSpace(enrolleeId))
            {
                throw new ArgumentException(invalidMemberException);
            }

            return enrolleeId;
        }

        public static bool IsValidPlanID(IEligibilityRepository eligbilityRepository, string adsConnectionString, string planID, out string errorMessage, bool useACPMessage = false)
        {
            errorMessage = "";
            bool valid = true;

            if (!eligbilityRepository.PlanExists(adsConnectionString, planID))
            {
                if (useACPMessage)
                {
                    errorMessage = "Invalid Plan ID";
                }
                else
                {
                    errorMessage = "M/I Plan ID";
                }

                valid = false;
            }

            return valid;
        }

        public static bool IsValidCardID(IEligibilityRepository eligibilityRepository, string adsConnectionString, string planID, string cardID, string person, out string errorMessage, bool useACPMessage = false)
        {
            errorMessage = "";
            bool valid = true;

            if (!(eligibilityRepository.GetMembersByPlanIDCardIDCardID2Person(adsConnectionString, planID, GetCardID(cardID), GetCardID2(cardID), person).Count > 0))
            {
                if (useACPMessage)
                {
                    errorMessage = "PlnId+CardId(1&2)+Person, Not In ApsEnr";
                }
                else
                {
                    errorMessage = "M/I Card ID";
                }

                valid = false;
            }

            return valid;
        }

        public static bool IsValidPharmacy(IPharmacyRepository pharmacyRepository, string adsConnectionString, string pharmacyID, out string errorMessage)
        {
            errorMessage = "";
            bool valid = true;

            if (!pharmacyRepository.PharmacyExists(adsConnectionString, pharmacyID).Result)
            {
                errorMessage = "Invalid Pharmacy";

                valid = false;
            }

            return valid;
        }

        public static bool IsValidMember(IPbmRepository pbmRepository, string adsConnectionString, string planId, string enrolleeId, out string errorMessage)
        {
            errorMessage = "";
            bool valid = true;

            if (!pbmRepository.MemberExists(adsConnectionString, planId, enrolleeId).Result)
            {
                errorMessage = $"Member {enrolleeId} not found in Plan {planId}";

                valid = false;
            }

            return valid;
        }

        public static bool IsPlanInDate(IEligibilityRepository eligibilityRepository, string adsConnectionString, string planID, DateTime adjustmentDate, out string errorMessage)
        {
            errorMessage = "";
            bool valid = true;

            if (!eligibilityRepository.PlanInDate(adsConnectionString, planID, adjustmentDate))
            {
                errorMessage = string.Format("There was no plan effective as of {0}", adjustmentDate.ToShortDateString());

                valid = false;
            }

            return valid;
        }

        public static SearchTypeOperator? GetSearchType(string fieldValue, string operatorValue)
        {
            SearchTypeOperator? searchOperator = null;

            if (!string.IsNullOrWhiteSpace(fieldValue))
            {
                if (!string.IsNullOrWhiteSpace(operatorValue))
                {
                    searchOperator = (SearchTypeOperator)operatorValue.ToUpper()[0];
                }
                else
                {
                    searchOperator = SearchTypeOperator.StartsWith;
                }
            }

            return searchOperator;
        }

        public static string GetSearchTypeString(SearchTypeOperator? searchTypeOperator)
        {
            return searchTypeOperator != null ? ((char)searchTypeOperator).ToString() : null;
        }

        public static T CloneJson<T>(T source)
        {
            //Return default for null objects
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }

        public static string GetCardID(string fullCardID)
        {
            return fullCardID.Substring(0, 9);
        }

        public static string GetCardID2(string fullCardID)
        {
            return fullCardID.Substring(9);
        }
    }
}
