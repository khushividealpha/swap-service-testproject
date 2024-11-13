using System.Collections.Concurrent;

namespace swap_service.Models
{
    public class NetPositions
    {
        private decimal currentPrice;
        private decimal openMTM;
        private decimal balance;
        private decimal charges;
        private decimal totalSellAmount;
        private decimal totalSellUnits;
        private decimal totalBuyAmount;
        private decimal totalBuyUnits;
        private decimal sellAvg;
        private decimal buyAvg;
        private decimal bookedPNL;
        private decimal equity;
        private double openAvg;
        private decimal _leverage;
        private decimal _commissionRate;
        private decimal _totalCommission;
        //private decimal _totalPendingCommission;
        private decimal _totalBuyUsedMargin;
        private decimal _totalSellUsedMargin;

        private decimal _openCommission;
        private decimal _netQty;
        private decimal _usedMargin;
        private decimal _newMargin;
        private decimal _openUnits;
        private decimal marginLevel;
        private decimal marginAvailable;
        private decimal totalMargin;

        public ConcurrentDictionary<string, decimal> dctNewMarginOrders { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Symbol { get; set; }

        public decimal CurrentPrice
        {
            get => this.currentPrice;
            set
            {
                this.currentPrice = value;
                //this.OnPropertyChange(nameof(CurrentPrice));
            }
        }

        public decimal OpenMTM
        {
            get => this.openMTM;
            set
            {
                this.openMTM = value;
                //this.OnPropertyChange(nameof(OpenMTM));
            }
        }

        public decimal Balance
        {
            get => this.balance;
            set
            {
                if (this.balance == value)
                    return;
                this.balance = value;
                //this.OnPropertyChange(nameof(Balance));
            }
        }



        public decimal TotalSellAmount
        {
            get => this.totalSellAmount;
            set
            {
                this.totalSellAmount = value;
                //this.OnPropertyChange(nameof(TotalSellAmount));
            }
        }

        public decimal TotalSellUnits
        {
            get => this.totalSellUnits;
            set
            {
                this.totalSellUnits = value;
                //this.OnPropertyChange(nameof(TotalSellUnits));
            }
        }

        public decimal TotalBuyAmount
        {
            get => this.totalBuyAmount;
            set
            {
                this.totalBuyAmount = value;
                //this.OnPropertyChange(nameof(TotalBuyAmount));
            }
        }

        public decimal TotalBuyUnits
        {
            get => this.totalBuyUnits;
            set
            {
                this.totalBuyUnits = value;
                //this.OnPropertyChange(nameof(TotalBuyUnits));
            }
        }

        public decimal SellAvg
        {
            get => this.sellAvg;
            set
            {
                this.sellAvg = value;
                //this.OnPropertyChange(nameof(SellAvg));
            }
        }

        public decimal BuyAvg
        {
            get => this.buyAvg;
            set
            {
                this.buyAvg = value;
                //this.OnPropertyChange(nameof(BuyAvg));
            }
        }



        public decimal BookedPNL
        {
            get => this.bookedPNL;
            set
            {
                this.bookedPNL = value;
                //this.OnPropertyChange(nameof(BookedPNL));
            }
        }

        public decimal Equity
        {
            get => this.equity;
            set
            {
                this.equity = value;
                //this.OnPropertyChange(nameof(Equity));
            }
        }

        public decimal Multiplier { get; set; }

        public double OpenAvg
        {
            get => this.openAvg;
            set
            {
                this.openAvg = value;
                //this.OnPropertyChange(nameof(OpenAvg));
            }
        }

        public decimal leverage
        {
            get => this._leverage;
            set
            {
                this._leverage = value;
                //this.OnPropertyChange(nameof(leverage));
            }
        }


        //public decimal TotalPendingCommission
        //{
        //    get => this._totalPendingCommission;
        //    set
        //    {
        //        this._totalPendingCommission = value;
        //        //this.OnPropertyChange(nameof(TotalPendingCommission));
        //    }
        //}

        public decimal TotalCommission
        {
            get => this._totalCommission;
            set
            {
                this._totalCommission = value;
                //this.OnPropertyChange(nameof(TotalCommission));
            }
        }

        public decimal TotalBuyUsedMargin
        {
            get => this._totalBuyUsedMargin;
            set
            {
                this._totalBuyUsedMargin = value;
                //this.OnPropertyChange(nameof(TotalBuyUsedMargin));
            }
        }

        public decimal TotalSellUsedMargin
        {
            get => this._totalSellUsedMargin;
            set
            {
                this._totalSellUsedMargin = value;
                //this.OnPropertyChange(nameof(TotalSellUsedMargin));
            }
        }



        public decimal OpenCommission
        {
            get => this._openCommission;
            set
            {
                this._openCommission = value;
                //this.OnPropertyChange(nameof(OpenCommission));
            }
        }

        public decimal NetQty
        {
            get => this._netQty;
            set
            {
                this._netQty = value;
                //this.OnPropertyChange(nameof(NetQty));
            }
        }

        public decimal UsedMargin
        {
            get => this._usedMargin;
            set
            {
                this._usedMargin = value;
                //this.OnPropertyChange(nameof(UsedMargin));
            }
        }

        public decimal NewMargin
        {
            get => this._newMargin;
            set
            {
                this._newMargin = value;
                //this.OnPropertyChange(nameof(NewMargin));
            }
        }

        public decimal OpenUnits
        {
            get => this._openUnits;
            set
            {
                this._openUnits = value;
                //this.OnPropertyChange(nameof(OpenUnits));
            }
        }

        public decimal MarginLevel
        {
            get => this.marginLevel;
            set
            {
                this.marginLevel = value;
                //this.OnPropertyChange(nameof(MarginLevel));
            }
        }

        public decimal MarginAvailable
        {
            get => this.marginAvailable;
            set
            {
                this.marginAvailable = value;
                //this.OnPropertyChange(nameof(MarginAvailable));
            }
        }

        public decimal TotalMargin
        {
            get => this.totalMargin;
            set
            {
                this.totalMargin = value;
                //this.OnPropertyChange(nameof(TotalMargin));
            }
        }



        public decimal BuyPrice { get; internal set; }

        public decimal SellPrice { get; internal set; }

        public decimal BuyQty { get; internal set; }

        public decimal SellQty { get; internal set; }

        public string OrderSide { get; internal set; }

        public string Currency { get; set; }

        public decimal BrokageAmount { get; internal set; }
        public decimal OpenValue { get; internal set; }

        public decimal NonUSDMargin { get; internal set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
