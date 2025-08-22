namespace FoodStuffs.Model.Events;

/// <summary>
/// Static helpers to create messages
/// </summary>
public static class EntityResponse
{
    /// <summary>
    /// Create a new UserMessage with an entity
    /// </summary>
    /// <param name="message">A UI-friendly message</param>
    /// <param name="entity">The entity affected during an event</param>
    /// <typeparam name="TEntity">The type of the entity</typeparam>
    public static EntityResponse<TEntity> Create<TEntity>(string message, TEntity entity)
    {
        return new EntityResponse<TEntity>(message, entity);
    }
}
