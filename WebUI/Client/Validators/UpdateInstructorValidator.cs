﻿using FluentValidation;
using WebUI.Client.InputModels.Instructors;

namespace WebUI.Client.Validators;

public class UpdateInstructorValidator : AbstractValidator<UpdateInstructorInputModel>
{
    public UpdateInstructorValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.LastName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.HireDate).NotEmpty();
    }
}
