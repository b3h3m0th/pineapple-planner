using Microsoft.AspNetCore.Components.Web;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Providers;

namespace PineapplePlanner.UI.Pages
{
    public partial class AIAssistant
    {
        private string _prompt = string.Empty;
        private readonly List<string> _placeholderPrompts =
        [
            "Prepare meeting notes for tomorrow at 8 pm. It’s crucial for clarity.",
            "Call the marketing team on Monday at 10 am.",
            "Submit the project report by Friday at 5 pm. Don’t miss the deadline.",
            "Brainstorm ideas with the team next Wednesday at 2 pm.",
            "Review the budget proposal by Thursday at 3 pm for accuracy.",
            "Follow up with the client on Friday at 3 pm.",
            "Finalize the presentation slides by 6 pm tonight.",
            "Interview the new candidate on Tuesday at 4 pm.",
            "Plan lunch with the sales team next Thursday at 1 pm.",
            "Compile potential investors by Monday at noon."
        ];
        private readonly Random random = new();
        private ResultBase<Domain.Entities.Entry> _promptResult = ResultBase<Domain.Entities.Entry>.Success();
        private bool _isGeneratingTask;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        private async Task HandleSubmitPrompt()
        {
            _promptResult = ResultBase<Domain.Entities.Entry>.Success();
            if (string.IsNullOrEmpty(_prompt?.Trim())) return;

            _isGeneratingTask = true;

            ResultBase<Domain.Entities.Entry> taskResult = await _aiService.GenerateTaskFromPrompt(_prompt);
            if (!taskResult.IsSuccess || taskResult.Data == null)
            {
                _promptResult.AddErrorAndSetFailure("Something went wrong. Please try again with a more descriptive prompt.");
            }

            string? firebaseUid = ((FirebaseAuthStateProvider)_authStateProvider).FirebaseUid;
            if (string.IsNullOrEmpty(firebaseUid))
            {
                _promptResult.AddErrorAndSetFailure("Authentication required");
            }

            if (_promptResult.IsSuccess && taskResult.Data != null && !string.IsNullOrEmpty(firebaseUid))
            {
                Domain.Entities.Entry createdTask = await CreateTask(taskResult.Data, firebaseUid);
                _promptResult.Data = createdTask;
            }

            _isGeneratingTask = false;

            StateHasChanged();
            await Task.Delay(30000);
            _promptResult = ResultBase<Domain.Entities.Entry>.Success();
            StateHasChanged();
        }

        private async Task HandlePromptKeyDown(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                await HandleSubmitPrompt();
            }

            StateHasChanged();
        }

        private async Task<Domain.Entities.Entry> CreateTask(Domain.Entities.Entry entry, string userUid)
        {
            entry.Id = Guid.NewGuid().ToString();
            entry.UserUid = userUid;
            await _taskRepository.AddAsync(entry);

            return entry;
        }

        private string GetRandomPromptPlaceholderString()
        {
            return _placeholderPrompts[random.Next(_placeholderPrompts.Count)];
        }
    }
}
