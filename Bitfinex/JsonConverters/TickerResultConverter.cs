﻿using System;
using System.Collections.Generic;
using Bitfinex.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bitfinex
{
    public class TickersResultConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<ITicker>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray array = JArray.Load(reader);

            var results = new List<ITicker>();

            foreach (var item in array)
            {
                var ticker = JsonConvert.DeserializeObject<ITicker>(item.ToString(), new TickerResultConverter());
                results.Add(ticker);
            }

            return results;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }
    }

    public class TickerResultConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ITicker);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            JArray array = JArray.Load(reader);

            string symbol = (string) array[0];

            if (symbol.StartsWith("t")) return JArrayToTradingTicker(array);
            if (symbol.StartsWith("f")) return JArrayToFundingTicker(array);

            return null;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private TradingTicker JArrayToTradingTicker(JArray array)
        {
            string symbol = (string)array[0];
            string symbolStripped = symbol.Substring(1);

            return new TradingTicker
            {
                Symbol = symbolStripped,
                Bid = (double)array[1],
                BidSize = (double)array[2],
                Ask = (double)array[3],
                AskSize = (double)array[4],
                DailyChange = (double)array[5],
                DailyChangePercent = (double)array[6],
                LastPrice = (double)array[7],
                Volume = (double)array[8],
                High = (double)array[9],
                Low = (double)array[10]
            };           
        }

        private FundingTicker JArrayToFundingTicker(JArray array)
        {
            string symbol = (string)array[0];
            string symbolStripped = symbol.Substring(1);

            return new FundingTicker
            {
                Symbol = symbolStripped,
                FlashReturnRate = (double)array[1],
                Bid = (double)array[2],
                BidPeriod = (int)array[3],
                BidSize = (double)array[4],
                Ask = (double)array[5],
                AskPeriod = (int)array[6],
                AskSize = (double)array[7],
                DailyChange = (double)array[8],
                DailyChangePercent = (double)array[9],
                LastPrice = (double)array[10],
                Volume = (double)array[11],
                High = (double)array[12],
                Low = (double)array[13]
            };
        }
    }
}
