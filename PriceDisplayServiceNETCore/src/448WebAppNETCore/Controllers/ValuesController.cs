using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        public class Pair
            {
            public int Row { get; set; }
            public int Val { get; set; }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Pair value)
        {
            try
            {
                Send(value.Row, value.Val);
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
