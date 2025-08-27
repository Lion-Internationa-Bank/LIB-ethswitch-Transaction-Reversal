using System.Xml.Serialization;

namespace LIB_TransactionReversal.API.AwachNameCheck
{
    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(Envelope));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (Envelope)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "WebRequestCommon")]
    public class WebRequestCommon
    {

        [XmlElement(ElementName = "company")]
        public object Company { get; set; }

        [XmlElement(ElementName = "password")]
        public string Password { get; set; }

        [XmlElement(ElementName = "userName")]
        public string UserName { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public object Xmlns { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "enquiryInputCollection")]
    public class EnquiryInputCollection
    {

        [XmlElement(ElementName = "columnName")]
        public string ColumnName { get; set; }

        [XmlElement(ElementName = "criteriaValue")]
        public string CriteriaValue { get; set; }

        [XmlElement(ElementName = "operand")]
        public string Operand { get; set; }
    }

    [XmlRoot(ElementName = "ACCTType")]
    public class ACCTType
    {

        [XmlElement(ElementName = "enquiryInputCollection")]
        public EnquiryInputCollection EnquiryInputCollection { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public object Xmlns { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ACCTENQ")]
    public class ACCTENQ
    {

        [XmlElement(ElementName = "WebRequestCommon")]
        public WebRequestCommon WebRequestCommon { get; set; }

        [XmlElement(ElementName = "ACCTType")]
        public ACCTType ACCTType { get; set; }
    }

    [XmlRoot(ElementName = "Body")]
    public class Body
    {

        [XmlElement(ElementName = "ACCTENQ")]
        public ACCTENQ ACCTENQ { get; set; }
    }

    [XmlRoot(ElementName = "Envelope")]
    public class Envelope
    {

        [XmlElement(ElementName = "Header")]
        public object Header { get; set; }

        [XmlElement(ElementName = "Body")]
        public Body Body { get; set; }

        [XmlAttribute(AttributeName = "soapenv")]
        public string Soapenv { get; set; }

        [XmlAttribute(AttributeName = "awac")]
        public string Awac { get; set; }

        [XmlText]
        public string Text { get; set; }
    }


}
