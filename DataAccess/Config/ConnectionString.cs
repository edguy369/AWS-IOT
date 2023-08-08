namespace DataAccess.Config
{
    internal class ConnectionString
    {
        public string Value { get; }
        public ConnectionString(string value) => Value = value;
    }
}
