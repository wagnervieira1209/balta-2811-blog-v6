using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogV6.Extensions
{
    public static class ModelStateExtensions
    {
        //
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            var result = new List<string>();

            foreach (var item in modelState.Values)
                result.AddRange(item.Errors.Select(x => x.ErrorMessage));

            return result;
        }
    }
}