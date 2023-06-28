// Copyright (c) Microsoft. All rights reserved.

using System.Threading.Tasks;
using Microsoft.SemanticKernel.Planning;
using Microsoft.SemanticKernel.SkillDefinition;

namespace Microsoft.SemanticKernel.Skills.Chat.Planner;

/// <summary>
/// A lightweight wrapper around a planner to allow for curating which skills are available to it.
/// </summary>
public class ChatPlanner
{
    /// <summary>
    /// The planner's kernel.
    /// </summary>
    public IKernel Kernel { get; }

    /// <summary>
    /// Options for the planner.
    /// </summary>
    private readonly PlannerOptions? _plannerOptions;

    /// <summary>
    /// Gets the pptions for the planner.
    /// </summary>
    public PlannerOptions? PlannerOptions => this._plannerOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatPlanner"/> class.
    /// </summary>
    /// <param name="plannerKernel">The planner's kernel.</param>
    /// <param name="plannerOptions"></param>
    public ChatPlanner(IKernel plannerKernel, PlannerOptions? plannerOptions)
    {
        this.Kernel = plannerKernel;
        this._plannerOptions = plannerOptions;
    }

    /// <summary>
    /// Create a plan for a goal.
    /// </summary>
    /// <param name="goal">The goal to create a plan for.</param>
    /// <returns>The plan.</returns>
    public Task<Plan> CreatePlanAsync(string goal)
    {
        FunctionsView plannerFunctionsView = this.Kernel.Skills.GetFunctionsView(true, true);
        if (plannerFunctionsView.NativeFunctions.IsEmpty && plannerFunctionsView.SemanticFunctions.IsEmpty)
        {
            // No functions are available - return an empty plan.
            return Task.FromResult(new Plan(goal));
        }

        if (this._plannerOptions?.Type == PlanType.Sequential)
        {
            return new SequentialPlanner(this.Kernel).CreatePlanAsync(goal);
        }

        return new ActionPlanner(this.Kernel).CreatePlanAsync(goal);
    }
}
