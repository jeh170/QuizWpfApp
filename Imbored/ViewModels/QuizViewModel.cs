using Imbored.DataObjects;
using Prism.Mvvm;
using System;
using System.Collections;
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
        private IEnumerable<Question> _questionList;
        private IEnumerator _questionEnumerator;

        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set
            {
                SetProperty(ref _currentQuestion, value);
            }
        }


        public IEnumerable<Question> QuestionList
        {
            get { return _questionList; }
            set { _questionList = value; }
        }

        public QuizViewModel()
        {
            OpenFileCommand = new DelegateCommand(OpenFile);
            SelectAnswerCommand = new DelegateCommand<int?>(SelectAnswer);//, CanSelectAnswer);
        }

        private bool CanSelectAnswer(int? arg)
        {
            return CurrentQuestion != null;
        }

        private void SelectAnswer(int? answer)
        {
            var b = answer == CurrentQuestion.CorrectAnswer;
            MessageBox.Show(b ? "Correct" : "Incorrect");

            if (b)
            {
                _questionEnumerator.MoveNext();
                CurrentQuestion = (Question)_questionEnumerator.Current;
            }
        }

        private void OpenFile()
        {
            var deSerializer = new XmlSerializer(typeof(QuestionSet));
            var dialog = new OpenFileDialog();
            dialog.Filter = "Question Set files (*.qst) |*.qst";
            var showDialog = dialog.ShowDialog() ?? false;
            if (showDialog)
            {
                using (Stream reader = dialog.OpenFile())
                {
                    var obj = deSerializer.Deserialize(reader);
                    var qs = (obj as QuestionSet)?.Questions;

                    if (qs != null)
                    {
                        QuestionList = qs;
                        _questionEnumerator = qs.GetEnumerator();
                        CurrentQuestion = (Question)_questionEnumerator.Current;
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
