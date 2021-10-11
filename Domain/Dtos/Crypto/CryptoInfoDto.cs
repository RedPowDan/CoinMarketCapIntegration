namespace Domain.Dtos.Crypto
{
    using System;
    using System.ComponentModel;

    public class CryptoInfoDto
    {
        [DisplayName("Наименование")]
        public string Name { get; set; }

        [DisplayName("Условное обозначение")]
        public string Symbol { get; set; }

        [DisplayName("Логотип")]
        public string Logo { get; set; }

        [DisplayName("Текущая цена в долларах")]
        public decimal CurrentPriceUsd { get; set; }

        [DisplayName("Процентное изменение за час")]
        public decimal PercentChangePerHour { get; set; }

        [DisplayName("Процент изменения за день")]
        public decimal PercentChangePerDay { get; set; }

        [DisplayName("Рыночная капитализация")]
        public decimal CapitalizationMarketCap { get; set; }

        [DisplayName("Дата и время обновления")]
        public DateTime UpdateTime { get; set; }
    }
}