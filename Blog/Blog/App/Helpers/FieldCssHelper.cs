﻿using Microsoft.AspNetCore.Components.Forms;

namespace Blog.App.Helpers;

public class FieldCssHelper : FieldCssClassProvider
{
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        return editContext.GetValidationMessages(fieldIdentifier).Any() ? "is-invalid" : "is-valid";
    }
}