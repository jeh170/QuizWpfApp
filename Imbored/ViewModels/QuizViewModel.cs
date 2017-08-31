using Imbored.DataObjects;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using Microsoft.Win32;
using Prism.Commands;

namespace Imbored.ViewModels
{
    public class QuizViewModel : BindableBase
    {
        private Question _currentQuestion;

        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set
            {
                SetProperty(ref _currentQuestion, value);
            }
        }

        private IEnumerable<Question> _questionList;

        public IEnumerable<Question> QuestionList
        {
            get { return _questionList; }
            set { _questionList = value; }
        }

        public QuizViewModel()
        {
            OpenFileCommand = new DelegateCommand(OpenFile);
            SelectAnswerCommand = new DelegateCommand<int?>(SelectAnswer);
        }

        private void SelectAnswer(int? answer)
        {
            MessageBox.Show(answer == CurrentQuestion.CorrectAnswer ? "Correct" : "Incorrect");
        }

        private void OpenFile()
        {
            var deSerializer = new XmlSerializer(typeof(Question));
            var dialog = new OpenFileDialog();
            var showDialog = dialog.ShowDialog() ?? false;
            if (showDialog)
            {
                using (Stream reader = dialog.OpenFile())
                {
                    object obj = deSerializer.Deserialize(reader);
                    Question q = obj as Question;

                    if (q != null)
                    {
                        CurrentQuestion = q;
                    }
                    else
                    {
                        MessageBox.Show("The file is corrupt!", "Corrupt File", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        public ICommand OpenFileCommand { get; set; }

        public ICommand SelectAnswerCommand { get; set; }
    }
}
