﻿namespace WebUI.Client.Pages.Home;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Shared.Home.Queries.GetAboutInfo;

public partial class About
{
    private AboutInfoVM aboutInfo;

    [Inject]
    public HttpClient Http { get; set; }

    [Inject]
    public IStringLocalizer<About> Localizer { get; set; }

    public List<ChartSeries> Series { get; set; } = [];
    public string[] XAxisLabels { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        aboutInfo = await Http.GetFromJsonAsync<AboutInfoVM>("/api/about");

        XAxisLabels = aboutInfo.Items.Select(x => x.EnrollmentDate.Value.ToShortDateString()).ToArray();
        var chartSeries = new ChartSeries() { Name = "Students", Data = aboutInfo.Items.Select(x => (double)x.StudentCount).ToArray() };
        Series = [chartSeries];
    }
}
