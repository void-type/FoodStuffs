using FoodStuffs.Model.Validation.Core;
using System;
using System.Collections.Generic;

namespace FoodStuffs.Model.Actions.Core.Responder
{
    /// <summary>
    /// Adapter for ActionSteps to use any response implementation. Retains validation errors and final response.
    /// </summary>
    public interface IActionResponder
    {
        /// <summary>
        /// Checks the response for a non-default value.
        /// </summary>
        bool ResponseCreated { get; }

        /// <summary>
        /// A list of validation errors that can be appended between steps.
        /// </summary>
        List<IValidationError> ValidationErrors { get; }

        /// <summary>
        /// Try to send a response from validation errors. If no errors are stored, no response will be made and the method will return false.
        /// </summary>
        /// <param name="logExtra"></param>
        /// <returns></returns>
        bool TryWithValidationError(string logExtra = null);

        /// <summary>
        /// Create a response with an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="logExtra"></param>
        void WithData<T>(T item, string logExtra = null);

        /// <summary>
        /// Create a response with a set of objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="logExtra"></param>
        void WithDataList<T>(IEnumerable<T> items, string logExtra = null);

        /// <summary>
        /// Create a response with a fatal error message.
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="logExtra"></param>
        /// <param name="ex"></param>
        void WithError(string userMessage, string logExtra = null, Exception ex = null);

        /// <summary>
        /// Create a response with a success message.
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="logExtra"></param>
        void WithSuccess(string userMessage, string logExtra = null);

        /// <summary>
        /// Append a list of validation errors and create a response with the stored set.
        /// </summary>
        /// <param name="newValidationErrors"></param>
        /// <param name="logExtra"></param>
        void WithValidationErrors(string logExtra, params IValidationError[] newValidationErrors);
    }
}