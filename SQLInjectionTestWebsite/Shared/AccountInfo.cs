namespace SQLInjectionTestWebsite.Shared
{
    public class AccountInfo
    {
        public readonly string UserName;
        public readonly string Password;
        public readonly string Email;
        public readonly string ID;
        public decimal CurrentBalance;
        public readonly bool IsAdmin;

        public AccountInfo(string userName, string password, string email, string iD, decimal currentBalance)
        {
            UserName = userName;
            Password = password;
            Email = email;
            ID = iD;
            CurrentBalance = currentBalance;
        }
    }
}
