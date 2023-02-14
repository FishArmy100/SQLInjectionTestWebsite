using SQLInjectionTestWebsite.Shared.Utils;

namespace SQLInjectionTestWebsite.Shared
{
	public static class Store
	{
		public static Option<AccountInfo> CurrentUser 
		{ 
			get { return s_CurrentUser; }
			set { s_CurrentUser = value; CurrentUserChanged?.Invoke(null, s_CurrentUser); }
		}

		private static Option<AccountInfo> s_CurrentUser = new Option<AccountInfo>();
		public static event EventHandler<Option<AccountInfo>>? CurrentUserChanged;

		public static void RefreshCurrentAccount()
		{
			CurrentUser.Match(ok =>
			{
				CurrentUser = WebsiteDatabase.TryGetAccount(ok.UserName, ok.Password);
			});
		}
	}
}
