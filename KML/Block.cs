using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KML
{
    class Block
    {
        public string name { get; private set; }
        public string to_run { get; set; }
        public string to_param { get; set; }


        public Block(string name,string _r,string _p)
        {
            this.name = name;
            this.to_run = _r;
            this.to_param = _p;
        }


        public int GetParamCount()
        {

            string[] einzel_param = to_param.Split(',');
            int i = 0;
            foreach (var param in einzel_param)
            {
                i++;
            }

            return i;

        }

        public string[] GetParams()
        {
            string[] einzel = to_param.Split(',');

            return einzel;
           
        }


        public void AddParametersToRuntime(string[] values)
        {
            string[] _params = this.GetParams();

            if(_params.Length != values.Length)
            {
                Global.PrintError("PARAMETER", "All parameters must be included!");
                return;
            }

            Log.Info($"|{_params[0]}|");
            Log.Info($"|{values[0]}|");

            for(int i = 0;i < _params.Length; i++)
            {
                Global.AddOrOverride(_params[i], values[i]);
            }
        }

    }
}
