namespace ContosoUniversityCore.UnitTests
{
    using System.Linq;
    using System.Reflection;
    using Fixie.Conventions;

    public static class FixieExtensions
    {
        public static ClassExpression IsBddStyleClassNameOrEndsWithTests(this ClassExpression expr)
        {
            return expr.Where(type => type.Name.EndsWith("Tests") || type.Name.StartsWith("When"));
        }

        public static ClassExpression ConstructorHasArguments(this ClassExpression filter)
        {
            return filter.Where(t => t.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .Any(x => x.GetParameters().Any()));
        }

        public static ClassExpression ConstructorDoesntHaveArguments(this ClassExpression filter)
        {
            return filter.Where(t => t.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .All(x => x.GetParameters().Length == 0));
        }
    }
}