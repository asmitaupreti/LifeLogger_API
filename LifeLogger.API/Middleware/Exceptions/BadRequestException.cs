
namespace LifeLogger.API.Middleware.Exceptions
{
    public class BadRequestException: Exception
    {
        public BadRequestException(string message): base(message)
        {
            
        }
    }
}