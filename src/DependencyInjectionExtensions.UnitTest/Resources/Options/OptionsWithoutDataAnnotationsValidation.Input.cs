using DependencyInjectionExtensions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Options
{
    [Options(ValidateDataAnnotations = false)]
    public class OptionsWithoutDataAnnotationsValidation
    {
        public string ConnectionString { get; set; } = null!;
    }
}
