using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для Feedback.xaml
    /// </summary>
    public partial class Feedback : Page
    {
        public Feedback()
        {
            FeedbackModel = new FeedbackModel();
            InitializeComponent();
        }

        TextBox selectedTextBox;
        private void buttonPressed(object sender, EventArgs e)
        {
            if (selectedTextBox != null)
            {
                TCSChelkovskiy.Controls.KeyboardEventArgs args = e as TCSChelkovskiy.Controls.KeyboardEventArgs;
                switch (args.CurrentKey)
                {
                    case "Lang":
                    
                        break;
                    case "Backspace":                    
                        if (selectedTextBox.Text.Length > 0)
                        {
                            selectedTextBox.Text = selectedTextBox.Text.Remove(selectedTextBox.Text.Length - 1);
                        }
                        break;
                    case "Shift":
                     
                        break;
                    case "123":

                        break;
                    case "АБВ":

                        break;
                    default:
                        selectedTextBox.Text += args.CurrentKey;
                        break;
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            selectedTextBox = sender as TextBox;
        }

        #region Обратная связь

        public static readonly DependencyProperty FeedbackModelProperty = DependencyProperty.Register(
     "FeedbackModel", typeof(FeedbackModel), typeof(Feedback), new PropertyMetadata(default(FeedbackModel)));
        //Не рефакторить, из-за browseBack не подгружаются картинки
        private ICommand goBack;
        public ICommand GoBack => goBack ??= new RelayCommand(f =>
        {
            NavigationService?.Navigate(new AboutTradeCenter(Memory.KioskObjects.AboutMall));
        });
        public FeedbackModel FeedbackModel
        {
            get => (FeedbackModel)GetValue(FeedbackModelProperty);
            set => SetValue(FeedbackModelProperty, value);
        }
        private RelayCommand sendFeedback;
        public RelayCommand SendFeedback
        {
            get
            {
                return sendFeedback ??
                    (sendFeedback = new RelayCommand(obj =>
                    {
                        FeedbackModel.ValidationErrors = new List<string>();
                        var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                        var context = new ValidationContext(FeedbackModel);
                        if (!Validator.TryValidateObject(FeedbackModel, context, results, true))
                        {
                            foreach (var error in results)
                            {
                                FeedbackModel.ValidationErrors.Add(error.ErrorMessage);
                            }
                            FeedbackModel.FirstValidationError = FeedbackModel.ValidationErrors.FirstOrDefault();
                        }
                        else
                        {
                            FeedbackModel.FirstValidationError = string.Empty;
                        
                            TCSchelkovskiyAPI.TCSchelkovskiyAPI.SendFeedback(FeedbackModel);
                            new CustomMessageBox("Уведомление", "Ваш отзыв был успешно нами получен").ShowDialog();
                        }
                    }));
            }
        }

        #endregion

    }
}
