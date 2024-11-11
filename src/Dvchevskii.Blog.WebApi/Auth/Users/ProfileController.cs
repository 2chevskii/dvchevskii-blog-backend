using Dvchevskii.Blog.Contracts.Authentication;
using Dvchevskii.Blog.Contracts.Authentication.Users;
using Microsoft.AspNetCore.Mvc;

namespace Dvchevskii.Blog.WebApi.Auth.Users;

[ApiController]
[Route("[controller]")]
public class ProfileController(
    IUserProfileService userProfileService,
    IAuthenticationContext authenticationContext
) : ControllerBase
{
    [HttpPatch("avatar")]
    public async Task<ActionResult<UserProfileDto>> UpdateAvatar(UserAvatarIdModel model)
    {
        var profile = await userProfileService.UpdateUserProfileAvatar(authenticationContext.UserId!.Value, model.Id);
        if (profile.AvatarId.HasValue)
        {
            profile.AvatarUrl = Url.Action(
                controller: "Images",
                action: "Download",
                values: new { id = profile.AvatarId.Value, }
            );
        }

        return Ok(profile);
    }
}
