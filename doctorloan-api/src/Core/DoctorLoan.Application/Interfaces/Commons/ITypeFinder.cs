using System.Reflection;

namespace DoctorLoan.Application.Interfaces.Commons;
public interface ITypeFinder
{
    IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

    IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

    IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

    IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

    IList<Assembly> GetAssemblies();
}
