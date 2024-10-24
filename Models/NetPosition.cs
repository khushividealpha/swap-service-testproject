namespace SwapAnalyzer.Models
{
    internal class NetPosition
    {
        public string symbol { get; set; }
        public double NetQty { get; set; }

        public double Multiplier { get; set; }

        public string userId { get; set; }

        public double OpenAvg { get; set; }
        public double ContractMultiplier { get; set; }

    }
}
