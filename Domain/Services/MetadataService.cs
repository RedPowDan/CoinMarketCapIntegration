using Domain.Dtos.Crypto;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    using Interfaces;
    using System.Linq;
    using Models;

    public class MetadataService : IMetadataService
    {
        private readonly DomainContext _context;
        private readonly ILogger<MetadataService> _logger;
        public MetadataService(
            DomainContext context,
            ILogger<MetadataService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Metadata[] GetMetadataForIds(int[] Ids)
        {
            return _context
                    .Metadata
                    .Where(l => Ids.Contains(l.Id))
                    .ToArray();
        }

        public void UpdateOrCreateMetadataForModels(Metadata[] newMetadataModels)
        {
            foreach (var newMetadata in newMetadataModels)
            {
                UpdateOrCreateMetadata(newMetadata);
            }

            _context.SaveChanges();
        }

        private void UpdateOrCreateMetadata(Metadata newMetadata)
        {
            var oldMetadata = _context.Metadata
                .FirstOrDefault(x => x.Crypto.ID == newMetadata.Crypto.ID);

            if (oldMetadata == null)
            {
                _context.Metadata.Add(newMetadata);
                return;
            }

            if (oldMetadata.LogoUri != newMetadata.LogoUri)
            {
                oldMetadata.LogoUri = newMetadata.LogoUri;
            }
        }

        public int[] GetIds(Metadata[] models)
        {
            if (models == null && models.Length == 0)
            {
                return null;
            }

            return models
                .GroupBy(x => x.Id)
                .Select(y => y.Key)
                .ToArray();
        }

        public int[] GetCryptoIds(Metadata[] models)
        {
            if (models == null && models.Length == 0)
            {
                return null;
            }

            return models
                .GroupBy(x => x.Crypto.ID)
                .Select(y => y.Key)
                .ToArray();
        }
        
        public Metadata MapToMetadata(string idCryptoModel, MetadataDto metadata)
        {
            var id = int.Parse(idCryptoModel);
            var cryptoModel = _context.Cryptos.FirstOrDefault(x => x.IdInApi == id);

            if (cryptoModel == null)
            {
                _logger.LogInformation($"CryptoModel is not found with id = {id}");
                return null;
            }

            return new Metadata()
            {
                Crypto = cryptoModel,
                LogoUri = metadata.Logo
            };
        }
    }
}