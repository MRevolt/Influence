using System.Reflection;

namespace Influence.Model
{
    public class AssemblyIdentifier
    {
        public static Assembly Get() =>
            typeof(AssemblyIdentifier).GetTypeInfo().Assembly;
    }
}
