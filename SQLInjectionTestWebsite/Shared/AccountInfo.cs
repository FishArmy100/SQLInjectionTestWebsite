namespace SQLInjectionTestWebsite.Shared
{
    public class AccountInfo
    {
        public readonly string UserName;
        public readonly string Password;
        public readonly string Email;
        public readonly string CreditCardNumber;
        public readonly string ID;
        public decimal CurrentBalance;
        public readonly bool IsAdmin;

        public AccountInfo(string userName, string password, string email, string creditCardNumber, string iD, decimal currentBalance, bool isAdmin)
        {
            UserName = userName;
            Password = password;
            Email = email;
            CreditCardNumber = creditCardNumber;
            ID = iD;
            CurrentBalance = currentBalance;
            IsAdmin = isAdmin;
        }

        public static AccountInfo GenAccount(string userName, string password, string email, string creditCardNumber, decimal currentBalance = 0, bool isAdmin = false)
        {
            string id = Guid.NewGuid().ToString();
            return new AccountInfo(userName, password, email, creditCardNumber, id, currentBalance, isAdmin);
        }
    }
}
