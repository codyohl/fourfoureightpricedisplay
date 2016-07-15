using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.IO.Pipes;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace PriceDisplayWebApp.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        [HttpGet]
        public string Get()
        {
            return "get";
        }

        // GET api/values/<number>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            try
            {
                ParseAndSend($"1," + id.ToString());
                return "Success.";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string pair)
        {
            try
            {
                ParseAndSend(pair);
            }
            catch
            {
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private void ParseAndSend(string pair)
        {
            Match m = (new Regex(@"(\d+),(\d+)")).Match(pair);
            if (m.Success)
            {
                Send(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
            }
        }

        public void Send(int r, int val)
        {
            // Sends to the price display forwarder by writing to the pipe.

            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.Out);
            pipeClient.Connect();
            
            WriteString($"{r},{val}", pipeClient);
        }

        public int WriteString(string outString, Stream stream)
        {
            byte[] outBuffer = new UnicodeEncoding().GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }

            stream.WriteByte((byte)(len / 256));
            stream.WriteByte((byte)(len & 255));
            stream.Write(outBuffer, 0, len);
            stream.Flush();

            return outBuffer.Length + 2;
        }
    }
}
