using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Ffsti.Library
{
    /// <summary>
    /// Classe para auxiliar na leitura e escrita de arquivos XML
    /// </summary>
    public class XmlHelper
    {
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

                //var document = XDocument.Load(stream).Root;

                //IEnumerable<XElement> nodes = null;

                //if (descendantName == "")
                //    nodes = document.DescendantsAndSelf();
                //else
                //    nodes = document.DescendantsAndSelf(descendantName);

                //if (nodes != null)
                //    try
                //    {
                //        return (nodes.DescendantsAndSelf().Where(n => n.Name.LocalName == attributeName).FirstOrDefault().Value);
                //    }
                //    catch
                //    {
                //        return "";
                //    }

                //return "";
            }
        }
    }
}
