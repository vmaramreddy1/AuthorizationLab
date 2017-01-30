using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace AuthorizationLab
{
	public class HasBadgeHandler : AuthorizationHandler<OfficeEntryRequirement>
	{
		protected override Task HandleRequirementAsync
		(AuthorizationHandlerContext context,OfficeEntryRequirement requirement)
		{
			 if (!context.User.HasClaim(c => c.Type == "BadgeNumber" && 
                                            c.Issuer == "https://contoso.com"))
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);

             return Task.CompletedTask;
		}
	}
}
