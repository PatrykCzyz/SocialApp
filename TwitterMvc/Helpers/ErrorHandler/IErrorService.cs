namespace TwitterMvc.Helpers.ErrorHandler
{
    public interface IErrorService
    {
        public string GetError(Error errorKey);
    }
}