using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Ffsti
{
    /// <summary>
    /// Classe para auxiliar na leitura e escrita de arquivos XML
    /// </summary>
    public class XmlHelper
    {
        public static DateTime GetValorAsDateTime(string fileName, string attributeName, string descendantName = "")
        {
            return Convert.ToDateTime(GetValueAsString(fileName, attributeName, descendantName));
        }

        public static long GetValueAsLong(string fileName, string attributeName, string descendantName = "")
        {
            return Convert.ToInt64(GetValueAsString(fileName, attributeName, descendantName));
        }

        /// <summary>
        /// Retorna o valor de um atribudo de um dado arquivo XML
        /// </summary>
        /// <param name="fileName">Arquivo</param>
        /// <param name="attributeName">Nome do atributo</param>
        /// <param name="descendantName">(Opcional)Nome do nó onde o atributo está localizado</param>
        /// <returns></returns>
        public static string GetValueAsString(string fileName, string attributeName, string descendantName = "")
        {
            using (var stream = File.OpenText(fileName))
            {

                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                XmlNodeList nodes = null;


                if (descendantName == "")
                    nodes = doc.ChildNodes;
                else
                    nodes = doc.GetElementsByTagName(descendantName)[0].ChildNodes;

                foreach (XmlNode node in nodes)
                {
                    if (node.NodeType != XmlNodeType.Comment && node.NodeType != XmlNodeType.XmlDeclaration)
                    {
                        var element = (XmlElement)node;

                        if (element.Name.Equals(attributeName))
                            return element.InnerText;
                    }
                }

                return "";
            }
        }

        /// <summary>
        /// Retorna o valor de um atribudo de um dado arquivo XML
        /// </summary>
        /// <param name="stream">Arquivo</param>
        /// <param name="attributeName">Nome do atributo</param>
        /// <param name="descendantName">(Opcional)Nome do nó onde o atributo está localizado</param>
        /// <returns></returns>
        public static string GetValueAsString(Stream stream, string attributeName, string descendantName = "")
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlNodeList nodes = null;

            var result = "";

            if (descendantName == "")
                nodes = doc.ChildNodes;
            else
                nodes = doc.GetElementsByTagName(descendantName)[0].ChildNodes;

            foreach (XmlNode node in nodes)
            {
                if (node.NodeType != XmlNodeType.Comment && node.NodeType != XmlNodeType.XmlDeclaration)
                {
                    var element = (XmlElement)node;

                    if (element.Name.Equals(attributeName))
                        result = element.InnerText;
                }
            }

            doc = null;
            return result;
        }

        public static Double GetValueAsDouble(string fileName, string attributeName, string descendantName = "")
        {
            return Convert.ToDouble(GetValueAsString(fileName, attributeName, descendantName));
        }
    }
}
