namespace SmartGym.Shared.Errors.Models
{
    public class UnAuthorizedExeption : ApplicationException
    {
        public UnAuthorizedExeption(string message) : base(message)
        {

        }
    }
}
