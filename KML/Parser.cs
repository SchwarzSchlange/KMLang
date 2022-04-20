using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KML
{
    class Parser
    {


    


        public static List<Token> ParseLine(string line)
        {
            line = line.Replace("(", " ( ");
            line = line.Replace(")", " ) ");
            line = line.Replace("{", " { ");
            line = line.Replace("}", " } ");

            line = line.Replace("[", " [ ");
            line = line.Replace("]", " ] ");

            line = line.Replace("+", " + ");
            line = line.Replace("-", " - ");
            line = line.Replace("/", " / ");
            line = line.Replace("*", " * ");
            line = line.Replace("|", "|");
            line = line.Replace(",", ",");
            line = line.Replace("=", "=");
            line = line.Replace("?", "?");
            string[] s_line = line.Split(' ');



            List<Token> tokens = new List<Token>();





            for (int i = 0;i < s_line.Length;i++)
            {
                if (s_line[i] == " ")
                {
                    continue;
                }
                else if (s_line[i] == "{")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.S_BREC_START, s_line[i], i));
                }
                else if (s_line[i] == "}")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.S_BREC_END, s_line[i], i));
                }
                else if (s_line[i] == ",")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.COMMA, s_line[i], i));
                }

                else if (s_line[i] == "[")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.PARAM_START, s_line[i], i));
                }
                else if (s_line[i] == "]")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.PARAM_END, s_line[i], i));
                }

                else if (s_line[i] == "(")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.BREC_START, s_line[i], i));
                }
                else if (s_line[i] == ")")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.BREC_END, s_line[i], i));
                }
                else if(s_line[i].Contains("$"))
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.VAR_SYM, s_line[i], i));
                }
                else if(s_line[i] == "=")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.EQUAL, s_line[i], i));
                }
                else if (s_line[i] == "?")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.THEN, s_line[i], i));

                }
                else if (s_line[i] == "+")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.PLUS, s_line[i], i));
                }
                else if (s_line[i] == "-")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.MINUS, s_line[i], i));
                }
                else if (s_line[i] == "/")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.DIVID, s_line[i], i));
                }
                else if (s_line[i] == "*")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.FACTOR, s_line[i], i));
                }
                else if (s_line[i] == "|")
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.AND, s_line[i], i));
                }

                else
                {
                    tokens.Add(new Token(Global.TOKEN_TYPE.STRING, s_line[i], i));
                }
                




            }

            //Global.TokenListDebugPrint(tokens);


 



            return tokens;

        }


        public static List<Token> UpdateTokenListIndexs(List<Token> tokens,int total_removed)
        {
            for(int i = 0;i < tokens.Count;i++)
            {
                tokens[i].index = tokens[i].index - total_removed;
            }

            return tokens;
        }
     
        public static string GetParameterString(List<Token> tokens)
        {
            Token param_start = tokens.Find(x => x.TYPE == Global.TOKEN_TYPE.PARAM_START);
            Token param_end = tokens.FindLast(x => x.TYPE == Global.TOKEN_TYPE.PARAM_END);

            var myparams = tokens.GetRange(param_start.index + 1, param_end.index - 1 - param_start.index);

            string to_param = "";
            foreach (var y in myparams)
            {
                if (tokens[y.index + 1].TYPE == Global.TOKEN_TYPE.STRING)
                {
                    to_param += y.value + " ";
                }
                else
                {

                    to_param += y.value;


                }

            }
            return to_param;
        }


        public static string GetStringBetweenBreckets(List<Token> tokens)
        {
            Token first_brec_start = tokens.Find(x => x.TYPE == Global.TOKEN_TYPE.BREC_START);
            Token last_brec_end = tokens.FindLast(x => x.TYPE == Global.TOKEN_TYPE.BREC_END);



            var fetched_tokens = tokens.GetRange(first_brec_start.index + 1, last_brec_end.index - first_brec_start.index - 1);

            string to_run = "";
            foreach (var y in fetched_tokens)
            {
                if (tokens[y.index + 1].TYPE == Global.TOKEN_TYPE.STRING || tokens[y.index + 1].TYPE == Global.TOKEN_TYPE.VAR_SYM)
                {
                    to_run += y.value + " ";
                }
                else
                {

                    to_run += y.value;


                }

            }
            return to_run;
        }

        
        public static Dictionary<string,string> GetAllVariable(List<Token> tokens)
        {
            Dictionary<string, string> vals = new Dictionary<string, string>();

            for (int i = 0; i < tokens.Count; i++)
            {
                if(tokens[i].TYPE == Global.TOKEN_TYPE.VAR_SYM)
                {
                    string VAR_NAME = tokens[i].value.Replace("$", "");
                    string VAR_VALUE = "";
                    if(Global.TryGetVarValue(VAR_NAME) != null)
                    {
                        VAR_VALUE = Global.TryGetVarValue(VAR_NAME);


                        if(!vals.TryGetValue(tokens[i].value,out string _that))
                        {
                            vals.Add(tokens[i].value, VAR_VALUE);
                        }
                       
                        
                    }
                    else
                    {
                        Global.PrintError("VARIABLE", $"Can't find that variable : {VAR_NAME}");
                    }
                }

            }


            return vals;
        }



        public static string GetMathValue(List<Token> tokens)
        {
            Token first_brec_start = tokens.Find(x => x.TYPE == Global.TOKEN_TYPE.S_BREC_START);
            Token last_brec_end = tokens.FindLast(x => x.TYPE == Global.TOKEN_TYPE.S_BREC_END);



            var fetched_tokens = tokens.GetRange(first_brec_start.index + 1, last_brec_end.index - first_brec_start.index - 1);

            string to_run = "";
            foreach (var y in fetched_tokens)
            {
                if (tokens[y.index + 1].TYPE == Global.TOKEN_TYPE.STRING || tokens[y.index + 1].TYPE == Global.TOKEN_TYPE.VAR_SYM)
                {
                    to_run += y.value + " ";
                }
                else
                {

                    to_run += y.value;


                }

            }
            return to_run;
        }


        public static string StringListToString(string[] s_list)
        {
            bool first = false;
            string VAR_VALUE = "";
            foreach (string to_sag in s_list)
            {
                if (!first) { first = true; VAR_VALUE += to_sag; }
                else { VAR_VALUE += " " + to_sag; }


            }

            return VAR_VALUE;


        }

        public static List<string> GetStringValueBetween(List<Token> tokens,Global.TOKEN_TYPE start,Global.TOKEN_TYPE end)
        {

    
            

            int start_t = 0;
            int end_t = 0;

            start_t = tokens.Find(x => x.TYPE == start).index+1;
            end_t = tokens.FindLast(x => x.TYPE == end).index;

            List<string> string_tok = new List<string>();

            for(int i = start_t;i < end_t; i++)
            {
                string_tok.Add(tokens[i].value);
            }


            return string_tok;
        }


    }
}
