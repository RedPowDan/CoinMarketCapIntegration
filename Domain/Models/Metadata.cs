namespace Domain.Models
{
    public class Metadata
    {
        public int Id { get; set; }
        public string LogoUri { get; set; }
        public Crypto Crypto { get; set; }

        public void MapToModel(Metadata metadata)
        {
            metadata.LogoUri = LogoUri;
        }
    }
}