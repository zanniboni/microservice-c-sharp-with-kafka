namespace Post.Cmd.Infrastructure.Config
{
    public class MongoDBConfig
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }
}