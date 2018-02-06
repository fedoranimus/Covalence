using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Covalence
{   
    [XmlRoot("ZipCodes")]
    public class ZipCodes {
        [XmlElement("ZipCode")]
        public List<ZipCode> Codes { get; set; }
    }

    public class ZipCode {
        [XmlElement("Code")]
        public string Code { get; set; }
        [XmlElement("Latitude")]
        public double Latitude { get; set; }
        [XmlElement("Longitude")]
        public double Longitude { get; set; }
    }
}