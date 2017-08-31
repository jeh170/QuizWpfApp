using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Common;
using Prism.Mvvm;

namespace Imbored.ViewModels
{
    public class NewQuestionViewModel : BindableBase
    {
        private string _query;
        private IEnumerable<ObservableObject<string>> _answers;
        private int _correctAnswer;

        public NewQuestionViewModel()
        {
            _answers = new ObservableObject<string>[4];
        }

        public string Query
        {
            get { return _query; }
            set { SetProperty(ref _query, value); }
        }

        public int CorrectAnswer
        {
            get { return _correctAnswer; }
            set { SetProperty(ref _correctAnswer, value); }
        }
        
        public IEnumerable<ObservableObject<string>> Answers
        {
            get { return _answers; }
            set { SetProperty(ref _answers, value); }
        }
    }
}
