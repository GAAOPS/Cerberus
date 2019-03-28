namespace Cerberus.Core.Diagnostics
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter)]
    public class AssertionConditionAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Sitecore.AssertionConditionAttribute" /> class.
        /// </summary>
        /// <param name="conditionType">Type of the condition.</param>
        public AssertionConditionAttribute(AssertionConditionType conditionType)
        {
            ConditionType = conditionType;
        }

        /// <summary>Gets the type of the condition.</summary>
        /// <value>The type of the condition.</value>
        public AssertionConditionType ConditionType { get; }
    }
}