using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamTestInvestigate
{
    public class DataSource
    {
        public Guid DataGuid { get; set; }
        public string Title { get; set; }

        public string Message { get; set; }

        public Color CardColor { get; set; }
    }
}
