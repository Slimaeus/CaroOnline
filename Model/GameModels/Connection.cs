namespace Model.GameModels
{
    public class Connection
    {
        public string ConnectionID { get; set; } = default!;
        public string UserAgent { get; set; } = default!;
        public bool Connected { get; set; } = default!;
    }
}
