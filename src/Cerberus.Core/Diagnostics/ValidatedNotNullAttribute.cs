namespace Cerberus.Core.Diagnostics
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter)]
    internal class ValidatedNotNullAttribute : Attribute
    {
    }
}