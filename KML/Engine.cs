using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using org.mariuszgromada.math.mxparser;

namespace KML
{
    class Engine
    {
        public static void Run(List<Token> tokens)
        {
            for(int i = 0; i < tokens.Count;i++)
            {
                if(i == 0)
                {
                    if (tokens[i].value == "out")
                    {
                        //out (WRITE_VALUE)
                        //WRITE_VALUE ==> {VARIABLE_IN_VALUE}




                        List<string> s_list = Parser.GetStringValueBetween(tokens, Global.TOKEN_TYPE.BREC_START, Global.TOKEN_TYPE.BREC_END);


                        Dictionary<string, string> vals = Parser.GetAllVariable(tokens);



                        if (s_list.Contains("{") && s_list.Contains("}"))
                        {
                            int sbs = tokens.Find(x => x.TYPE == Global.TOKEN_TYPE.S_BREC_START).index - 2;
                            int sbe = tokens.Find(x => x.TYPE == Global.TOKEN_TYPE.S_BREC_END).index;

                            string math = Parser.GetMathValue(tokens);


                            string[] exp_math = math.Split(' ');
                            for (int k = 0; k < exp_math.Length; k++)
                            {
                                if (vals.TryGetValue(exp_math[k], out string _value))
                                {
                                    //Log.Info($"{exp_math[k]} == {_value}");
                                    exp_math[k] = _value;
                                }
                              


                            }

                            math = Parser.StringListToString(exp_math);

                            //Log.Success(math);

                            Expression exp = new Expression(math);
                            int index_boku = s_list.Count - (sbe - sbs) - 1;
                            s_list.RemoveRange(sbs, sbe - sbs);

                            double to_add = exp.calculate();


                            s_list.Insert(index_boku, to_add.ToString().Trim());
                            
                        }








                        for (int k = 0; k < s_list.Count; k++)
                        {
                            if (vals.TryGetValue(s_list[k], out string _value))
                            {

                                Console.Write($"{_value} ");
                            }
                            else
                            {
                                Console.Write($"{s_list[k]} ");
                            }


                        }
                        Console.WriteLine("\n");

                    }
                    else if(tokens[0].value == "clear")
                    {
                        Console.Clear();
                    }
                    else if (tokens[0].value == "push")
                    {
                        //push VAR_NAME(VAR_VALUE)
                        string VAR_NAME = "";
                        try
                        {
                            VAR_NAME = tokens[i + 1].value;
                        }
                        catch
                        {
                            Global.PrintError("Synxtax", "VAR_NAME is missing.");
                            return;
                        }






                        List<string> s_list = Parser.GetStringValueBetween(tokens, Global.TOKEN_TYPE.BREC_START, Global.TOKEN_TYPE.BREC_END);

                        if (s_list.Count == 0)
                        {
                            Global.PrintError("Synxtax", "VAR_VALUE is missing.");
                            return;
                        }

                        bool first = false;
                        string VAR_VALUE = "";
                        foreach (string to_sag in s_list)
                        {
                            if (!first) { first = true; VAR_VALUE += to_sag; }
                            else { VAR_VALUE += " " + to_sag; }


                        }


                        Global.AddOrOverride(VAR_NAME, VAR_VALUE);


                    }
                    else if(tokens[0].value == "block")
                    {
                        //block BLOCK_NAME (BLOCK_TO_DO1 | BLOCK_TO_DO2 | ...)[param]
                        //Z.B ==>block in_name_out_name(in (name) | out ($name))[param = 20]


                        string BLOCK_NAME = tokens[1].value;

                        if(BLOCK_NAME == "" || BLOCK_NAME == null)
                        {
                            Global.PrintError("Synxtax", "BLOCK_NAME not assigned!");
                            return;
                        }

                      
                        string to_param = Parser.GetParameterString(tokens);
                        Console.WriteLine("to_param ==>" + to_param);

                        string to_run = Parser.GetStringBetweenBreckets(tokens);
                        Console.WriteLine("to_run ==>" + to_run);


                        Global.AddBlock(new Block(BLOCK_NAME,to_run,to_param));
                      

                        


                    }
                    else if(tokens[0].value == "call")
                    {
                        //call BLOCK_NAME[]

                        //block write_line_input(in x | out ($x))

                        string BLOCK_NAME = tokens[1].value;

                        if (BLOCK_NAME == "" || BLOCK_NAME == null)
                        {
                            Global.PrintError("Synxtax", "BLOCK_NAME not assigned!");
                            return;
                        }


                        if(Global.runtime_blocks.Find(x => x.name == BLOCK_NAME) == null)
                        {
                            Global.PrintError("RUNTIME", "There is not a block named " + BLOCK_NAME);
                            return;
                        }


                        Block this_block = Global.runtime_blocks.Find(x => x.name == BLOCK_NAME);


                        string[] splited_to_run = this_block.to_run.Split('|');


                        string[] values_param = Parser.GetParameterString(tokens).Split(',');




                        this_block.AddParametersToRuntime(values_param);
                        foreach (string run in splited_to_run)
                        {
                            
                            Console.WriteLine("run ==> " + run.TrimStart(new char[' ']));
                            List<Token> run_tokens = Parser.ParseLine(run.TrimStart(new char[' ']));

                            Engine.Run(run_tokens);
                        }





                    }
                    else if(tokens[0].value == "end")
                    {
                        Environment.Exit(0);
                    }
                    else if(tokens[0].value == "wait")
                    {

                        List<string> s_list_time = Parser.GetStringValueBetween(tokens, Global.TOKEN_TYPE.S_BREC_START, Global.TOKEN_TYPE.S_BREC_END);


                        int total = 0;

                     

                        try
                        {
                            for (int p = 0; p < s_list_time.Count; p++)
                            {
                            
                                total += int.Parse(s_list_time[p]);
                            }

                            Thread.Sleep(total);
                        }
                        catch
                        {
                            Global.PrintError("WAIT", "Enter a valid value.");
                            return;
                        }
                    
                    }
                    else if(tokens[0].value == "in")
                    {
                        //in (VARIABLE NAME)
                        //==> out == VALUE
                        string VALUE = Console.ReadLine();

                        string VARIABLE_NAME = tokens[1].value;


                        Global.AddOrOverride(VARIABLE_NAME, VALUE);


                    }
                    else if (tokens[0].value == "if")
                    {
                        //if ($VARIABLE ODER ANY) = ($VARIABLE ODER ANY) ? (RUN IF THE CONDITION IS TRUE)

                        Dictionary<string, string> vals = Parser.GetAllVariable(tokens);



                        string erste_val = "";
                        string zweite_val = "";

                        int p = 0;
                        int equal_index = 0;
                        while(p < tokens.Count)
                        {
                            if (tokens[p].TYPE == Global.TOKEN_TYPE.EQUAL)
                            {
                                equal_index = tokens[p].index;
                                break;
                            }
                            p++;
                        }

                        if(equal_index == 0)
                        {
                            Global.PrintError("Error", "'=' occured an error.");
                            return;
                        }

                        if (vals.TryGetValue(tokens[equal_index-1].value, out string _valueeins))
                        {
                            erste_val = _valueeins;
                        }
                        else
                        {
                            int erste_val_brec_index_start = 0;
                            int erste_val_brec_index_end = 0;

                            for(int e = 0; e < tokens[equal_index-1].index; e++)
                            {
                                if(tokens[e].TYPE == Global.TOKEN_TYPE.BREC_START)
                                {
                                    erste_val_brec_index_start = tokens[e].index;
                                }
                                else if(tokens[e].TYPE == Global.TOKEN_TYPE.BREC_END)
                                {
                                    erste_val_brec_index_end = tokens[e].index;
                                }


                            }

                            if(erste_val_brec_index_end == 0 || erste_val_brec_index_start == 0)
                            {
                                Global.PrintError("Error", "An error occured with '(' or ')' before '='.");

                            }


                            bool isFirst = false;
                            for(int z = erste_val_brec_index_start+1; z < erste_val_brec_index_end; z++)
                            {
                                if (!isFirst) { isFirst = true; erste_val += tokens[z].value; }
                                else { erste_val += $" {tokens[z].value}"; }
                                
                            }

                            Console.WriteLine($"index_1_=_before {erste_val_brec_index_start}");
                            Console.WriteLine($"index_2_=_before {erste_val_brec_index_end}");

                        }

                        if (vals.TryGetValue(tokens[equal_index+1].value, out string _valuezwei))
                        {
                            zweite_val = _valuezwei;
                        }
                        else
                        {
                            int zweite_val_brec_index_start = 0;
                            int zweite_val_brec_index_end = 0;


                            Token then_token = tokens.Find(x => x.TYPE == Global.TOKEN_TYPE.THEN);
                               
                            if(then_token == null)
                            {
                                Global.PrintError("SYNXTAX", "'?' is missing.");
                                return;
                            }
                           

                            for (int e = equal_index+1; e < then_token.index; e++)
                            {
                                if (tokens[e].TYPE == Global.TOKEN_TYPE.BREC_START)
                                {
                                    zweite_val_brec_index_start = tokens[e].index;
                                }
                                else if (tokens[e].TYPE == Global.TOKEN_TYPE.BREC_END)
                                {
                                    zweite_val_brec_index_end = tokens[e].index;
                                }


                            }

                            if (zweite_val_brec_index_end == 0 || zweite_val_brec_index_start == 0)
                            {
                                Global.PrintError("Error", "An error occured with '(' or ')' after '='.");

                            }

                            bool isFirst = false;
                            for (int z = zweite_val_brec_index_start + 1; z < zweite_val_brec_index_end; z++)
                            {
                                if (!isFirst) { isFirst = true; zweite_val += tokens[z].value; }
                                else { zweite_val += $" {tokens[z].value}"; }
                             
                            }

                            Console.WriteLine($"index_1_=_after {zweite_val_brec_index_start}");
                            Console.WriteLine($"index_2_=_after {zweite_val_brec_index_end}");
                        }

                        Console.WriteLine("index:"+equal_index);
                        Console.WriteLine($"[{erste_val}]");
                        Console.WriteLine($"[{zweite_val}]");

                        Console.WriteLine("\n");

                        int k = 0;
                        int then_index = 0;
                        while (k < tokens.Count)
                        {
                            if (tokens[p].TYPE == Global.TOKEN_TYPE.THEN)
                            {
                                then_index = tokens[p].index;
                                break;
                            }
                            p++;
                        }

                        if(then_index == 0)
                        {
                            Global.PrintError("Error", "'?' occured an error.");
                            return;
                        }

                        List<Token> to_run_tokens = new List<Token>();
                        int normalized_index = 0;
                        for (int y = then_index+1;y < tokens.Count;y++)
                        {
                            to_run_tokens.Add(new Token(tokens[y].TYPE, tokens[y].value,normalized_index)) ;
                            normalized_index++;
                        }

                        

                        if (erste_val == zweite_val)
                        {
                            Console.WriteLine("TRUE");
                            //Global.TokenListDebugPrint(to_run_tokens);
                            Engine.Run(to_run_tokens);
                        }
                        else
                        {
                            Console.WriteLine("FALSE");
                        }




                    }
                }
               
            }

        }



    }
}
