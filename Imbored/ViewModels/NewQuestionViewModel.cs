using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<string> _answers;
        private int _correctAnswer;

        public NewQuestionViewModel()
        {
            _answers = new ObservableCollection<string>(new string[4]);
            SaveQuestionCommand = new DelegateCommand(SaveQuestion);
        }

        private void SaveQuestion()
        {
            var serializer = new XmlSerializer(typeof(Question));
            var dialog = new SaveFileDialog
            {
                Filter = "Quesiton File (*.qst)|*qst",
                DefaultExt = "qst",
                AddExtension = true
            };
            var showDialog = dialog.ShowDialog() ?? false;
            if (showDialog)
            {
                Question q = new Question(_query, _answers.ToList(), _correctAnswer);
                using (Stream writer = dialog.OpenFile())
                {
                    serializer.Serialize(writer, q);
                }
            }
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
        
        public ObservableCollection<string> Answers
        {
            get { return _answers; }
            set { SetProperty(ref _answers, value); }
        }
    }
}
