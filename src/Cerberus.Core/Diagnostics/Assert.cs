namespace Sitecore.Helix.Validator.Common.Diagnostics
{
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.Parameter)]
    internal class ValidatedNotNullAttribute : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class InvokerParameterNameAttribute : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AssertionMethodAttribute : Attribute
    {
    }
    public class Assert
    {
        [AssertionMethod]
        public static void ArgumentNotNullOrEmpty([AssertionCondition(AssertionConditionType.IS_NOT_NULL), ValidatedNotNull] string argument, [InvokerParameterName] string argumentName)
        {
            if (!string.IsNullOrEmpty(argument))
                return;
            if (argument == null)
            {
                if (argumentName != null)
                    throw new ArgumentNullException(argumentName, "Null ids are not allowed.");
                throw new ArgumentNullException();
            }
            if (argumentName != null)
                throw new ArgumentException("Empty strings are not allowed.", argumentName);
            throw new ArgumentException("Empty strings are not allowed.");
        }

        [AssertionMethod]
        public static void ArgumentNotNull([AssertionCondition(AssertionConditionType.IS_NOT_NULL), ValidatedNotNull] object argument, [InvokerParameterName] string argumentName)
        {
            if (argument != null)
                return;
            if (argumentName != null)
                throw new ArgumentNullException(argumentName);
            throw new ArgumentNullException();
        }
        [AssertionMethod]
        public static void ArgumentCondition([AssertionCondition(AssertionConditionType.IS_TRUE)] bool condition, [InvokerParameterName] string argumentName, [Localizable(false)] string message)
        {
            if (condition)
                return;
            message = GetMessage(message, "An argument condition was false.");
            if (argumentName != null)
                throw new ArgumentException(message, argumentName);
            throw new ArgumentException(message);
        }

        private static string GetMessage(params string[] values)
        {
            Assert.ArgumentNotNull((object)values, nameof(values));
            foreach (string str in values)
            {
                if (!string.IsNullOrEmpty(str))
                    return str;
            }
            return string.Empty;
        }
    }
}