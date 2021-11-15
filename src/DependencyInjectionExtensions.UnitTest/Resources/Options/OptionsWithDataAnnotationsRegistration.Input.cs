using DependencyInjectionExtensions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Options
{
    [Options(ValidateDataAnnotations = true)]
    public class OptionsWithDataAnnotationsRegistration
    {
        public string ConnectionString { get; set; } = null!;
    }
}
