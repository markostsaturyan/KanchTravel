using Authentication.DataManagement.BusinessLogicLayer;
using Authentication.DataManagement.BusinessLogicLayer.BusiessLayerDataModel;
using Authentication.Validators;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class ProfileService : IProfileService
    {
        private IUserRepository userRepository;

        public ProfileService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                if (!string.IsNullOrEmpty(context.Subject.Identity.Name))
                {
                    //get user from db (in my case this is by email)
                    var user = userRepository.FindUserAsync(context.Subject.Identity.Name);

                    if (user != null)
                    {
                        //set issued claims to return
                        context.IssuedClaims = GetUserClaims(user);
                    }
                }
                else
                {
                    //get subject from context (this was set ResourceOwnerPasswordValidator.ValidateAsync),
                    //where and subject was set to my user id.
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                    if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
                    {
                        //get user from db (find user by user id)
                        var user = userRepository.FindUserAsync(long.Parse(userId.Value));

                        // issue the claims for the user
                        if (user != null)
                        {
                            context.IssuedClaims = ResourceOwnerPasswordValidator.GetUserClaims(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //log your error
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                //get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
                var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "user_id");

                if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
                {
                    var user = userRepository.FindUserAsync(long.Parse(userId.Value));

                    if (user != null)
                    {
                        if (user.IsActive)
                        {
                            context.IsActive = user.IsActive;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //handle error logging
            }
        }

        private List<Claim> GetUserClaims(User user)
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
