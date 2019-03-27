namespace Cerberus.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using Configy.Parsing;

    public class HelixConventionVariablesReplacer : ContainerDefinitionVariablesReplacer
    {
        public override void ReplaceVariables(ContainerDefinition definition)
        {
            if (definition.Name == null)
            {
                return;
                throw new ArgumentException(
                    "Configuration without a name was used. Add a name attribute to all configurations.",
                    nameof(definition));
            }

            var variables = GetVariables(definition.Name);

            ApplyVariables(definition.Definition, variables);
        }

        public virtual Dictionary<string, string> GetVariables(string name)
        {
            var pieces = name.Split('.');

            if (pieces.Length < 2)
            {
                return new Dictionary<string, string>();
            }

            var vars = new Dictionary<string, string>
            {
                {"layer", pieces[0]},
                {"module", pieces[1]}
            };

            if (pieces.Length >= 3)
            {
                vars.Add("moduleConfigName", pieces[2]);
            }
            else
            {
                // fallback if no third level name is used but variable is defined
                vars.Add("moduleConfigName", "Dev");
            }

            return vars;
        }
    }
}