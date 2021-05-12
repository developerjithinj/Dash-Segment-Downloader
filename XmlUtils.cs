using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Dash_Downloader
{
    class XmlUtils
    {

        public static string getAttributeValue(string key, XmlNode node)
        {
            if (key == null || node == null)
            {
                return "";
            }
            XmlNode xmlNode = node.Attributes.GetNamedItem(key);
            if (xmlNode != null)
            {
                return xmlNode.Value;
            }

            return "";
        }

        public static double getTime(String ptStr, String timeunit)
        {
            try
            {
                ptStr = ptStr.ToUpper();
                timeunit = timeunit.ToUpper();
                char[] ptStrArr = ptStr.Split(timeunit)[0].ToCharArray();
                String numStr = "";
                for (int i = ptStrArr.Length - 1; i >= 0; i--)
                {
                    if (Char.IsLetter(ptStrArr[i]))
                    {
                        break;
                    }
                    numStr += ptStrArr[i];
                }
                numStr = new string(numStr.Reverse().ToArray());
                return Double.Parse(numStr);
            }
            catch (Exception e) { }
            return 0;
        }
        public static XmlNode getNode(String name, XmlNode parent)
        {
            if (parent == null || name == null)
                return null;
            foreach (XmlNode node in parent.ChildNodes)
                if (name == node.Name)
                    return node;
            return null;
        }
    }
}
