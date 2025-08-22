using VoidCore.Model.Responses.Messages;

namespace FoodStuffs.Model.Events;

/// <summary>
/// A UI-friendly message and the entity that was affected during an event.
/// </summary>
public class EntityResponse<TEntity> : UserMessage
{
    /// <summary>
    /// Create a new UserMessage with an entity.
    /// </summary>
    /// <param name="message">The Ui-Friendly message</param>
    /// <param name="entity">The entity affected during an event</param>
    internal EntityResponse(string message, TEntity entity) : base(message)
    {
        Entity = entity;
    }

    /// <summary>
    /// The entity affected during an event.
    /// </summary>
    public TEntity Entity { get; }
}
