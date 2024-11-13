using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class InstrumentSwapModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? CalculationMethod { get; set; }
    public List<Decimal> DayMultiplier { get; set; }
    public string? MotherCurrency { get; set; }
    public Decimal ShortSwap { get; set; }
    public Decimal LongSwap { get; set; }
    public Decimal TickSize { get; set; }
    public string?  InstrumentName { get; set; }
}