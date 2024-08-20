using CS_Advanced_Atsiskaitymas_Restoranas_v2.Models;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Services.Interfaces
{
    internal interface IUserService
    {
        User? ValidateUser(string userLogInName, string userLogInPassCode);
    }
}