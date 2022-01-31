using HomeStrategiesApi.Helper;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeStrategiesApi.Auth
{
    public class AuthenticationClaimsHelper
    {
        public ClaimsIdentity Identity { get; set; }

        public AuthenticationClaimsHelper(ClaimsIdentity Identity)
        {
            this.Identity = Identity;
        }

        public int GetIdClaimFromUser()
        {
            var userIdClaim = Identity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var parsed = int.TryParse(userIdClaim, out var userId);

            if (parsed)
            {
                return userId;
            }
            else
            {
                return 0;
            }
        }

        public int GetHouseholdClaimFromUser()
        {

            var householdIdClaim = Identity.FindFirst(ClaimTypes.GroupSid).Value;

            var parsed = int.TryParse(householdIdClaim, out var householdId);

            if (parsed)
            {
                return householdId;
            }
            else
            {
                return 0;
            }
        }

        public bool IsAuthenticatedForHousehold(int householdId)
        {
            var identityHouseholdId = GetHouseholdClaimFromUser();
            return householdId.Equals(identityHouseholdId);
        }

        public async Task<string> GetUserNameFromClaims(HomeStrategiesContext _context)
        {
            var userId = GetIdClaimFromUser();
            var user = await _context.User.FindAsync(userId);
            return user.Firstname + " " + user.Surname;
        }
    }
}
