using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace test.Models
{
    [DataContract]
    public class ProductJson
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Price { get; set; }
    }
}