using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using IdentityModel;
using Authentication.DataManagement.BusinessLogicLayer;
using Authentication.DataManagement.BusinessLogicLayer.BusiessLayerDataModel;
using Authentication.Security;

namespace Authentication.Validators
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IUserRepository userRepository;

        public ResourceOwnerPasswordValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                //get your user model from db (by username - in my case its email)
                var user = userRepository.FindUserAsync(context.UserName);

                var password = (context.Password + user.HashGuide).HashSHA1();

                if (user != null)
                {
                    //check if password match - remember to hash password if stored as hash in db
                    if (user.Password == password)
                    {
                        //set the result
                        context.Result = new GrantValidationResult(
                            subject: user.Id.ToString(),
                            authenticationMethod: "custom",
                            claims: GetUserClaims(user));

                        return;
                    }

                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect password");
                    return;
                }
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist.");
                return;
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
            }
        }

        public static List<Claim> GetUserClaims(User user)
        {
            return new List<Claim>
            {
                new Claim("user_id", user.Id.ToString() ?? ""),
                
                //roles
                new Claim(JwtClaimTypes.Role, user.Role)
            };
        }
    }
}
