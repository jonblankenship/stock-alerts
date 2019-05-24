using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockAlerts.Data.Model
{
    [Table("Stocks")]
    public class Stock : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid StockId { get; set; }

        public string Symbol { get; set; }

        public decimal LastPrice { get; set; }

        public decimal PreviousLastPrice { get; set; }

        public DateTimeOffset LastTime { get; set; }
    }
}
