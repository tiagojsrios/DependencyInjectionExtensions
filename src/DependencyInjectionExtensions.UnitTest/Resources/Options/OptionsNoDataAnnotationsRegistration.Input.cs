using DependencyInjectionExtensions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Options
{
    [Options(ValidateDataAnnotations = false)]
    public class OptionsNoDataAnnotationsRegistration
    {
        [Required] public string ConnectionString { get; set; } = null!;
    }
}
