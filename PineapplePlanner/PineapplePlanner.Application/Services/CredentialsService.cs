using CredentialManagement;
using PineapplePlanner.Domain.Shared;

namespace PineapplePlanner.Application.Services
{
    public class CredentialsService
    {
        private readonly string APPLICATION_CREDENTIALS_RESOURCE = "pineapple-planner-credentials";

        public ResultBase Save(string email, string password)
        {
            ResultBase result = ResultBase.Success();
            try
            {
                using Credential credential = new();

                credential.Target = APPLICATION_CREDENTIALS_RESOURCE;
                credential.Username = email;
                credential.Password = password;
                credential.Type = CredentialType.Generic;
                credential.PersistanceType = PersistanceType.LocalComputer;
                credential.Save();
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
                using Credential credential = new() { Target = APPLICATION_CREDENTIALS_RESOURCE };

                if (credential.Load())
                {
                    result.Data = (credential.Username, credential.Password);
                }
                else
                {
                    result.Data = null;
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
                using Credential credentials = new() { Target = APPLICATION_CREDENTIALS_RESOURCE };

                if (credentials.Load())
                {
                    credentials.Delete();
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
