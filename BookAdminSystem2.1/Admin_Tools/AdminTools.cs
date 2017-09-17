using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StConlethsBookSystem_v2._1.Admin_Tools
{
    public class AdminTools
    {
        public static List<string> passwordValid(string Password)
        {
            List<string> output = new List<string>();



            List<char> list = new List<char>(new char[] { '\\', '|', ',', '.', '<', '>', '/', '?', '#', '~', ':', ';', '{', '[', '}', ']', '=', '+'
                , '_', '-', ')', '(', '*', '&', '^', '%', '$', '"', '€', '£'});

            bool result = false;

            if (Password.Any(char.IsUpper))
            {
                if (Password.Any(char.IsLower))
                {
                    if (Password.Any(char.IsDigit))
                    {
                        for (int i = 0; i < list.Count && result == false; i++)
                        {
                            char a = list[i];

                            for (int j = 0; j < Password.Length; j++)
                            {
                                if (Password.Contains(a))
                                {
                                    result = true;
                                }

                                else
                                {
                                    result = false;
                                }
                            }
                        }

                        if (result == true)
                        {
                            //Do nothing
                        }
                        else
                        {
                            output.Add("Password Error: Non-numeric or alphabetic character must be used");
                        }
                    }
                    else
                    {
                        output.Add("Password Error: Password must contain at least one number");
                    }
                }
                else
                {
                    output.Add("Password Error: Password must contain at least one lowercase letter");
                }
            }
            else
            {
                output.Add("Password Error: Password must contain at least one uppercase letter");
            }



            return output;
        }
    }
}