namespace ASPProject.Tests.Tests.UI
{
    public class Login
    {
        public Login(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public string Username { get; private set; }

        public string Password { get; private set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Login;
            if (compareTo == null)
            {
                return false;
            }

            return compareTo.Username == this.Username && compareTo.Password == this.Password;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Username?.GetHashCode() ?? 0) * 397) ^ (this.Password?.GetHashCode() ?? 0);
            }
        }
    }
}