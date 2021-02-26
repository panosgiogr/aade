using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace aade
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            System.IO.File.WriteAllText(@"PATH", SOAPManual());

        }

        public String SOAPManual()
        {
            const string url = "https://www1.gsis.gr/wsaade/RgWsPublic2/RgWsPublic2";
            const string action = "POST";

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
            HttpWebRequest webRequest = CreateWebRequest(url, action);

            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            string result;
            using (WebResponse response = webRequest.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    result = rd.ReadToEnd();
                }
            }
            return result;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "application/soap+xml;charset=\"utf-8\"";
            webRequest.Accept = "application/xml";
            webRequest.Method = "POST";
            return webRequest;
        }
        /*Fiends USERNAME , PASSWORD and AFM-TO-SEARCH should be changed in xml*/
        private static XmlDocument CreateSoapEnvelope()
        {
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(@"<?xml version=""1.0""?>
<env:Envelope xmlns:ns3=""http://rgwspublic2/RgWsPublic2"" xmlns:ns2=""http://rgwspublic2/RgWsPublic2Service"" xmlns:ns1=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:env=""http://www.w3.org/2003/05/soap-envelope"">
<env:Header>
<ns1:Security>
<ns1:UsernameToken>
<ns1:Username>USERNAME</ns1:Username>
<ns1:Password>PASSWORD</ns1:Password>
</ns1:UsernameToken>
</ns1:Security>
</env:Header>
<env:Body>
<ns2:rgWsPublic2AfmMethod>
<ns2:INPUT_REC>
<ns3:afm_called_by/>
<ns3:afm_called_for>AFM-TO-SEARCH</ns3:afm_called_for>
</ns2:INPUT_REC>
</ns2:rgWsPublic2AfmMethod>
</env:Body>
</env:Envelope>");
            return soapEnvelopeXml;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

    }
}
