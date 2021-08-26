using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TEST_DEMO
{
    [Route("api/Test")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public String TEST()
        {
            return "1. /api/Test/stringDecoder \n2. /api/Test/phone ";
        }

        [HttpPost]
        [Route("stringDecoder")]
        public String TEST1(string param)
        {
            try
            {
                
                String arr = param.ToUpper().Replace("WUB", " ");
                arr=arr.Trim();
                arr = Regex.Replace(arr, " {2,}", " ");
                return arr;
            }
            catch (Exception err)
            {
                return err.Message;
            }
           
        }

        [HttpPost]
        [Route("phone")]
        public List<ResultEntity> TEST2()
        {
            string dr = @"+1-541-754-3010 156 Alphand_St. <J Steeve>\n 133, Green, Rd. <E Kustur> NY-56423 ;+1-541-914-3010\n
            +1-541-984-3012 <P Reed> /PO Box 530; Pollocksville, NC-28573\n :+1-321-512-2222 <Paul Dive> Sequoia Alley PQ-67209\n
            +1-741-984-3090 <Peter Reedgrave> _Chicago\n :+1-921-333-2222 <Anna Stevens> Haramburu_Street AA-67209\n
            +1-111-544-8973 <Peter Pan> LA\n +1-921-512-2222 <Wilfrid Stevens> Wild Street AA-67209\n
            <Peter Gone> LA ?+1-121-544-8974 \n <R Steell> Quora Street AB-47209 +1-481-512-2222\n
            <Arthur Clarke> San Antonio $+1-121-504-8974 TT-45120\n <Ray Chandler> Teliman Pk. !+1-681-512-2222! AB-47209,\n
            <Sophia Loren> +1-421-674-8974 Bern TP-46017\n <Peter O'Brien> High Street +1-908-512-2222; CC-47209\n
            <Anastasia> +48-421-674-8974 Via Quirinal Roma\n <P Salinger> Main Street, +1-098-512-2222, Denver\n
            <C Powel> *+19-421-674-8974 Chateau des Fosses Strasbourg F-68000\n <Bernard Deltheil> +1-498-512-2222; Mount Av.Eldorado\n
            +1-099-500-8000 <Peter Crush> Labrador Bd.\n +1-931-512-4855 <William Saurin> Bison Street CQ-23071\n
            <P Salinge> Main Street, +1-098-512-2222, Denve\n ";
            List<ResultEntity> result = new List<ResultEntity>();
            try
            {
                dr = dr.Replace("\r", "");
                dr = dr.Replace("\n", "");
                string[] arr = dr.Split("\\n");
                string p1 = @"([+])([0-9]+)-([0-9]+)-([0-9]+)-([0-9]+)"; // @"([A-Za-z0-9\-]+)+([A-Za-z0-9\-]+)";
                string p2 = @"([<])([A-Za-z\s']+)([>])";
                string tempStr = "";
                foreach (var item in arr)
                {
                    ResultEntity Temp = new ResultEntity();
                    tempStr = "";

                    Temp.Phone = Regex.Match(item, p1, RegexOptions.IgnoreCase).Value.ToString();
                    Temp.Name = Regex.Match(item, p2,RegexOptions.IgnoreCase).Value.ToString();


                    Regex replace_1 = new Regex(p1);
                    Regex replace_2 = new Regex(p2);
                    tempStr = replace_1.Replace(item, "");
                    tempStr = replace_2.Replace(tempStr, "");
                    Temp.Address = Regex.Replace(tempStr, "([?:;%#@!])", "").Trim();

                    result.Add(Temp);

                }

                foreach ( var c in result.GroupBy(a => a.Phone).Select(g => new { g.Key, Count = g.Count() }) )
                {
                    if (c.Count > 1)
                    {
                        result.Where(h=>h.Phone==c.Key).Select(s => { s.check_duplicate = "To many people : " + s.Phone; return s; }).ToList();
                    }
                }

                return result;
            }
            catch (Exception err)
            {
                return result;
            }

        }

    }
}
