using System;
using System.Collections.Generic;
using System.Text;

namespace TxtExtractWithReadAPI
{
    public class ReadAPIResult
    {
        public string status { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime lastUpdatedDateTime { get; set; }
        public Analyzeresult analyzeResult { get; set; }
    }

    public class Analyzeresult
    {
        public string version { get; set; }
        public Readresult[] readResults { get; set; }
    }

    public class Readresult
    {
        public int page { get; set; }
        public string language { get; set; }
        public float angle { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public string unit { get; set; }
        public Line[] lines { get; set; }
    }

    public class Line
    {
        public string language { get; set; }
        public float[] boundingBox { get; set; }
        public string text { get; set; }
        public Word[] words { get; set; }
    }

    public class Word
    {
        public float[] boundingBox { get; set; }
        public string text { get; set; }
        public float confidence { get; set; }
    }

}
