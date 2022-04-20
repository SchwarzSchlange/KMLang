using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KML
{
    class Token
    {
        public Global.TOKEN_TYPE TYPE;
        public string value;
        public int index;

        public Token(Global.TOKEN_TYPE _type,string _value,int _index)
        {
            this.TYPE = _type;
            this.value = _value;
            this.index = _index;
        }

    }
}
