using Microsoft.AspNetCore.Components;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Layouts;
using PineapplePlanner.UI.Providers;
using PineapplePlanner.UI.Services;

namespace PineapplePlanner.UI.Pages
{
    public enum TasksListFilterOption
    {
        Completed,
        Uncompleted
    }

    public enum TasksListSortOption
    {
        CreationDateAscending,
        CreationDateDescending,
        CompletionDateAscending,
        CompletionDateDescending,
        DueDateAscending,
        DueDateDescending,
        PriorityAscending,
        PriorityDescending
    }



    public partial class Home
    {
        [CascadingParameter(Name = "AuthenticatedLayout")]
        public AuthenticatedLayout? AuthenticatedLayout { get; set; }

        private ResultBase<List<Domain.Entities.Task>> _tasksResult = new();
        private List<Domain.Entities.Task> _filteredTasks = [];
        private string _searchQuery = string.Empty;
        private List<TasksListFilterOption> _selectedFilterOptions = [TasksListFilterOption.Uncompleted];
        private TasksListSortOption _selectedSortOption = TasksListSortOption.DueDateAscending;


        protected override async Task OnParametersSetAsync()
        {
            await LoadTasks();

            await base.OnParametersSetAsync();
        }




        private async Task LoadTasks()
        {
            string? firebaseUid = ((FirebaseAuthStateProvider)_authStateProvider).FirebaseUid;

            if (!string.IsNullOrEmpty(firebaseUid))
            {
                _tasksResult = await _taskRepository.GetAllAsync<Domain.Entities.Task>(firebaseUid);
                ApplyFilterAndSort();
            }
        }

        private void HandleSelectedFilterValuesChanged(IEnumerable<TasksListFilterOption>? newFilters)
        {
            _selectedFilterOptions = newFilters?.ToList() ?? [];
            ApplyFilterAndSort();
        }

        private void HandleSelectedSortValueChanged(TasksListSortOption sortOption)
        {
            _selectedSortOption = sortOption;
            ApplyFilterAndSort();
        }

        private void HandleSearchQueryValueChanged(string value)
        {
            _searchQuery = value;
            ApplyFilterAndSort();
        }

        private void ApplyFilterAndSort()
        {
            if (_tasksResult.Data == null)
            {
                _filteredTasks = [];
                return;
            }

            List<Domain.Entities.Task> tasks = _tasksResult.Data;

            _filteredTasks = [.. _tasksResult.Data
                .Where(t => (_selectedFilterOptions.Contains(TasksListFilterOption.Completed) && t.IsCompleted)
                         || (_selectedFilterOptions.Contains(TasksListFilterOption.Uncompleted) && !t.IsCompleted))
                .Where(t =>
                {
                    if (string.IsNullOrWhiteSpace(_searchQuery)) return true;

                    var searchWords = _searchQuery.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    return searchWords.All(word => t.Name.Contains(word, StringComparison.OrdinalIgnoreCase));
                })];

            _filteredTasks = _selectedSortOption switch
            {
                TasksListSortOption.CreationDateAscending => [.. _filteredTasks.OrderBy(t => t.CreatedAt)],
                TasksListSortOption.CreationDateDescending => [.. _filteredTasks.OrderByDescending(t => t.CreatedAt)],
                TasksListSortOption.CompletionDateAscending => [.. _filteredTasks.OrderBy(t => t.CompletedAt)],
                TasksListSortOption.CompletionDateDescending => [.. _filteredTasks.OrderByDescending(t => t.CompletedAt)],
                TasksListSortOption.DueDateAscending => [.. _filteredTasks.OrderBy(t => t.DateDue)],
                TasksListSortOption.DueDateDescending => [.. _filteredTasks.OrderByDescending(t => t.DateDue)],
                TasksListSortOption.PriorityAscending => [.. _filteredTasks.OrderBy(t => t.Priority)],
                TasksListSortOption.PriorityDescending => [.. _filteredTasks.OrderByDescending(t => t.Priority)],
                _ => _filteredTasks
            };
        }

        private void HandleCreateTask()
        {
            AuthenticatedLayout?.OpenTaskDetail();
        }

        private void HandleEditTask(Domain.Entities.Entry entry)
        {
            AuthenticatedLayout?.OpenTaskDetail(entry);
        }

        private async Task HandleTaskCompleteChange(Domain.Entities.Entry entry)
        {
            await _taskRepository.UpdateAsync(entry);
            await LoadTasks();
            AuthenticatedLayout?.OpenTaskDetail(entry);
        }
    }
}
