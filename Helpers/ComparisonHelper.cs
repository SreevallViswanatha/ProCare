using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProCare.API.PBM.Repository.Helpers
{
    public static class ComparisonHelper
    {
        public static bool NullableIntsAreEqual(int? a, int? b)
        {
            bool equal = true;

            if (
                    (a.HasValue != b.HasValue) ||
                    (a.HasValue && b.HasValue && a.Value != b.Value)
                )
            {
                equal = false;
            }

            return equal;
        }

        public static bool NullableDateTimesAreEqual(DateTime? a, DateTime? b)
        {
            bool equal = true;

            if (
                    (a.HasValue != b.HasValue) ||
                    (a.HasValue && b.HasValue && a.Value != b.Value)
                )
            {
                equal = false;
            }

            return equal;
        }
    }
}
