﻿using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;
using WebUI.Client.Extensions;
using WebUI.Client.Pages.Courses;
using WebUI.Client.Services;
using WebUI.Shared.Courses.Queries.GetCoursesOverview;

namespace WebUI.Client.ViewModels.Courses
{
    public class CoursesViewModel : CoursesViewModelBase
    {
        private IDialogService _dialogService { get; set; }
        private ISnackbar _snackbar { get; set; }
        public MudTable<CourseVM> Table { get; set; }

        public CoursesOverviewVM CoursesOverview { get; set; } = new CoursesOverviewVM();

        public CoursesViewModel(ICourseService courseService,
            IDialogService dialogService, ISnackbar snackbar,
            IStringLocalizer<CourseResources> courseLocalizer,
            IStringLocalizer<GeneralResources> generalLocalizer) 
            : base(courseService,courseLocalizer,generalLocalizer)
        {
            _dialogService = dialogService;
            _snackbar = snackbar;
            _snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
            _snackbar.Configuration.ClearAfterNavigation = true;
        }

        private async Task GetCourses()
        {
            await Table.ReloadServerData();
        }

        public async Task DeleteCourse(int courseId, string title)
        {
            bool? dialogResult = await _dialogService.ShowMessageBox(_generalLocalizer["Confirm"], _courseLocalizer["DeleteConfirmation", title],
                yesText: _generalLocalizer["Delete"], cancelText: _generalLocalizer["Cancel"]);

            if (dialogResult == true)
            {
                var result = await _courseService.DeleteAsync(courseId.ToString());

                if (result.IsSuccessStatusCode)
                {
                    _snackbar.Add(_courseLocalizer["DeleteFeedback", title], Severity.Success);
                    await GetCourses();
                }
            }
        }

        public void OpenCourseDetails(int courseId)
        {
            var parameters = new DialogParameters();
            parameters.Add("CourseId", courseId);

            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall };

            _dialogService.Show<CourseDetails>(_courseLocalizer["CourseDetails"], parameters, options);
        }

        public async Task OpenCourseEdit(int courseId)
        {
            var parameters = new DialogParameters();
            parameters.Add("CourseId", courseId.ToString());

            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall };

            var dialog = _dialogService.Show<CourseEdit>(_courseLocalizer["CourseEdit"], parameters, options);

            var result = await dialog.Result;

            if (result.Data != null && (bool)result.Data)
            {
                await GetCourses();
            }
        }

        public async Task OpenCourseCreate()
        {
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Large };

            var dialog = _dialogService.Show<CourseCreate>(_courseLocalizer["CreateCourse"], options);
            var result = await dialog.Result;

            if (result.Data != null && (bool)result.Data)
            {
                await GetCourses();
            }
        }

        public async Task Filter()
        {
            await GetCourses();
        }

        public async Task BackToFullList()
        {
            CoursesOverview.MetaData.SearchString = "";
            await GetCourses();
        }

        public async Task<TableData<CourseVM>> ServerReload(TableState state)
        {
            var searchString = CoursesOverview?.MetaData.SearchString ?? "";
            var sortString = state.GetSortString();

            var result = await _courseService.GetAllAsync(sortString, state.Page, searchString, state.PageSize);

            return new TableData<CourseVM>() { TotalItems = result.MetaData.TotalRecords, Items = result.Courses };
        }
    }
}
