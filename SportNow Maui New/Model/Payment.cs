﻿using System;
using Microsoft.Maui;

namespace SportNow.Model
{
    public class Payment
    {
        public string id { get; set; }
        public string name { get; set; }
        public string invoiceid { get; set; }
        public string orderid { get; set; }
        public string entity { get; set; }
        public string reference { get; set; }
        public double value { get; set; }
        public string status { get; set; }
        public string statusText { get; set; }
        public string participationid { get; set; }
        public string type { get; set; }

    }


}
