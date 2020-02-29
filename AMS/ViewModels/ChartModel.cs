using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.ViewModels
{
    public class ChartModel
    {
        public ChartModel()
        {
            Labels = new List<string>();
            DataSetLabels = new List<DataSetLabel>();
        }
        public List<string> Labels { get; set; }
        public List<DataSetLabel> DataSetLabels { get; set; }

        public class DataSetLabel
        {
            public DataSetLabel()
            {
                Data = new List<int>();
            }

            public string Label { get; set; }
            public List<int> Data { get; set; }
        }
    }
}
