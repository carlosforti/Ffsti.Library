using System;
using System.IO;
using System.Xml;

namespace Ffsti
{
    /// <summary>
    /// XML read/write auxiliary class
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// Get a value as a date/time
        /// </summary>
        /// <param name="fileName">The xml file name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <param name="descendantName">The descendant name</param>
        /// <returns></returns>
        public static DateTime GetValueAsDateTime(string fileName, string attributeName, string descendantName = "")
        {
            return Convert.ToDateTime(GetValueAsString(fileName, attributeName, descendantName));
        }

        /// <summary>
        /// Get a value as a long value
        /// </summary>
        /// <param name="fileName">The xml file name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <param name="descendantName">The descendant name</param>
        /// <returns></returns>
        public static long GetValueAsLong(string fileName, string attributeName, string descendantName = "")
        {
            return Convert.ToInt64(GetValueAsString(fileName, attributeName, descendantName));
        }

        /// <summary>
        /// Get a value as a string
        /// </summary>
        /// <param name="fileName">The xml file name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <param name="descendantName">The descendant name</param>
        /// <returns></returns>
        public static string GetValueAsString(string fileName, string attributeName, string descendantName = "")
        {
            var doc = new XmlDocument();
            doc.Load(fileName);

            var nodes = descendantName == ""
                ? doc.ChildNodes
                : doc.GetElementsByTagName(descendantName)[0].ChildNodes;

            foreach (XmlNode node in nodes)
            {
                if (node.NodeType == XmlNodeType.Comment || node.NodeType == XmlNodeType.XmlDeclaration)
                    continue;

                var element = (XmlElement) node;

                if (!element.Name.Equals(attributeName))
                    continue;

                return element.InnerText;
            }

            return "";
        }

        /// <summary>
        /// Get a value as a string
        /// </summary>
        /// <param name="stream">The xml file stream</param>
        /// <param name="attributeName">The attribute name</param>
        /// <param name="descendantName">The descendant name</param>
        /// <returns></returns>
        public static string GetValueAsString(Stream stream, string attributeName, string descendantName = "")
        {
            var doc = new XmlDocument();
            doc.Load(stream);

            var result = "";

            var nodes = descendantName == "" 
                ? doc.ChildNodes 
                : doc.GetElementsByTagName(descendantName)[0].ChildNodes;

            foreach (XmlNode node in nodes)
            {
                if (node.NodeType == XmlNodeType.Comment || node.NodeType == XmlNodeType.XmlDeclaration)
                    continue;

                var element = (XmlElement)node;

                if (!element.Name.Equals(attributeName))
                    continue;

                result = element.InnerText;
            }

/*
            doc = null;
*/
            return result;
        }

        /// <summary>
        /// Get a value as a double value
        /// </summary>
        /// <param name="fileName">The xml file name</param>
        /// <param name="attributeName">The attribute name</param>
        /// <param name="descendantName">The descendant name</param>
        /// <returns></returns>
        public static Double GetValueAsDouble(string fileName, string attributeName, string descendantName = "")
        {
            return Convert.ToDouble(GetValueAsString(fileName, attributeName, descendantName));
        }
    }
}
