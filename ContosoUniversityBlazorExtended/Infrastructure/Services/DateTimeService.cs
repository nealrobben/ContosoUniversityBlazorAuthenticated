﻿using ContosoUniversityBlazor.Application.Common.Interfaces;
using System;

namespace ContosoUniversityBlazor.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
