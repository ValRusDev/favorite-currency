namespace FavoriteCurrency.CurrencyUpdater.Worker.Entities
{
    public sealed class CurrencyRecord
    {
        private CurrencyRecord()
        {
        }

        public CurrencyRecord(Guid id, string code, string name, decimal rate, DateTime updatedAtUtc)
        {
            Id = id;
            Code = code;
            Name = name;
            Rate = rate;
            UpdatedAtUtc = updatedAtUtc;
        }

        public Guid Id { get; private set; }

        public string Code { get; private set; } = null!;

        public string Name { get; private set; } = null!;

        public decimal Rate { get; private set; }

        public DateTime UpdatedAtUtc { get; private set; }

        public void Update(string name, decimal rate, DateTime updatedAtUtc)
        {
            Name = name;
            Rate = rate;
            UpdatedAtUtc = updatedAtUtc;
        }
    }
}
