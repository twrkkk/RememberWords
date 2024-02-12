namespace NetSchool.Common.Exceptions;

public class EntityNotFoundException : Exception
{
    public string Name { get; }


    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string message) : base(message)
    {
    }

    public EntityNotFoundException(Exception inner) : base(inner.Message, inner)
    {
    }

    public EntityNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }

    public static void ThrowIf(Func<bool> predicate, string message)
    {
        if (predicate.Invoke())
            throw new EntityNotFoundException(message);
    }
}
