namespace APBD_example_test1_2025.Exceptions;

public class InvalidException : Exception
{
    public InvalidException()
    {
    }

    public InvalidException(string? message) : base(message)
    {
    }

    public InvalidException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}