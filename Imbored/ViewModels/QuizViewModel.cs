﻿using Imbored.DataObjects;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private int _points;

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
            SelectAnswerCommand = new DelegateCommand<int?>(SelectAnswer, CanSelectAnswer);
            PropertyChanged += QuizViewModel_PropertyChanged;
        }

        private void QuizViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ((DelegateCommandBase)SelectAnswerCommand).RaiseCanExecuteChanged();
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
                _points++;

            if (_questionEnumerator.MoveNext())
                CurrentQuestion = (Question)_questionEnumerator.Current;
            else
            {
                MessageBox.Show($"Quiz Completed! You got {_points} questions right out of {QuestionList.Count()}",
                    "Quiz Completed");
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
                        _questionEnumerator.MoveNext();
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
