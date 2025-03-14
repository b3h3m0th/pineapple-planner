using Microsoft.AspNetCore.Components;
using PineapplePlanner.Domain.Shared;
using PineapplePlanner.UI.Layouts;
using PineapplePlanner.UI.Providers;
using System.Collections.Generic;
using System.Linq;

namespace PineapplePlanner.UI.Pages
{
    public enum TaskFilterOption
    {
        All,
        Active,
        Completed
    }

    public enum TaskSortOption
    {
        All,
        Newest,
        Oldest,
        HighPriority,
        MediumPriority,
        LowPriority,
        DueDate
    }

    public partial class Home
    {
        [CascadingParameter(Name = "AuthenticatedLayout")]
        public AuthenticatedLayout? AuthenticatedLayout { get; set; }

        private ResultBase<List<Domain.Entities.Task>> _tasksResult = new();
        private List<Domain.Entities.Task> _filteredTasks = new();
        private TaskFilterOption _selectedFilterOption = TaskFilterOption.All;
        private TaskSortOption _selectedSortOption = TaskSortOption.All;

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

        private void HandleFilterChange(TaskFilterOption newFilter)
        {
            _selectedFilterOption = newFilter;
            ApplyFilterAndSort();
        }

        private void HandleSortChange(TaskSortOption newSort)
        {
            _selectedSortOption = newSort;
            ApplyFilterAndSort();
        }

        private void ApplyFilterAndSort()
        {
            if (_tasksResult.Data == null)
            {
                _filteredTasks = new List<Domain.Entities.Task>();
                return;
            }

            var tasks = _tasksResult.Data.OfType<Domain.Entities.Task>();

            switch (_selectedFilterOption)
            {
                case TaskFilterOption.All:
                    tasks = tasks;
                    break;
                case TaskFilterOption.Active:
                    tasks = tasks.Where(t => t.CompletedAt == null);
                    break;
                case TaskFilterOption.Completed:
                    tasks = tasks.Where(t => t.CompletedAt != null);
                    break;
                default:
                    tasks = tasks;
                    break;
            }

            switch (_selectedSortOption)
            {
                case TaskSortOption.All:
                    tasks = tasks.OrderByDescending(t => t.CreatedAt);
                    break;
                case TaskSortOption.Newest:
                    tasks = tasks.OrderByDescending(t => t.CreatedAt);
                    break;
                case TaskSortOption.Oldest:
                    tasks = tasks.OrderBy(t => t.CreatedAt);
                    break;
                case TaskSortOption.HighPriority:
                    tasks = tasks.Where(t => t.Priority == Domain.Enums.Priority.High)
                                .OrderByDescending(t => t.CreatedAt);
                    break;
                case TaskSortOption.MediumPriority:
                    tasks = tasks.Where(t => t.Priority == Domain.Enums.Priority.Medium)
                                .OrderByDescending(t => t.CreatedAt);
                    break;
                case TaskSortOption.LowPriority:
                    tasks = tasks.Where(t => t.Priority == Domain.Enums.Priority.Low)
                                .OrderByDescending(t => t.CreatedAt);
                    break;
                case TaskSortOption.DueDate:
                    tasks = tasks.Where(t => t.DateDue != null)
                                .OrderBy(t => t.DateDue)
                                .ThenByDescending(t => t.CreatedAt);
                    break;
                default:
                    tasks = tasks.OrderByDescending(t => t.CreatedAt);
                    break;
            }

            _filteredTasks = tasks.ToList();
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
