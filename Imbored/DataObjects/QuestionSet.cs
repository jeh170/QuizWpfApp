using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Prism.Mvvm;
using System.Xml.Serialization;

namespace Imbored.DataObjects
{
    public class QuestionSet : BindableBase
    {
        private List<Question> _questions;

        public QuestionSet()
        {
            _questions = new List<Question>();
        }

        public QuestionSet(IEnumerable<Question> questions)
        {
            _questions = questions.ToList();
        }

        [XmlElement("Question")]
        public List<Question> Questions
        {
            get { return _questions; }
            set { SetProperty(ref _questions, value); }
        }
    }
}
