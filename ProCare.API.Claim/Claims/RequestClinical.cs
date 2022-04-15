using ServiceStack;
using System;
using System.Collections.Generic;
using ProCare.API.Core;

namespace ProCare.API.Claims.Claims
{
    public class RequestClinical
    {
        public List<DiagnosisCodes> DiagnosisCodes { get; set; }

        public List<Measurements> Measurements { get; set; }


    }

    public class DiagnosisCodes
    {
        /// <summary>
        ///     Field Number: 424-DO
        ///     <para />
        ///     Description: Code identifying the diagnosis of the patient.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     The value for this field is obtained from the prescriber or
        ///     authorized representative.
        ///     <para />
        ///     Required if this field could result in different coverage,
        ///     pricing, patient financial responsibility, and/or drug
        ///     utilization review outcome.
        ///     <para />
        ///     Required if this field affects payment for professional
        ///     pharmacy service.
        ///     <para />
        ///     Required if this information can be used in place of prior
        ///     authorization.
        ///     <para />
        ///     Required if necessary for state/federal/regulatory agency
        ///     programs.
        /// </summary>
        [ApiMember(Name = "DiagnosisCode", Description = FieldDescriptions.DiagnosisCode, DataType = "string", IsRequired = false)]
        public string DiagnosisCode { get; set; }

        /// <summary>
        ///     Field Number: 492-WE
        ///     <para />
        ///     Description: Code qualifying the Diagnosis Code.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Diagnosis Code(424-DO) is used.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "DiagnosisCodeQualifier", Description = FieldDescriptions.DiagnosisCodeQualifier, DataType = "string", IsRequired = false)]
        public string DiagnosisCodeQualifier { get; set; }
    }

    public class Measurements
    {
        /// <summary>
        ///     Field Number: 494-ZE
        ///     <para />
        ///     Description: Date clinical information was collected or measured.
        ///     <para />
        ///     Format: 9(08) CCYYMMDD
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if necessary when this field could result in
        ///     different coverage and/or drug utilization review outcome.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "MeasurementDate", Description = FieldDescriptions.MeasurementDate, DataType = "String", Format = "Date", IsRequired = false)]
        public DateTime? MeasurementDate { get; set; }

        /// <summary>
        ///     Field Number: 496-H2
        ///     <para />
        ///     Description: Code indicating the clinical domain of the observed value in Measurement Value.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Required if Measurement Unit (497-H3) and Measurement Value(499-H4) are used.
        ///     <para />
        ///     Required if necessary when this field could result in different coverage and/or drug utilization review outcome.
        ///     <para />
        ///     Required if necessary for patient’s weight and height when billing Medicare for a claim that includes a Certificate
        ///     of Medical Necessity (CMN).
        ///     <para />
        /// </summary>
        [ApiMember(Name = "MeasurementDimension", Description = FieldDescriptions.MeasurementDimension, DataType = "String", IsRequired = false)]
        public string MeasurementDimension { get; set; }

        /// <summary>
        ///     Field Number: 495-H1
        ///     <para />
        ///     Description: Time clinical information was collected or measured.
        ///     <para />
        ///     Format: 9(04) HHMM
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Time is known or has impact on measurement.
        ///     <para />
        ///     Required if necessary when this field could result in different coverage and/or drug utilization review outcome.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "MeasurementTime", Description = FieldDescriptions.MeasurementTime, DataType = "String", IsRequired = false)]
        public string MeasurementTime { get; set; }


        /// <summary>
        ///     Field Number: 497-H3
        ///     <para />
        ///     Description: Code indicating the metric or English units used with the clinical information.
        ///     <para />
        ///     Format: X(02)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Measurement Dimension(496-H2) and Measurement Value(499-H4) are used.
        ///     <para />
        ///     Required if necessary for patient’s weight and height when billing Medicare for a claim that includes a Certificate
        ///     of Medical Necessity (CMN).
        ///     <para />
        ///     Required if necessary when this field could result in different coverage and/or drug utilization review outcome.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "MeasurementUnit", Description = FieldDescriptions.MeasurementUnit, DataType = "String", IsRequired = false)]
        public string MeasurementUnit { get; set; }

        /// <summary>
        ///     Field Number: 499-H4
        ///     <para />
        ///     Description: Actual value of clinical information.
        ///     <para />
        ///     Format: X(15)
        ///     <para />
        ///     Designation: Qualified: repeating
        ///     <para />
        ///     Claim Billing/Encounter:
        ///     Required if Measurement Dimension(496-H2) and Measurement Unit(497-H3) are used.
        ///     <para />
        ///     Required if necessary for patient’s weight and height when billing Medicare for a claim that includes a Certificate
        ///     of Medical Necessity (CMN).
        ///     <para />
        ///     Required if necessary when this field could result in different coverage and/or drug utilization review outcome.
        ///     <para />
        /// </summary>
        [ApiMember(Name = "MeasurementValue", Description = FieldDescriptions.MeasurementValue, DataType = "String", IsRequired = false)]
        public string MeasurementValue { get; set; }
    }
}
