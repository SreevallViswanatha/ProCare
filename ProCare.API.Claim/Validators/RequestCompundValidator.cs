using FluentValidation;
using ProCare.NCPDP.Telecom.Request;


namespace ProCare.API.Claims.Validators
{
    public class RequestCompoundValidator : AbstractValidator<RequestCompound>
    {
        public RequestCompoundValidator()
        {
            RuleSet("Aetna", () =>
            {
                RuleFor(x => x.DosageFormDescriptionCode)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dosage Form Discription Code");
            
                RuleFor(x => x.DispensingUnitFormIndicator)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dispensing Unit Form Indicator");
            
                RuleFor(x => x.IngredientCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Ingredient Count");
            });

            RuleSet("CareMarkCompound", () =>
            {
                RuleFor(x => x.DispensingUnitFormIndicator)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dispensing Unit Form Indicator");
            
                RuleFor(x => x.DosageFormDescriptionCode)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dosage Form Discription Code");
            
                RuleFor(x => x.IngredientCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Ingredient Count");
            });

            RuleSet("Envision", () =>
            {
                RuleFor(x => x.DispensingUnitFormIndicator)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dispensing Unit Form Indicator");
            
                RuleFor(x => x.DosageFormDescriptionCode)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dosage Form Discription Code");
           
                RuleFor(x => x.IngredientCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Ingredient Count");
            });

            RuleSet("MedImpact", () =>
            {
                RuleFor(x => x.DispensingUnitFormIndicator)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dispensing Unit Form Indicator");
            
                RuleFor(x => x.DosageFormDescriptionCode)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dosage Form Discription Code");
            
                RuleFor(x => x.IngredientCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Ingredient Count");
            });

            RuleSet("MRX", () =>
            {
                RuleFor(x => x.DispensingUnitFormIndicator)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dispensing Unit Form Indicator");
            
                RuleFor(x => x.DosageFormDescriptionCode)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dosage Form Discription Code");
            
                RuleFor(x => x.IngredientCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Ingredient Count");
            });

            RuleSet("ExpressScripts", () =>
            {
                //RuleFor(x => x.DispensingUnitFormIndicator)
                //    .NotEmpty()
                //    .WithSeverity(Severity.Error)
                //    .WithState(x => "Missing Dispensing Unit Form Indicator");
            
                //RuleFor(x => x.DosageFormDescriptionCode)
                //    .NotEmpty()
                //    .WithSeverity(Severity.Error)
                //    .WithState(x => "Missing Dosage Form Discription Code");
            
                //RuleFor(x => x.IngredientCount)
                //    .NotEmpty()
                //    .WithSeverity(Severity.Error)
                //    .WithState(x => "Missing Ingredient Count");
            });

            RuleSet("Prime", () =>
            {
                RuleFor(x => x.DispensingUnitFormIndicator)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dispensing Unit Form Indicator");
            
                RuleFor(x => x.DosageFormDescriptionCode)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Dosage Form Discription Code");
            
                RuleFor(x => x.IngredientCount)
                    .NotEmpty()
                    .WithSeverity(Severity.Error)
                    .WithState(x => "Missing Ingredient Count");
            });

            
        }
    }
}
