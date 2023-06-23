using System.Collections.Generic;

using ETHTPS.Data.Core.Models.DataPoints.XYPoints;

namespace ETHTPS.Data.Core.Models.ResponseModels.L2s
{
    public abstract class DatasetAnalysisBase
    {
        protected readonly IEnumerable<IXYMultiConvertible> _dataset;
        protected DatasetAnalysisBase() { }
        protected DatasetAnalysisBase(IEnumerable<IXYMultiConvertible> dataset)
        {
            _dataset = dataset;
        }
    }
}
