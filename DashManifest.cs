using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace Dash_Downloader
{
    public class DashManifest
    {
        public double mediaDuration;
        public string path;
        public ArrayList tracks = new ArrayList();
        public class Track
        {
            public string id;
            public string type;
            public long bandwidth;
            public int height;
            public int width;
            public float frameRate;
            public string language;
            public int segmentStartIndex;
            public int segmentCount;
            public string initSegmentTemplate;
            public string mediaSegmentTemplate;
            public bool selected;

            override public string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Id: ").AppendLine(id)
               .Append("Type: ").AppendLine(type)
               .Append("Bandwidth: ").AppendLine(bandwidth + "")
               .Append("Height: ").AppendLine(height + "")
               .Append("Width: ").AppendLine(width + "")
               .Append("Frame Rate: ").AppendLine(frameRate + "")
               .Append("Language: ").AppendLine(language)
               .Append("Segment Start Index: ").AppendLine(segmentStartIndex + "")
               .Append("Segment Count: ").AppendLine(segmentCount + "")
               .Append("InitSegment Template: ").AppendLine(initSegmentTemplate)
               .Append("Media Segment Template: ").AppendLine(mediaSegmentTemplate)
               .Append("User Selected: ").AppendLine(selected+"");
               
                return sb.ToString();
            }
        }
        override public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Path: ").AppendLine(path)
                .Append("Duration: ").AppendLine(mediaDuration + "")
                .AppendLine("Tracks:").AppendLine();

            foreach (Track track in tracks)
            {
                sb.Append(track.ToString()).AppendLine();
            }

            return sb.ToString();
        }

        public static DashManifest parseManifest(string fileUrl)
        {
            DashManifest returnManifest = new DashManifest();

            XmlDocument doc = new XmlDocument();
            //Load xml file
            doc.Load(fileUrl);

            //Get high level nodes
            XmlNode mpdNode = doc.GetElementsByTagName("MPD")[0];
            XmlNode periodNode = XmlUtils.getNode("Period", mpdNode);
            XmlNode baseUrlNode = XmlUtils.getNode("BaseURL", periodNode);

            //Compute total media durationin seconds
            String mediaPresentationDuration = XmlUtils.getAttributeValue("mediaPresentationDuration", mpdNode); //eg: PT1H30M30.042666S
            double hour = XmlUtils.getTime(mediaPresentationDuration, "H");
            double minute = XmlUtils.getTime(mediaPresentationDuration, "M");
            double seconds = XmlUtils.getTime(mediaPresentationDuration, "S");
            returnManifest.mediaDuration = hour * 3600 + minute * 60 + seconds;

            //Path after baseUrl
            if (baseUrlNode == null || baseUrlNode.InnerText == null)
            {
                returnManifest.path = "";
            }
            else
            {
                returnManifest.path = baseUrlNode.InnerText;
            }

            //Loop through different adaptation set to get track information
            foreach (XmlNode adaptationNode in periodNode.ChildNodes)
            {
                if ("AdaptationSet" == adaptationNode.Name)
                {
                    XmlNode adaptationSet = adaptationNode;
                    string contentType = XmlUtils.getAttributeValue("contentType", adaptationSet);

                    XmlNode segmentTemplate = XmlUtils.getNode("SegmentTemplate", adaptationSet);

                    //Segment count and startIndex calculation
                    string timescaleStr = XmlUtils.getAttributeValue("timescale", segmentTemplate);
                    string durationStr = XmlUtils.getAttributeValue("duration", segmentTemplate);
                    long timescale = 0;
                    long duration = 0;
                    long.TryParse(timescaleStr, out timescale);
                    long.TryParse(durationStr, out duration);
                    if (timescale == 0)
                        timescale = 1;
                    long segmentDuration = duration / timescale;
                    int segmentCount = (int)(returnManifest.mediaDuration / segmentDuration);
                    string startIndex = XmlUtils.getAttributeValue("startWithSAP", adaptationSet);

                    //We won't support the mpd file
                    if (segmentCount <= 0)
                    {
                        return null;
                    }

                    string lang = XmlUtils.getAttributeValue("lang", adaptationSet);
                    string frameRate = XmlUtils.getAttributeValue("frameRate", adaptationSet);

                    //Intialization and media segment template strings
                    string initialization = XmlUtils.getAttributeValue("initialization", segmentTemplate);
                    string media = XmlUtils.getAttributeValue("media", segmentTemplate);

                    foreach (XmlNode representationNode in adaptationNode.ChildNodes)
                    {
                        if ("Representation" == representationNode.Name)
                        {
                            //Extract track information
                            DashManifest.Track track = new Track();
                            string id = XmlUtils.getAttributeValue("id", representationNode);
                            string bandwidth = XmlUtils.getAttributeValue("bandwidth", representationNode);
                            string width = XmlUtils.getAttributeValue("width", representationNode);
                            string height = XmlUtils.getAttributeValue("height", representationNode);

                            //Set everything to track object
                            track.id = id;
                            track.type = contentType;
                            long.TryParse(bandwidth, out track.bandwidth);
                            int.TryParse(width, out track.width);
                            int.TryParse(height, out track.height);
                            float.TryParse(frameRate, out track.frameRate);
                            track.language = lang;
                            track.segmentCount = segmentCount;
                            int.TryParse(startIndex, out track.segmentStartIndex);
                            track.initSegmentTemplate = initialization;
                            track.mediaSegmentTemplate = media;

                            //Add to manifest object
                            returnManifest.tracks.Add(track);

                        }
                    }
                }
            }

            return returnManifest;
        }
    }
}
