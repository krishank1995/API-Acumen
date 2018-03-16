using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallTracer.DataProviders
{
    interface ITraceRepository
    {
        void SaveTrace();

        void RequestAllTraces();

        void RequestSpecificTrace(int id);

        

    }
}
