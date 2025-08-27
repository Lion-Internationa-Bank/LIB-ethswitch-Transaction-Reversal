using System.Xml.Serialization;
using System;

namespace LIB_TransactionReversal.API.AwachTestModels
{
    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(Document));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (Document)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "CdOrPrtry")]
    public class CdOrPrtry
    {

        [XmlElement(ElementName = "Cd")]
        public string Cd { get; set; }
    }

    [XmlRoot(ElementName = "Tp")]
    public class Tp
    {

        [XmlElement(ElementName = "CdOrPrtry")]
        public CdOrPrtry CdOrPrtry { get; set; }
    }

    [XmlRoot(ElementName = "DltPrvtDataDtl")]
    public class DltPrvtDataDtl
    {

        [XmlElement(ElementName = "PrvtDtInf")]
        public string PrvtDtInf { get; set; }

        [XmlElement(ElementName = "Tp")]
        public Tp Tp { get; set; }
    }

    [XmlRoot(ElementName = "DltPrvtData")]
    public class DltPrvtData
    {

        [XmlElement(ElementName = "FlwInd")]
        public string FlwInd { get; set; }

        [XmlElement(ElementName = "DltPrvtDataDtl")]
        public DltPrvtDataDtl DltPrvtDataDtl { get; set; }

        [XmlElement(ElementName = "OrdrPrties")]
        public OrdrPrties OrdrPrties { get; set; }
    }

    [XmlRoot(ElementName = "GrpHdr")]
    public class GrpHdr
    {

        [XmlElement(ElementName = "MsgId")]
        public string MsgId { get; set; }

        [XmlElement(ElementName = "CreDtTm")]
        public DateTime CreDtTm { get; set; }

        [XmlElement(ElementName = "NbOfTxs")]
        public int NbOfTxs { get; set; }

        [XmlElement(ElementName = "CtrlSum")]
        public int CtrlSum { get; set; }

        [XmlElement(ElementName = "InitgPty")]
        public object InitgPty { get; set; }

        [XmlElement(ElementName = "DltPrvtData")]
        public DltPrvtData DltPrvtData { get; set; }
    }

    [XmlRoot(ElementName = "OrdrPrties")]
    public class OrdrPrties
    {

        [XmlElement(ElementName = "Tp")]
        public string Tp { get; set; }

        [XmlElement(ElementName = "Md")]
        public string Md { get; set; }
    }

    [XmlRoot(ElementName = "SvcLvl")]
    public class SvcLvl
    {

        [XmlElement(ElementName = "Prtry")]
        public string Prtry { get; set; }
    }

    [XmlRoot(ElementName = "PmtTpInf")]
    public class PmtTpInf
    {

        [XmlElement(ElementName = "InstrPrty")]
        public string InstrPrty { get; set; }

        [XmlElement(ElementName = "SvcLvl")]
        public SvcLvl SvcLvl { get; set; }
    }

    [XmlRoot(ElementName = "SchmeNm")]
    public class SchmeNm
    {

        [XmlElement(ElementName = "Prtry")]
        public string Prtry { get; set; }
    }

    [XmlRoot(ElementName = "Othr")]
    public class Othr
    {

        [XmlElement(ElementName = "Id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "SchmeNm")]
        public SchmeNm SchmeNm { get; set; }
    }

    [XmlRoot(ElementName = "Id")]
    public class Id
    {

        [XmlElement(ElementName = "Othr")]
        public Othr Othr { get; set; }
    }

    [XmlRoot(ElementName = "DbtrAcct")]
    public class DbtrAcct
    {

        [XmlElement(ElementName = "Id")]
        public Id Id { get; set; }

        [XmlElement(ElementName = "Ccy")]
        public string Ccy { get; set; }
    }

    [XmlRoot(ElementName = "FinInstnId")]
    public class FinInstnId
    {

        [XmlElement(ElementName = "Nm")]
        public string Nm { get; set; }

        [XmlElement(ElementName = "Othr")]
        public Othr Othr { get; set; }
    }

    [XmlRoot(ElementName = "BrnchId")]
    public class BrnchId
    {

        [XmlElement(ElementName = "Id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "Nm")]
        public string Nm { get; set; }
    }

    [XmlRoot(ElementName = "DbtrAgt")]
    public class DbtrAgt
    {

        [XmlElement(ElementName = "FinInstnId")]
        public FinInstnId FinInstnId { get; set; }

        [XmlElement(ElementName = "BrnchId")]
        public BrnchId BrnchId { get; set; }
    }

    [XmlRoot(ElementName = "PmtId")]
    public class PmtId
    {

        [XmlElement(ElementName = "InstrId")]
        public string InstrId { get; set; }

        [XmlElement(ElementName = "EndToEndId")]
        public string EndToEndId { get; set; }
    }

    [XmlRoot(ElementName = "InstdAmt")]
    public class InstdAmt
    {

        [XmlAttribute(AttributeName = "Ccy")]
        public string Ccy { get; set; }

        [XmlText]
        public int Text { get; set; }
    }

    [XmlRoot(ElementName = "Amt")]
    public class Amt
    {

        [XmlElement(ElementName = "InstdAmt")]
        public InstdAmt InstdAmt { get; set; }
    }

    [XmlRoot(ElementName = "CdtrAgt")]
    public class CdtrAgt
    {

        [XmlElement(ElementName = "FinInstnId")]
        public FinInstnId FinInstnId { get; set; }

        [XmlElement(ElementName = "BrnchId")]
        public BrnchId BrnchId { get; set; }
    }

    [XmlRoot(ElementName = "CdtrAcct")]
    public class CdtrAcct
    {

        [XmlElement(ElementName = "Id")]
        public Id Id { get; set; }

        [XmlElement(ElementName = "Ccy")]
        public string Ccy { get; set; }
    }

    [XmlRoot(ElementName = "RmtInf")]
    public class RmtInf
    {

        [XmlElement(ElementName = "Ustrd")]
        public string Ustrd { get; set; }
    }

    [XmlRoot(ElementName = "CdtTrfTxInf")]
    public class CdtTrfTxInf
    {

        [XmlElement(ElementName = "PmtId")]
        public PmtId PmtId { get; set; }

        [XmlElement(ElementName = "Amt")]
        public Amt Amt { get; set; }

        [XmlElement(ElementName = "CdtrAgt")]
        public CdtrAgt CdtrAgt { get; set; }

        [XmlElement(ElementName = "Cdtr")]
        public object Cdtr { get; set; }

        [XmlElement(ElementName = "CdtrAcct")]
        public CdtrAcct CdtrAcct { get; set; }

        [XmlElement(ElementName = "RmtInf")]
        public RmtInf RmtInf { get; set; }
    }

    [XmlRoot(ElementName = "PmtInf")]
    public class PmtInf
    {

        [XmlElement(ElementName = "PmtInfId")]
        public string PmtInfId { get; set; }

        [XmlElement(ElementName = "PmtMtd")]
        public string PmtMtd { get; set; }

        [XmlElement(ElementName = "BtchBookg")]
        public int BtchBookg { get; set; }

        [XmlElement(ElementName = "NbOfTxs")]
        public int NbOfTxs { get; set; }

        [XmlElement(ElementName = "CtrlSum")]
        public int CtrlSum { get; set; }

        [XmlElement(ElementName = "DltPrvtData")]
        public DltPrvtData DltPrvtData { get; set; }

        [XmlElement(ElementName = "PmtTpInf")]
        public PmtTpInf PmtTpInf { get; set; }

        [XmlElement(ElementName = "ReqdExctnDt")]
        public DateTime ReqdExctnDt { get; set; }

        [XmlElement(ElementName = "Dbtr")]
        public object Dbtr { get; set; }

        [XmlElement(ElementName = "DbtrAcct")]
        public DbtrAcct DbtrAcct { get; set; }

        [XmlElement(ElementName = "DbtrAgt")]
        public DbtrAgt DbtrAgt { get; set; }

        [XmlElement(ElementName = "CdtTrfTxInf")]
        public CdtTrfTxInf CdtTrfTxInf { get; set; }
    }

    [XmlRoot(ElementName = "CstmrCdtTrfInitn")]
    public class CstmrCdtTrfInitn
    {

        [XmlElement(ElementName = "GrpHdr")]
        public GrpHdr GrpHdr { get; set; }

        [XmlElement(ElementName = "PmtInf")]
        public PmtInf PmtInf { get; set; }
    }

    [XmlRoot(ElementName = "Document") ]
    public class Document
    {

        [XmlElement(ElementName = "CstmrCdtTrfInitn")]
        public CstmrCdtTrfInitn CstmrCdtTrfInitn { get; set; }

        [XmlAttribute(AttributeName = "xsi")]
        public string Xsi { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlText]
        public string Text { get; set; }

        public Document()
        {
            Xsi = "http://www.w3.org/2001/XMLSchema-instance";
            Xmlns = "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03DB";
        }
    }
}
