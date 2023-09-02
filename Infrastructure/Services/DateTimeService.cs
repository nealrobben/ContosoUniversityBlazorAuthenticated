namespace ContosoUniversityBlazor.Infrastructure.Services;

using ContosoUniversityBlazor.Application.Common.Interfaces;
using System;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
