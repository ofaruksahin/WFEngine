namespace WFEngine.Presentation.AuthorizationServer.Exceptions
{
    public class UnsupportedAuthorizationFlowException : Exception
    {
        public UnsupportedAuthorizationFlowException()
            : base ("Unsupported authorization flow")
        {
            
        }
    }
}
