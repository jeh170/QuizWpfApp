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
        private IList<ObservableObject<string>> _answers;
        private string _query;

        private int _correctAnswer;

        public int CorrectAnswer
        {
            get { return _correctAnswer; }
            set { SetProperty(ref _correctAnswer, value); }
        }
        
        public string Query
        {
            get { return _query; }
            set { SetProperty(ref _query, value); }
        } 

        public IList<ObservableObject<string>> Answers
        {
            get { return _answers; }
            set {SetProperty(ref _answers, value); }
        }
    }
}
