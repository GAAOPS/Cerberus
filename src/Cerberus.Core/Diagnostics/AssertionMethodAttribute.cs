namespace Cerberus.Core.Diagnostics
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AssertionMethodAttribute : Attribute
    {
    }
}