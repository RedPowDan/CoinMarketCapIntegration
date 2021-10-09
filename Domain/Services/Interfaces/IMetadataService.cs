using System.Linq;
using Domain.Dtos.Crypto;
using Domain.Models;

namespace Domain.Services.Interfaces
{
    public interface IMetadataService
    {
        public Metadata[] GetMetadataForIds(int[] Ids);

        public void UpdateOrCreateMetadataForModels(Metadata[] newMetadataModels);

        public int[] GetIds(Metadata[] models);

        public int[] GetCryptoIds(Metadata[] models);

        public Metadata MapToMetadata(string idCryptoModel, MetadataDto metadata);
    }
}