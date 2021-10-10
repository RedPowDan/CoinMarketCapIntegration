using System.Collections.Generic;

namespace Domain.Dtos.Crypto
{
    public class MetadataResponseDto
    {
        public StatusDto Status { get; set; }
        public Dictionary<string, MetadataDto> Data { get; set; }
    }
}
