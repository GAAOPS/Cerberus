namespace Cerberus.Core.Diagnostics
{
    using System;
    using System.ComponentModel;

    public static class Assert
    {
        [AssertionMethod]
        public static void ArgumentNotNullOrEmpty(
            [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] [ValidatedNotNull]
            string argument, [InvokerParameterName] string argumentName)
        {
            if (!string.IsNullOrEmpty(argument))
            {
                return;
            }

            if (argument == null)
            {
                if (argumentName != null)
                {
                    throw new ArgumentNullException(argumentName, "Null is not allowed.");
                }

                throw new ArgumentNullException(nameof(argument),"Null is not allowed");
            }

            if (argumentName != null)
            {
                throw new ArgumentException("Empty strings are not allowed.", argumentName);
            }

            throw new ArgumentException("Empty strings are not allowed.");
        }

        [AssertionMethod]
        public static void ArgumentNotNull([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] [ValidatedNotNull]
            object argument, [InvokerParameterName] string argumentName)
        {
            if (argument != null)
            {
                return;
            }

            if (argumentName != null)
            {
                throw new ArgumentNullException(argumentName);
            }

            throw new ArgumentNullException(nameof(argument), "Null is not allowed");

        }

        [AssertionMethod]
        public static void ArgumentCondition([AssertionCondition(AssertionConditionType.IS_TRUE)]
            bool condition, [InvokerParameterName] string argumentName, [Localizable(false)] string message)
        {
            if (condition)
            {
                return;
            }

            message = GetMessage(message, "An argument condition was false.");
            if (argumentName != null)
            {
                throw new ArgumentException(message, argumentName);
            }

            throw new ArgumentException(message);
        }

        private static string GetMessage(params string[] values)
        {
            ArgumentNotNull(values, nameof(values));
            foreach (var str in values)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
            }

            return string.Empty;
        }
    }
}