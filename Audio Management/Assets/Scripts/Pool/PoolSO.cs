using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic pool that generates members of type T on-demand via a factory.
/// </summary>
/// <typeparam name="T">Specifies the type of elements to pool.</typeparam>
public abstract class PoolSO<T> : ScriptableObject, IPool<T>
{
    [SerializeField, Min(1)]
    protected uint initialSize = 1;

    protected readonly Stack<T> Available = new Stack<T>();

    /// <summary>
    /// The factory which will be used to create <typeparamref name="T"/> on demand.
    /// </summary>
    public abstract IFactory<T> Factory { get; set; }

    /// <summary>
    /// Shows if the pool has been prewarmed. 
    /// </summary>
    protected bool HasBeenPrewarmed { get; set; }

    /// <summary>
    /// Creates the pool of objects by initializing the factory of objects of type T.
    /// </summary>
    /// <returns></returns>
    protected virtual T Create() => Factory.Create();

    /// <summary>
    /// Prewarms the pool with a <paramref name="num"/> of <typeparamref name="T"/>.
    /// </summary>
    /// <param name="num">The number of members to create as a part of this pool.</param>
    /// <remarks>NOTE: This method can be called at any time, but only once for the lifetime of the pool.</remarks>
    public virtual void Prewarm(int num)
    {
        if (HasBeenPrewarmed)
        {
            Debug.LogWarning($"Pool {name} has already been prewarmed.");
            return;
        }

        if (num <= 0)
        {
            Debug.LogError($"Not valid amount of members to prewarm was given ({num}).");
            return;
        }

        // Assign initial size and prewarm.
        initialSize = (uint)num;
        Prewarm();
    }

    /// <summary>
    /// Prewarms the pool with a <paramref name="initialSize"/> of <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>NOTE: This method can be called at any time, but only once for the lifetime of the pool.</remarks>
    public virtual void Prewarm()
    {
        if (HasBeenPrewarmed)
        {
            Debug.LogWarning($"Pool {name} has already been prewarmed.");
            return;
        }

        for (int i = 0; i < initialSize; i++)
            Available.Push(Create());

        HasBeenPrewarmed = true;
    }

    /// <summary>
    /// Requests a <typeparamref name="T"/> from this pool.
    /// </summary>
    /// <returns>The requested <typeparamref name="T"/>.</returns>
    public virtual T Request() => Available.Count > 0 ? Available.Pop() : Create();

    /// <summary>
    /// Batch requests a <typeparamref name="T"/> collection from this pool.
    /// </summary>
    /// <returns>A <typeparamref name="T"/> collection.</returns>
    public virtual IEnumerable<T> Request(int num = 1)
    {
        List<T> members = new (num);

        for (int i = 0; i < num; i++)
            members.Add(Request());

        return members;
    }

    /// <summary>
    /// Returns a <typeparamref name="T"/> to the pool.
    /// </summary>
    /// <param name="member">The <typeparamref name="T"/> to return.</param>
    public virtual void Return(T member) => Available.Push(member);

    /// <summary>
    /// Returns a <typeparamref name="T"/> collection to the pool.
    /// </summary>
    /// <param name="members">The <typeparamref name="T"/> collection to return.</param>
    public virtual void Return(IEnumerable<T> members)
    {
        foreach (T member in members)
            Return(member);
    }

    public virtual void OnDisable()
    {
        Available.Clear();

        HasBeenPrewarmed = false;
    }
}