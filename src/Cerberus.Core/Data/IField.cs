﻿namespace Sitecore.Helix.Validator.Common.Data
{
    using System;

    public interface IField
    {
        Guid Id { get; set; }
        string Name { get; set; }
        ITemplate Template { get; set; }
    }
}