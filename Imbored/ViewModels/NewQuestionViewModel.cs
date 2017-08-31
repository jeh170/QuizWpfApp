using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using Imbored.DataObjects;
using Microsoft.Win32;
using Prism.Commands;
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
            SaveQuestionCommand = new DelegateCommand(SaveQuestion);
        }

        private void SaveQuestion()
        {
            var serializer = new XmlSerializer(typeof(Question));
            var dialog = new SaveFileDialog();
            dialog.Filter = "Quesiton File (*.qst)|*qst";
            var showDialog = dialog.ShowDialog() ?? false;
            if (showDialog)
            {
                Question q = new Question(_query, (IEnumerable<string>)Unobserve((IEnumerable<ObservableObject<object>>)_answers), _correctAnswer);
                using (Stream writer = dialog.OpenFile())
                {
                    serializer.Serialize(writer, q);
                }
            }
        }

        private IEnumerable<object> Unobserve(IEnumerable<ObservableObject<object>> enumerable)
        {
            return enumerable.Select(o => o.Value);
        }

        public ICommand SaveQuestionCommand { get; set; }

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
