namespace Dvchevskii.Blog.Contracts.Infrastructure;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SetupIdempotenceAttribute(bool isIdempotent) : Attribute
{
    public bool IsIdempotent { get; } = isIdempotent;
}