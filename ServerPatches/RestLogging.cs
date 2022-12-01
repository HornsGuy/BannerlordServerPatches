using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPatches
{
    public class RestLogging 
    {
        static LoggingInstance _instance;
        public static LoggingInstance Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LoggingInstance();
                }
                return _instance;
            }
        }
    }
}
