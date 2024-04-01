using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Email
{
    public class Attachment
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}
