using SQLInjectionTestWebsite.Shared.SQL;

namespace SQLInjectionTestWebsite.Shared
{
    [SQLSerializeableObject]
    public class AccountInfo
    {
        [SQLSerializeableField]
        public readonly string UserName;
		[SQLSerializeableField]
		public readonly string Password;
		[SQLSerializeableField]
		public readonly string Email;
		[SQLSerializeableField]
		public readonly string CreditCardNumber;
		[SQLSerializeableField]
		public readonly string ID;
		[SQLSerializeableField]
		public float CurrentBalance;
		[SQLSerializeableField]
		public readonly bool IsAdmin;
        [SQLSerializeableField]
        public readonly string CartItems;

        public AccountInfo(string userName, string password, string email, string creditCardNumber, string iD, float currentBalance, bool isAdmin, string cartItems)
        {
            UserName = userName;
            Password = password;
            Email = email;
            CreditCardNumber = creditCardNumber;
            ID = iD;
            CurrentBalance = currentBalance;
            IsAdmin = isAdmin;
            CartItems = cartItems;
        }

        public static AccountInfo GenAccount(string userName, string password, string email, string creditCardNumber, float currentBalance = 0, bool isAdmin = false, string cartItems = "")
        {
            string id = Guid.NewGuid().ToString();
            return new AccountInfo(userName, password, email, creditCardNumber, id, currentBalance, isAdmin, cartItems);
        }
    }
}
