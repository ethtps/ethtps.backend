﻿using System;

namespace ETHTPS.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DisabledAttribute : Attribute
    {

    }
}
