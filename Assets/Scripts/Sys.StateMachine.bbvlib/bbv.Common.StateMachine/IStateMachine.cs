//-------------------------------------------------------------------------------
// <copyright file="IStateMachine.cs" company="bbv Software Services AG">
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

namespace bbv.Common.StateMachine
{
    using System;
    using Internals;

    /// <summary>
    /// A state machine.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IStateMachine<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// Occurs when no transition could be executed.
        /// </summary>
        event EventHandler<TransitionEventArgs<TState, TEvent>> TransitionDeclined;

        /// <summary>
        /// Occurs when an exception was thrown inside the state machine.
        /// </summary>
        event EventHandler<ExceptionEventArgs<TState, TEvent>> ExceptionThrown;

        /// <summary>
        /// Occurs when an exception was thrown inside a transition of the state machine.
        /// </summary>
        event EventHandler<TransitionExceptionEventArgs<TState, TEvent>> TransitionExceptionThrown;

        /// <summary>
        /// Occurs when a transition begins.
        /// </summary>
        event EventHandler<TransitionEventArgs<TState, TEvent>> TransitionBegin;

        /// <summary>
        /// Occurs when a transition completed.
        /// </summary>
        event EventHandler<TransitionCompletedEventArgs<TState, TEvent>> TransitionCompleted;

        /// <summary>
        /// Gets a value indicating whether this instance is running. The state machine is running if if was started and not yet stopped.
        /// </summary>
        /// <value><c>true</c> if this instance is running; otherwise, <c>false</c>.</value>
        bool IsRunning { get; }

        /// <summary>
        /// Define the behavior of a state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>Syntax to build state behavior.</returns>
        IEntryActionSyntax<TState, TEvent> In(TState state);

        /// <summary>
        /// Defines a state hierarchy.
        /// </summary>
        /// <param name="superStateId">The super state id.</param>
        /// <param name="initialSubStateId">The initial sub state id.</param>
        /// <param name="historyType">Type of the history.</param>
        /// <param name="subStateIds">The sub state ids.</param>
        void DefineHierarchyOn(TState superStateId, TState initialSubStateId, HistoryType historyType, params TState[] subStateIds);

        /// <summary>
        /// Fires the specified event.
        /// </summary>
        /// <param name="eventId">The event.</param>
        /// <param name="eventArguments">The event arguments.</param>
        void Fire(TEvent eventId, params object[] eventArguments);

        /// <summary>
        /// Fires the specified priority event. The event will be handled before any already queued event.
        /// </summary>
        /// <param name="eventId">The event.</param>
        /// <param name="eventArguments">The event arguments.</param>
        void FirePriority(TEvent eventId, params object[] eventArguments);

        /// <summary>
        /// Initializes the state machine to the specified initial state.
        /// </summary>
        /// <param name="initialState">The state to which the state machine is initialized.</param>
        void Initialize(TState initialState);

        /// <summary>
        /// Starts the state machine. Events will be processed.
        /// If the state machine is not started then the events will be queued until the state machine is started.
        /// Already queued events are processed
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the state machine. Events will be queued until the state machine is started.
        /// </summary>
        void Stop();

        /// <summary>
        /// Adds an extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        void AddExtension(IExtension<TState, TEvent> extension);

        /// <summary>
        /// Clears all extensions.
        /// </summary>
        void ClearExtensions();

        /// <summary>
        /// Creates a state machine report with the specified generator.
        /// </summary>
        /// <param name="reportGenerator">The report generator.</param>
        void Report(IStateMachineReport<TState, TEvent> reportGenerator);
    }
}