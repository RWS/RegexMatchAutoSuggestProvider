using RegexMASProviderLib.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexMASProviderLib.Extensions
{
    public static class VariableListExtension
    {
        public static IEnumerable<Variable> ToDistinct(this IEnumerable<Variable> list)
        {
            var variables = new List<Variable>();
            foreach (var variable in list.Where(e => e.IsEnabled && !e.HasErrors))
            {
                if (variables.Count == 0 || variables.All(e => e.Name != variable.Name))
                {
                    variables.Add(variable);
                }
            }
            return variables;
        }
    }
}
