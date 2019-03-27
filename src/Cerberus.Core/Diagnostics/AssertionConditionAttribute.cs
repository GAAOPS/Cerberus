namespace Sitecore.Helix.Validator.Common.Diagnostics
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class AssertionConditionAttribute : Attribute
    {
        private readonly AssertionConditionType _conditionType;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.AssertionConditionAttribute" /> class.
        /// </summary>
        /// <param name="conditionType">Type of the condition.</param>
        public AssertionConditionAttribute(AssertionConditionType conditionType)
        {
            this._conditionType = conditionType;
        }

        /// <summary>Gets the type of the condition.</summary>
        /// <value>The type of the condition.</value>
        public AssertionConditionType ConditionType => this._conditionType;
    }
}