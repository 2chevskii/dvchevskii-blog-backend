using System.Reflection;
using Dvchevskii.Blog.Contracts.Authentication;

namespace Dvchevskii.Blog.Contracts.Infrastructure;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SetupUserAttribute(string technicalUserName) : Attribute
{
    public string TechnicalUserName { get; } = technicalUserName;

    public UserDto User => (UserDto)typeof(TechnicalUsers)
        .GetField(TechnicalUserName, BindingFlags.Public | BindingFlags.Static)!
        .GetValue(null)!;
}
