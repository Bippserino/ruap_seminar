using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ruap_seminar
{
    public static class Utilities
    {

        public static int[] GetFormatedArray(int[] inputArray)
        {
            int[] rowSections = new int[256];

            for (int rowSection = 0; rowSection < 256; rowSection++)
            {
                for (int section = 0; section < 4; section++)
                {
                    rowSections[rowSection] += inputArray[4 * rowSection + section];
                }
            }

            int[] result = new int[64];
            int offset = 0;

            for (int columnSection = 0; columnSection < 64; columnSection++)
            {
                if (columnSection != 0 && columnSection % 8 == 0)
                {
                    offset++;
                }
                result[columnSection] = rowSections[24 * offset + columnSection] + rowSections[24 * offset + columnSection + 8]
                    + rowSections[24 * offset + columnSection + 16] + rowSections[24 * offset + columnSection + 16];
            }

            return result;
        }

        public static async void makeAPIrequest(int[] resultArray, GridForm form)
        {
            string[] resultStringArray = resultArray.Select(x => x.ToString()).ToArray();
            Console.WriteLine(String.Join(",", resultStringArray));
            Array.Resize<string>(ref resultStringArray, 65);

            var client = new HttpClient();
            var scoreRequest = new
            {
                Inputs = new Dictionary<string, StringTable>() {
                        {
                            "input1",
                            new StringTable()
                            {
                                ColumnNames = new string[] {"Col1", "Col2", "Col3", "Col4", "Col5", "Col6", "Col7", "Col8", "Col9", "Col10", "Col11", "Col12", "Col13", "Col14", "Col15", "Col16", "Col17", "Col18", "Col19", "Col20", "Col21", "Col22", "Col23", "Col24", "Col25", "Col26", "Col27", "Col28", "Col29", "Col30", "Col31", "Col32", "Col33", "Col34", "Col35", "Col36", "Col37", "Col38", "Col39", "Col40", "Col41", "Col42", "Col43", "Col44", "Col45", "Col46", "Col47", "Col48", "Col49", "Col50", "Col51", "Col52", "Col53", "Col54", "Col55", "Col56", "Col57", "Col58", "Col59", "Col60", "Col61", "Col62", "Col63", "Col64", "Col65"},
                                Values = new string[2, 65] { { resultStringArray[0], resultStringArray[1], resultStringArray[2], resultStringArray[3], resultStringArray[4], resultStringArray[5], resultStringArray[6], resultStringArray[7], resultStringArray[8], resultStringArray[9], resultStringArray[10], resultStringArray[11], resultStringArray[12], resultStringArray[13], resultStringArray[14], resultStringArray[15], resultStringArray[16], resultStringArray[17], resultStringArray[18], resultStringArray[19], resultStringArray[20], resultStringArray[21], resultStringArray[22], resultStringArray[23], resultStringArray[24], resultStringArray[25], resultStringArray[26], resultStringArray[27], resultStringArray[28], resultStringArray[29], resultStringArray[30], resultStringArray[31], resultStringArray[32], resultStringArray[33], resultStringArray[34], resultStringArray[35], resultStringArray[36], resultStringArray[37], resultStringArray[38], resultStringArray[39], resultStringArray[40], resultStringArray[41], resultStringArray[42], resultStringArray[43], resultStringArray[44], resultStringArray[45], resultStringArray[46], resultStringArray[47], resultStringArray[48], resultStringArray[49], resultStringArray[50], resultStringArray[51], resultStringArray[52], resultStringArray[53], resultStringArray[54], resultStringArray[55], resultStringArray[56], resultStringArray[57], resultStringArray[58], resultStringArray[59], resultStringArray[60], resultStringArray[61], resultStringArray[62], resultStringArray[63], resultStringArray[64] },  { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" },  }
                            }
                        },
                    },
                GlobalParameters = new Dictionary<string, string>()
                {
                }
            };
            const string apiKey = "VPan2/IRDgE5+ObQoZEVQ0NCpfXw/t+GCG8pHFq7YjO9TywybZOy1Ntv8iAATYWv3Ewju3FxOpha+AMCgNDCOw==";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/6a97490368f5494b86837f59ca7c678c/services/45dfda9aa8064ce5a8510bb94437742e/execute?api-version=2.0&details=true");

            HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("SUCCESS");
                string result = await response.Content.ReadAsStringAsync();
                dynamic jsonResult = JsonConvert.DeserializeObject(result);               

                for (int i = 0; i < 11; i++)
                {
                    string value = (jsonResult["Results"]["output1"]["value"]["Values"][0])[65 + i];
                    string label = jsonResult["Results"]["output1"]["value"]["ColumnNames"][65 + i];
                    if (i != 10)
                    {
                        value = Math.Round((float.Parse(value) * 100), 2).ToString();
                        form.labels[i].Text = label + ": "
                         + value + "%";
                        form.labels[i].ForeColor = Color.Black;
                    }
                    else
                    {
                        form.labels[i].Text = label + ": "
                         + value;
                        form.labels[Int32.Parse(value)].ForeColor = Color.Red;
                    }
                 }
                
            }
            else
            {
                Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));
                Console.WriteLine(response.Headers.ToString());
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
            }
        }

    }
}
