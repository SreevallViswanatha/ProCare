using ServiceStack;
using System.Collections.Generic;
using ProCare.NCPDP.Telecom;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestCompound
    {
        /// <summary>
        ///     Field Number: 451-EG
        ///     <para />
        ///     Description: NCPDP standard product billing codes.
        ///     <para />
        ///     Format: 9(01)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        /// </summary>
        [ApiMember(Name = "DispensingUnitFormIndicator", Description = FieldDescriptions.DispensingUnitFormIndicator, DataType = "", IsRequired = true)]
        public DispensingUnitFormIndicator DispensingUnitFormIndicator { get; set; }

        /// <summary>
        ///     Field Number: 450-EF
        ///     <para />
        ///     Description: Dosage form of the complete compound mixture.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        /// </summary>
        [ApiMember(Name = "DosageFormDescriptionCode", Description = FieldDescriptions.DosageFormDescriptionCode, DataType = "String", IsRequired = true)]
        public DosageFormDescriptionCode DosageFormDescriptionCode { get; set; }

        /// <summary>
        ///     Field Number: 447-EC
        ///     <para />
        ///     Description: Count of compound product IDs in the compound mixture.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        ///     Maximum count of 25 ingredients.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "IngredientCount", Description = FieldDescriptions.IngredientCount, DataType = "Int", IsRequired = true)]
        public int IngredientCount => Ingredients.Count;

        public List<CompoundProduct> Ingredients { get; set; }

        /// <summary>
        ///     Field Number: 362-2G
        ///     <para />
        ///     Description: Code indicating the number of Compound Ingredient Modifier Code(363-2H).
        ///     <para />
        ///     Format: 9(2)
        ///     <para />
        ///     Designation: Qualified
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required when Compound Ingredient Modifier Code(363-2H) is sent.
        ///     <para />
        ///     Maximum count of 1Ø.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "ModifiedCodeCount", Description = FieldDescriptions.ModifiedCodeCount, DataType = "Int", IsRequired = false)]
        public int ModifiedCodeCount => ModifierCodes.Count;

        /// <summary>
        ///     Field Number: 363-2H
        ///     <para />
        ///     Description: Identifies special circumstances related to the dispensing/payment of the product as identified in the 
        ///     Compound Product ID(498-TE).
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Claim Rebill:
        ///     Required if necessary for state/federal/regulatory agency programs.
        ///     <para />
        ///     CMS code set of HCPCS modifiers
        ///     <para />
        /// </summary>
        [ApiMember(Name = "ModifierCodes", Description = FieldDescriptions.ModifierCodes, DataType = "List<String>", IsRequired = false)]
        public List<string> ModifierCodes { get; set; }

        /// <summary>
        ///     Field Number: 452-EH
        ///     <para />
        ///     Description: Code for the route of administration of the complete compound mixture.
        ///     <para />
        ///     Format: 9(02)
        ///     <para />
        ///     Designation: Mandatory
        ///     <para />
        /// </summary>
        [ApiMember(Name = "RouteOfAdministration", Description = FieldDescriptions.RouteOfAdministration, DataType = "", IsRequired = true)]
        public RouteOfAdministration RouteOfAdministration { get; set; }

    }
    
    public class CompoundProduct
    {
        /// <summary>
        ///     490-UE
        /// </summary>
        /// <summary>
        ///     Field Number:
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation:
        ///     <para />
        /// </summary>
        public string BasisOfCostDetermination { get; set; } = string.Empty;

        /// <summary>
        ///     449-EE
        /// </summary>
        /// <summary>
        ///     Field Number:
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation:
        ///     <para />
        /// </summary>
        public decimal? Cost { get; set; }

        /// <summary>
        ///     489-TE
        /// </summary>
        /// <summary>
        ///     Field Number:
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation:
        ///     <para />
        /// </summary>
        public string ProductID { get; set; } = string.Empty;

        /// <summary>
        ///     488-RE
        /// </summary>
        /// <summary>
        ///     Field Number:
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation:
        ///     <para />
        /// </summary>
        public string ProductIDQualifier { get; set; } = string.Empty;

        /// <summary>
        ///     448-ED
        /// </summary>
        /// <summary>
        ///     Field Number:
        ///     <para />
        ///     Description:
        ///     <para />
        ///     Format:
        ///     <para />
        ///     Designation:
        ///     <para />
        /// </summary>
        public decimal? Quantity { get; set; }
    }
}
