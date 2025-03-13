﻿using Microsoft.AspNetCore.Components;
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

    public partial class Home
    {
        [CascadingParameter(Name = "AuthenticatedLayout")]
        public AuthenticatedLayout? AuthenticatedLayout { get; set; }

        private ResultBase<List<Domain.Entities.Task>> _tasksResult = new();
        private List<Domain.Entities.Task> _filteredTasks = new();
        private TaskFilterOption _selectedFilterOption = TaskFilterOption.All;
        private int _activeTabIndex = 0;

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
                ApplyFilter();
            }
        }

        private void HandleFilterChange(TaskFilterOption newFilter)
        {
            _selectedFilterOption = newFilter;
            _activeTabIndex = (int)newFilter;
            ApplyFilter();
        }
        
        private void UpdateFilterFromTab()
        {
            _selectedFilterOption = (TaskFilterOption)_activeTabIndex;
            ApplyFilter();
        }

        private void ApplyFilter()
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
                    _filteredTasks = tasks.OrderByDescending(t => t.CreatedAt)
                        .ToList();
                    break;
                case TaskFilterOption.Active:
                    _filteredTasks = tasks.Where(t => t.CompletedAt == null)
                        .OrderByDescending(t => t.CreatedAt)
                        .ToList();
                    break;
                case TaskFilterOption.Completed:
                    _filteredTasks = tasks.Where(t => t.CompletedAt != null)
                        .OrderByDescending(t => t.CreatedAt)
                        .ToList();
                    break;
                default:
                    _filteredTasks = tasks.OrderByDescending(t => t.CreatedAt)
                        .ToList();
                    break;
            }
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
