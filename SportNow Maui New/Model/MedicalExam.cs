using System;
namespace SportNow.Model
{


    public class MedicalExam
    {
        public string id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string expireDate { get; set; }
        public string filename { get; set; }
        public string mimeType { get; set; }
        
        public override string ToString()
        {
            return name;
        }
    }
}
