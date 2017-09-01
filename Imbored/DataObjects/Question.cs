using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Prism.Mvvm;

namespace Imbored.DataObjects
{
    public class Question : BindableBase
    {
        private string _query;
        private List<string> _answers;
        private int _correctAnswer;

        public Question()
        {
            
        }

        public Question(string query, List<string> answers, int correctAnswer)
        {
            _query = query;
            _answers = answers;
            _correctAnswer = correctAnswer;
        }

        

        [XmlElement("Query")]
        public string Query
        {
            get { return _query; }
            set { SetProperty(ref _query, value); }
        }
        
        [XmlElement("Answer")]
        public List<string> Answers
        {
            get { return _answers; }
            set { SetProperty(ref _answers, value); }
        }
        
        [XmlElement("CorrectAnswer")]
        public int CorrectAnswer
        {
            get { return _correctAnswer; }
            set { SetProperty(ref _correctAnswer, value); }
        }
    }
}
