namespace swap_service.Models
{
    enum EnumSwapMethod
    {
        None = -1,
        ByPoint = 0,
        ByMoney = 1,
        ByPercentage
    }

    public enum DayOfWeek
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday

    }

    public enum EnumStatus
    {
        NEW,
        FILLED,
        REJECTED,
        CANCELLED,
        CANCELMODIFY,
        PARTIALLYFILLED
    }

    public enum EnumSLTPStatus
    {
        None,
        Target,
        Stoploss
    }
}
