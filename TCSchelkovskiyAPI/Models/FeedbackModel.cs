using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TCSchelkovskiyAPI.Models
{
    public class FeedbackModel : INotifyPropertyChanged
    {
        public FeedbackModel()
        {
           
        }

   


        private string topic;
        [Required(ErrorMessage = "Укажите тему обращения")]
        public string Topic
        {
            get
            {
                return topic;
            }
            set
            {
                topic = value;
                OnPropertyChanged("Topic");
            }
        }

        private string name;
        [Required(ErrorMessage = "Укажите ваше имя")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");

            }
        }
        private string emailOrPhone;
        [Required(ErrorMessage = "Укажите ваш email или телефон")]
        public string EmailOrPhone
        {
            get
            {
                return emailOrPhone;
            }
            set
            {
                emailOrPhone = value;
                OnPropertyChanged("EmailOrPhone");
            }
        }
        private string text;
        [Required(ErrorMessage = "Напишите ваше обращения")]
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }
        private bool agreementState;
        [Required(ErrorMessage = "Необходимо ваше согласие на обработку персональных данных")]
        public bool AgreementState
        {
            get
            {
                return agreementState;
            }
            set
            {
                agreementState = value;
                OnPropertyChanged("AgreementState");
            }
        }

        private List<string> validationErrors;
        public List<string> ValidationErrors
        {
            get
            {
                return validationErrors;
            }
            set
            {
                validationErrors = value;
                OnPropertyChanged("ValidationErrors");
            }
        }

        private string firstValidationError;
        public string FirstValidationError
        {
            get
            {
                return firstValidationError;
            }
            set
            {
                firstValidationError = value;
                OnPropertyChanged("FirstValidationError");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
