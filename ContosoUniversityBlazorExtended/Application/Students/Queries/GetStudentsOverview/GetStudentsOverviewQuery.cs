﻿using MediatR;
using WebUI.Shared.Students.Queries.GetStudentsOverview;

namespace ContosoUniversityBlazor.Application.Students.Queries.GetStudentsOverview
{
    public class GetStudentsOverviewQuery : IRequest<StudentsOverviewVM>
    {
        public string SortOrder { get; set; }
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }
        public int? PageNumber { get; set; }

        public GetStudentsOverviewQuery(string sortOrder, string currentFilter, 
            string searchString, int? pageNumber)
        {
            SortOrder = sortOrder;
            CurrentFilter = currentFilter;
            SearchString = searchString;
            PageNumber = pageNumber;
        }
    }
}
