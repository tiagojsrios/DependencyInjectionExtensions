using DependencyInjectionExtensions.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Options
{
    [Options]
    public class DefaultOptionsRegistration
    {
        [Required] public string ConnectionString { get; set; } = null!;
    }
}
