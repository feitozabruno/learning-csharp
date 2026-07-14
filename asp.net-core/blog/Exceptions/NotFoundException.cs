namespace Blog.Exceptions;

public class NotFoundException(string resourceName, object resourceId) : Exception(
    $"{resourceName} com id '{resourceId}' não foi encontrado.")
{ }
