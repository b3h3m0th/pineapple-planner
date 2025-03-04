using Google.Cloud.Firestore;
using PineapplePlanner.Application.Interfaces;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.Infrastructure;

namespace PineapplePlanner.Application.Repositories;

public class UserRepository : BaseRepository<Domain.Entities.User>, IUserRepository
{
    public UserRepository(FirestoreService firestoreService) : base(firestoreService)
    {
    }

    public async Task<ResultBase<Domain.Entities.User>> CreateAsync(Domain.Entities.User user)
    {
        ResultBase<Domain.Entities.User> result = ResultBase<Domain.Entities.User>.Success();

        try
        {
            user.Id = Guid.NewGuid().ToString();
            DocumentReference docRef = _firestoreService.FirestoreDb.Collection(_collectionName).Document(user.Id);
            await docRef.SetAsync(user);

            result.Data = user;
        }
        catch (Exception ex)
        {
            result.AddErrorAndSetFailure(ex.Message);
        }

        return result;
    }

    public async Task<ResultBase<Domain.Entities.User?>> GetByUIdAsync(string userUid)
    {
        try
        {
            QuerySnapshot querySnapshot = await _firestoreService.FirestoreDb
                .Collection(_collectionName)
                .WhereEqualTo("UserUid", userUid)
                .GetSnapshotAsync();
            DocumentSnapshot? documentSnapshot = querySnapshot.FirstOrDefault();

            Domain.Entities.User? document = documentSnapshot?.Exists == true ? documentSnapshot.ConvertTo<Domain.Entities.User>() : default;

            return ResultBase<Domain.Entities.User?>.Success(document);
        }
        catch (Exception)
        {
            return ResultBase<Domain.Entities.User?>.Failure();
        }
    }
}

