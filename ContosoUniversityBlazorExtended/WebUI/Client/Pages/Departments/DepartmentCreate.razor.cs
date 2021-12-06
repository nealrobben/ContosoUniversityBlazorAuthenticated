﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Client.Services;
using WebUI.Client.Shared;
using WebUI.Shared.Departments.Commands.CreateDepartment;
using WebUI.Shared.Instructors.Queries.GetInstructorsLookup;

namespace WebUI.Client.Pages.Departments
{
    public partial class DepartmentCreate
    {
        [Inject]
        IDepartmentService DepartmentService { get; set; }

        [Inject]
        IInstructorService InstructorService { get; set; }

        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        private CustomValidation _customValidation;

        public CreateDepartmentCommand CreateDepartmentCommand { get; set; } = new CreateDepartmentCommand() { StartDate = DateTime.UtcNow.Date };
        public InstructorsLookupVM InstructorsLookup { get; set; }

        public bool ErrorVisible { get; set; }

        protected override async Task OnInitializedAsync()
        {
            InstructorsLookup = await InstructorService.GetLookupAsync();
            CreateDepartmentCommand.InstructorID = InstructorsLookup.Instructors.First().ID;
        }

        public async Task FormSubmitted(EditContext editContext)
        {
            _customValidation.ClearErrors();
            ErrorVisible = false;
            bool formIsValid = editContext.Validate();

            if (formIsValid)
            {
                var result = await DepartmentService.CreateAsync(CreateDepartmentCommand);

                if (result.IsSuccessStatusCode)
                {
                    CreateDepartmentCommand = new CreateDepartmentCommand();
                    MudDialog.Close(DialogResult.Ok(true));
                }
                else
                {
                    ErrorVisible = true;
                }
            }
        }

        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
