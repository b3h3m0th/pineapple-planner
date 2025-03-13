using CredentialManagement;
using PineapplePlanner.Domain.Shared;

namespace PineapplePlanner.Application.Services
{
    public class CredentialsService
    {
        private const string APPLICATION_CREDENTIALS_RESOURCE = "pineapple-planner-credentials";

        public ResultBase Save(string email, string password)
        {
            ResultBase result = ResultBase.Success();
            try
            {
                using (var cred = new Credential())
                {
                    cred.Target = APPLICATION_CREDENTIALS_RESOURCE;
                    cred.Username = email;
                    cred.Password = password;
                    cred.Type = CredentialType.Generic;
                    cred.PersistanceType = PersistanceType.LocalComputer;
                    cred.Save();
                }
            }
            catch (Exception ex)
            {
                result.AddErrorAndSetFailure(ex.Message);
            }

            return result;
        }

        public ResultBase<(string Email, string Password)?> Get()
        {
            ResultBase<(string, string)?> result = ResultBase<(string, string)?>.Success();

            try
            {
                using (var cred = new Credential { Target = APPLICATION_CREDENTIALS_RESOURCE })
                {
                    if (cred.Load())
                    {
                        result.Data = (cred.Username, cred.Password);
                    }
                    else
                    {
                        result.Data = null;
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddErrorAndSetFailure(ex.Message);
            }

            return result;
        }

        public ResultBase Remove()
        {
            ResultBase result = ResultBase.Success();

            try
            {
                using (Credential credentials = new() { Target = APPLICATION_CREDENTIALS_RESOURCE })
                {
                    if (credentials.Load())
                    {
                        credentials.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddErrorAndSetFailure(ex.Message);
            }

            return result;
        }
    }
}
