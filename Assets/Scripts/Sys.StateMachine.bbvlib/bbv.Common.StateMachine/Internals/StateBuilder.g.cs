//-------------------------------------------------------------------------------
// <copyright file="StateBuilder.cs" company="bbv Software Services AG">
//   Copyright (c) 2008-2011 bbv Software Services AG
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace bbv.Common.StateMachine.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides operations to build a state machine.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public sealed class StateBuilder<TState, TEvent> : 
        IEntryActionSyntax<TState, TEvent>,
        IGotoInIfSyntax<TState, TEvent>,
        IOtherwiseSyntax<TState, TEvent>,
        IGotoSyntax<TState, TEvent>,
        IIfSyntax<TState, TEvent>,
        IOnSyntax<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        private readonly IState<TState, TEvent> state;

        private readonly IStateDictionary<TState, TEvent> stateDictionary;

        private readonly IFactory<TState, TEvent> factory;

        private ITransition<TState, TEvent> currentTransition;

        private TEvent currentEventId;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateBuilder&lt;TState, TEvent&gt;"/> class.
        /// </summary>
        /// <param name="state">The state to build.</param>
        /// <param name="stateDictionary">The state dictionary of the state machine.</param>
        /// <param name="factory">The factory.</param>
        public StateBuilder(IState<TState, TEvent> state, IStateDictionary<TState, TEvent> stateDictionary, IFactory<TState, TEvent> factory)
        {
            this.state = state;
            this.stateDictionary = stateDictionary;
            this.factory = factory;
        }

        /// <summary>
        /// Defines entry actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <returns>Exit action syntax.</returns>
        IExitActionSyntax<TState, TEvent> IEntryActionSyntax<TState, TEvent>.ExecuteOnEntry(params Action[] actions)
        {
            Ensure.ArgumentNotNull(actions, "actions");

            foreach (var action in actions)
            {
                this.state.EntryActions.Add(this.factory.CreateActionHolder(action));    
            }
            
            return this;
        }

        /// <summary>
        /// Defines an entry action.
        /// </summary>
        /// <typeparam name="T">Type of the parameter of the entry action method.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="parameter">The parameter that will be passed to the entry action.</param>
        /// <returns>Exit action syntax.</returns>
        IExitActionSyntax<TState, TEvent> IEntryActionSyntax<TState, TEvent>.ExecuteOnEntry<T>(Action<T> action, T parameter)
        {
            this.state.EntryActions.Add(this.factory.CreateActionHolder(action, parameter));

            return this;
        }

        /// <summary>
        /// Defines exit actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <returns>Event syntax.</returns>
        IEventSyntax<TState, TEvent> IExitActionSyntax<TState, TEvent>.ExecuteOnExit(params Action[] actions)
        {
            Ensure.ArgumentNotNull(actions, "actions");

            foreach (var action in actions)
            {
                this.state.ExitActions.Add(this.factory.CreateActionHolder(action));
            }

            return this;
        }

        /// <summary>
        /// Defines an exit action.
        /// </summary>
        /// <typeparam name="T">Type of the parameter of the exit action method.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="parameter">The parameter that will be passed to the exit action.</param>
        /// <returns>Exit action syntax.</returns>
        IEventSyntax<TState, TEvent> IExitActionSyntax<TState, TEvent>.ExecuteOnExit<T>(Action<T> action, T parameter)
        {
            this.state.ExitActions.Add(this.factory.CreateActionHolder(action, parameter));

            return this;
        }

        /// <summary>
        /// Builds a transition.
        /// </summary>
        /// <param name="eventId">The event that triggers the transition.</param>
        /// <returns>Syntax to build the transition.</returns>
        IOnSyntax<TState, TEvent> IEventSyntax<TState, TEvent>.On(TEvent eventId)
        {
            this.currentEventId = eventId;

            this.CreateTransition();

            return this;
        }

        private void CreateTransition()
        {
            this.currentTransition = this.factory.CreateTransition();
            this.state.Transitions.Add(this.currentEventId, this.currentTransition);
        }

        /// <summary>
        /// Defines where to go in response to an event.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>Execute syntax.</returns>
        IGotoSyntax<TState, TEvent> IOnSyntax<TState, TEvent>.Goto(TState target)
        {
            this.SetTargetState(target);

            return this;
        }

        IGotoSyntax<TState, TEvent> IOtherwiseSyntax<TState, TEvent>.Goto(TState target)
        {
            this.SetTargetState(target);

            return this;
        }

        IGotoInIfSyntax<TState, TEvent> IIfSyntax<TState, TEvent>.Goto(TState target)
        {
            this.SetTargetState(target);

            return this;
        }

        IEventSyntax<TState, TEvent> IOtherwiseSyntax<TState, TEvent>.Execute(params Action<object[]>[] actions)
        {
            return Execute(actions);
        }

        IEventSyntax<TState, TEvent> IOtherwiseSyntax<TState, TEvent>.Execute(params Action[] actions)
        {
            return Execute(actions);
        }

        IEventSyntax<TState, TEvent> IOtherwiseSyntax<TState, TEvent>.Execute<T>(params Action<T>[] actions)
        {
            return Execute<T>(actions);
        }

        IIfOrOtherwiseSyntax<TState, TEvent> IGotoInIfSyntax<TState, TEvent>.Execute(params Action<object[]>[] actions)
        {
            return Execute(actions);
        }

        IIfOrOtherwiseSyntax<TState, TEvent> IGotoInIfSyntax<TState, TEvent>.Execute(params Action[] actions)
        {
            return Execute(actions);
        }

        IIfOrOtherwiseSyntax<TState, TEvent> IGotoInIfSyntax<TState, TEvent>.Execute<T>(params Action<T>[] actions)
        {
            return Execute<T>(actions);
        }

        IEventSyntax<TState, TEvent> IGotoSyntax<TState, TEvent>.Execute(params Action<object[]>[] actions)
        {
            return Execute(actions);
        }

        IEventSyntax<TState, TEvent> IGotoSyntax<TState, TEvent>.Execute(params Action[] actions)
        {
            return Execute(actions);
        }

        IEventSyntax<TState, TEvent> IGotoSyntax<TState, TEvent>.Execute<T>(params Action<T>[] actions)
        {
            return Execute<T>(actions);
        }

        IIfOrOtherwiseSyntax<TState, TEvent> IIfSyntax<TState, TEvent>.Execute(params Action<object[]>[] actions)
        {
            return Execute(actions);
        }

        IIfOrOtherwiseSyntax<TState, TEvent> IIfSyntax<TState, TEvent>.Execute(params Action[] actions)
        {
            return Execute(actions);
        }

        IIfOrOtherwiseSyntax<TState, TEvent> IIfSyntax<TState, TEvent>.Execute<T>(params Action<T>[] actions)
        {
            return Execute<T>(actions);
        }

        /// <summary>
        /// Defines the actions to execute on a transition.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <returns>Guard syntax.</returns>
        IEventSyntax<TState, TEvent> IOnSyntax<TState, TEvent>.Execute(params Action<object[]>[] actions)
        {
            return this.Execute(actions);
        }

        IEventSyntax<TState, TEvent> IOnSyntax<TState, TEvent>.Execute(params Action[] actions)
        {
            return this.Execute(actions);
        }

        IEventSyntax<TState, TEvent> IOnSyntax<TState, TEvent>.Execute<T>(params Action<T>[] actions)
        {
            return this.Execute<T>(actions);
        }

        private StateBuilder<TState, TEvent> Execute(IEnumerable<Action<object[]>> actions)
        {
            if (actions == null)
            {
                return this;
            }

            foreach (var action in actions)
            {
                this.currentTransition.Actions.Add(this.factory.CreateTransitionActionHolder(action));
            }

            this.CheckGuards();
            
            return this;
        }

        private StateBuilder<TState, TEvent> Execute(IEnumerable<Action> actions)
        {
            if (actions == null)
            {
                return this;
            }

            foreach (var action in actions)
            {
                this.currentTransition.Actions.Add(this.factory.CreateTransitionActionHolder(action));
            }

            this.CheckGuards();

            return this;
        }

        private StateBuilder<TState, TEvent> Execute<T>(IEnumerable<Action<T>> actions)
        {
            if (actions == null)
            {
                return this;
            }

            foreach (var action in actions)
            {
                this.currentTransition.Actions.Add(this.factory.CreateTransitionActionHolder(action));
            }

            this.CheckGuards();

            return this;
        }

        /// <summary>
        /// Defines a guard for a transition.
        /// </summary>
        /// <param name="guard">The guard.</param>
        /// <returns>Event syntax.</returns>
        IIfSyntax<TState, TEvent> IOnSyntax<TState, TEvent>.If(Func<object[], bool> guard)
        {
            this.SetGuard(guard);

            return this;
        }

        IIfSyntax<TState, TEvent> IOnSyntax<TState, TEvent>.If<T>(Func<T, bool> guard)
        {
            this.SetGuard<T>(guard);

            return this;
        }

        IIfSyntax<TState, TEvent> IOnSyntax<TState, TEvent>.If(Func<bool> guard)
        {
            this.SetGuard(guard);

            return this;
        }

        IIfSyntax<TState, TEvent> IIfOrOtherwiseSyntax<TState, TEvent>.If(Func<object[], bool> guard)
        {
            this.CreateTransition();

            this.SetGuard(guard);

            return this;
        }

        IIfSyntax<TState, TEvent> IIfOrOtherwiseSyntax<TState, TEvent>.If<T>(Func<T, bool> guard)
        {
            this.CreateTransition();

            this.SetGuard<T>(guard);

            return this;
        }

        IIfSyntax<TState, TEvent> IIfOrOtherwiseSyntax<TState, TEvent>.If(Func<bool> guard)
        {
            this.CreateTransition();

            this.SetGuard(guard);

            return this;
        }

        IOtherwiseSyntax<TState, TEvent> IIfOrOtherwiseSyntax<TState, TEvent>.Otherwise()
        {
            this.CreateTransition();

            return this;
        }

        private void SetGuard(Func<object[], bool> guard)
        {
            this.currentTransition.Guard = this.factory.CreateGuardHolder(guard);
        }

        private void SetGuard<T>(Func<T, bool> guard)
        {
            this.currentTransition.Guard = this.factory.CreateGuardHolder(guard);
        }

        private void SetGuard(Func<bool> guard)
        {
            this.currentTransition.Guard = this.factory.CreateGuardHolder(guard);
        }

        private void SetTargetState(TState target)
        {
            this.currentTransition.Target = this.stateDictionary[target];

            this.CheckGuards();
        }

        private void CheckGuards()
        {
            var byEvent = this.state.Transitions.GetTransitions().GroupBy(t => t.EventId);
            var withMoreThenOneTransitionWithoutGuard = byEvent.Where(g => g.Count(t => t.Guard == null) > 1);

            if (withMoreThenOneTransitionWithoutGuard.Any())
            {
                throw new InvalidOperationException(ExceptionMessages.OnlyOneTransitionMayHaveNoGuard);
            }

            foreach (var group in byEvent)
            {
                var transition = group.SingleOrDefault(t => t.Guard == null);

                if (transition != null && group.LastOrDefault() != transition)
                {
                    throw new InvalidOperationException(ExceptionMessages.TransitionWithoutGuardHasToBeLast);
                }
            }
        }
    }
}