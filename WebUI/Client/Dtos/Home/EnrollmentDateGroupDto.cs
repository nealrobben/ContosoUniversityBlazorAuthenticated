﻿using System;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Client.Dtos.Home;

public class EnrollmentDateGroupDto
{
    public DateTime? EnrollmentDate { get; set; }

    public int StudentCount { get; set; }
}
