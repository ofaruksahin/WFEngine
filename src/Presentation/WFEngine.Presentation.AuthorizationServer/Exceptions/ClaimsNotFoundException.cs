namespace WFEngine.Presentation.AuthorizationServer.Exceptions
{
    internal class ClaimsNotFoundException : Exception
	{
		public ClaimsNotFoundException()
			:base("User claims not found for login")
		{

		}
	}
}

