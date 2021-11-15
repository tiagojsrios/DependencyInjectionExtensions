using DependencyInjectionExtensions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Options
{
    [Options(ConfigurationSectionName = "CustomSectionName")]
    public class OptionsCustomSectionNameRegistration
    {
        [Required] public string ConnectionString { get; set; } = null!;
    }
}
