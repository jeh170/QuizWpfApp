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
        private string _catagory;
        private string _query;
        private ObservableCollection<string> _answers;
        private int _correctAnswer;
        private ObservableCollection<Question> _questionList;

        public NewQuestionViewModel()
        {
            _answers = new ObservableCollection<string>(new string[4]);
            _questionList = new ObservableCollection<Question>();
            AddQuestionCommand = new DelegateCommand(AddQuestion, CanAddQuestion);
            SaveFileCommand = new DelegateCommand(SaveFile, CanSaveFile);
            PropertyChanged += QuizViewModel_PropertyChanged;
        }

        public ICommand AddQuestionCommand { get; set; }

        public ICommand SaveFileCommand { get; set; }

        public ObservableCollection<Question> QuestionList
        {
            get { return _questionList; }
            set { SetProperty(ref _questionList, value); }
        }

        public string Catagory
        {
            get { return _catagory; }
            set { SetProperty(ref _catagory, value); }
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
        
        public ObservableCollection<string> Answers
        {
            get { return _answers; }
            set { SetProperty(ref _answers, value); }
        }

        private bool CanSaveFile()
        {
            return QuestionList.Any();
        }

        private void QuizViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ((DelegateCommandBase)AddQuestionCommand).RaiseCanExecuteChanged();
            ((DelegateCommandBase)SaveFileCommand).RaiseCanExecuteChanged();
        }

        private void ResetForm()
        {
            Catagory = null;
            Query = null;
            Answers = new ObservableCollection<string>(new string[4]);
        }

        private void AddQuestion()
        {
            Question q = new Question(_catagory, _query, _answers.ToList(), _correctAnswer);
            QuestionList.Add(q);
            ResetForm();
        }

        private bool CanAddQuestion()
        {
            if (_catagory == null || _query == null)
                return false;

            return _answers.All(s => s != null);
        }

        private void SaveFile()
        {
            var serializer = new XmlSerializer(typeof(QuestionSet));
            var dialog = new SaveFileDialog
            {
                Filter = "Quesiton File (*.qst)|*qst",
                DefaultExt = "qst",
                AddExtension = true
            };
            var showDialog = dialog.ShowDialog() ?? false;
            if (showDialog)
            {
                QuestionSet qs = new QuestionSet(_questionList);
                using (Stream writer = dialog.OpenFile())
                {
                    serializer.Serialize(writer, qs);
                }

                QuestionList.Clear();
            }
        }
    }
}
