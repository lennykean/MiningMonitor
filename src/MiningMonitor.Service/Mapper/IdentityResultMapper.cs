using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using MiningMonitor.Common.Mapper;

namespace MiningMonitor.Service.Mapper
{
    public class IdentityResultMapper : IMapper<IdentityResult, ModelStateDictionary>
    {
        public ModelStateDictionary Map(IdentityResult identityResult)
        {
            var state = new ModelStateDictionary();
            foreach (var error in identityResult.Errors)
            {
                if (error.Code.IndexOf("password", StringComparison.OrdinalIgnoreCase) > -1)
                    state.AddModelError("password", error.Description);
                else if (error.Code.IndexOf("username", StringComparison.OrdinalIgnoreCase) > -1)
                    state.AddModelError("username", error.Description);
                else
                    state.AddModelError("", error.Description);
            }

            return state;
        }
    }
}