namespace Dvchevskii.Blog.Contracts.Infrastructure;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SetupOrderAttribute(int order) : Attribute
{
    public int Order { get; } = order;
}
