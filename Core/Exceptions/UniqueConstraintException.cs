namespace Core.Exceptions;

public class UniqueConstraintException(string message) : Exception(message);