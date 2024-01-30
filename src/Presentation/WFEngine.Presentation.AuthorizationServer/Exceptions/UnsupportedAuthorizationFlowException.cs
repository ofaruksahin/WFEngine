namespace WFEngine.Presentation.AuthorizationServer.Exceptions
{
    internal class UnsupportedAuthorizationFlowException : Exception
    {
        public UnsupportedAuthorizationFlowException()
            : base ("Unsupported authorization flow")
        {
            
        }
    }
}
