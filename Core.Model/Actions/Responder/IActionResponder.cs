using Core.Model.Validation;
using System;
using System.Collections.Generic;

namespace Core.Model.Actions.Responder
{
    /// <summary>
    /// ActionResponder retains validation errors between steps and sets the final response.
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
        /// Create a response with a fatal error message.
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="logExtra"></param>
        /// <param name="ex"></param>
        void WithError(string userMessage, string logExtra = null, Exception ex = null);

        /// <summary>
        /// Create a response with an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="logExtra"></param>
        void WithItem<T>(T item, string logExtra = null);

        /// <summary>
        /// Create a response with a set of objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="logExtra"></param>
        void WithSet<T>(IEnumerable<T> items, string logExtra = null);

        /// <summary>
        /// Create a response with success message and the id of the created/modified entity.
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="id"></param>
        /// <param name="logExtra"></param>
        void WithPostSuccess(string userMessage, string id, string logExtra = null);

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