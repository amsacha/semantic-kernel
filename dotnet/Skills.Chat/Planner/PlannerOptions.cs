// Copyright (c) Microsoft. All rights reserved.
using System.ComponentModel.DataAnnotations;

namespace Microsoft.SemanticKernel.Skills.Chat.Planner;

/// <summary>
/// Configuration options for the planner.
/// </summary>
public class PlannerOptions
{
    public const string PropertyName = "Planner";

    /// <summary>
    /// Define if the planner must be Sequential or not.
    /// </summary>
    [Required]
    public PlanType Type { get; set; } = PlanType.Action;
}
