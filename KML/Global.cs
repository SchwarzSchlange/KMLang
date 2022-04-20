using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KML
{
    class Global
    {

        #region RUNTIME

        public static Dictionary<string, string> runtime_variables = new Dictionary<string, string>();

        public static List<Block> runtime_blocks = new List<Block>();


        public static string TryGetVarValue(string key)
        {
            if(runtime_variables.TryGetValue(key,out string value))
            {
                return value;
            }
            else
            {
                return null;
            }

            

        }

        public static void AddBlock(Block block)
        {
            if(runtime_blocks.Find(x => x.name == block.name) != null)
            {
                runtime_blocks.Remove(runtime_blocks.Find(x => x.name == block.name));
            }
            else
            {
                runtime_blocks.Add(block);
            }
        }
     

        public static Block GetBlockByName(string name) { return runtime_blocks.Find(x => x.name == name); }

        public static void AddOrOverride(string key,string value)
        {
            if(runtime_variables.TryGetValue(key,out string x))
            {
                runtime_variables[key] = value;
            }
            else
            {
                runtime_variables.Add(key, value);
            }
        }


        public static void PrintError(string type,string cause)
        {

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            
            Console.WriteLine($"*{type.ToUpper()}* [{DateTime.Now.ToLongTimeString()}] ==> [{cause}]");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n");
        }

        #endregion


        public static bool isError = false;

        public enum TOKEN_TYPE
        {
            STRING,
            BREC_START,
            BREC_END,
            S_BREC_START,
            S_BREC_END,
            VAR_SYM,
            EQUAL,
            THEN,
            PLUS,
            MINUS,
            FACTOR,
            DIVID,
            AND,
            PARAM_START,
            PARAM_END,
            COMMA


        };


        public static void StringListDebugPrint(string[] list)
        {

            int i = 0;
            foreach(string x in list)
            {

                Console.WriteLine($"[{i}] ==> [{x}]");
                i++;
            }
        }

        public static void TokenListDebugPrint(List<Token> tokens)
        {

            foreach (Token token in tokens)
            {
                Console.WriteLine($"[{token.index}] [{token.TYPE.ToString()}] [{token.value}]");
            }
        }
        public static void StringGenericListDebugPrint(List<string> list)
        {
            int i = 0;
            foreach (string element in list)
            {
                Console.WriteLine($"{element}");
                i++;
            }
        }

    }
}
