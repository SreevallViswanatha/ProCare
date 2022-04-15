using ProCare.API.PBM.Messages.Request;
using ServiceStack.FluentValidation;

namespace ProCare.API.PBM.RequestValidator
{
    public class HospiceImportInsertRecordRequestValidator : AbstractValidator<HospiceImportInsertRecordRequest>
    {

        public HospiceImportInsertRecordRequestValidator()
        {
            //required fields
            RuleFor(x => x.PatientRecord).NotEmpty();
        }
    }
}
