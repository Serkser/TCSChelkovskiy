using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TCSChelkovskiy.Models
{
    public class FeedbackModel : INotifyPropertyChanged
    {
        enum FieldType 
        { 
          Topic,
          Name,
          EmailOrPhone,
          Text
        }
        public FeedbackModel()
        {
            MakeDefaultValues();

        }

        public void MakeDefaultValues()
        {
            Topic = "Тема";
            Name = "Имя";
            EmailOrPhone = "E-mail / телефон";
            Text = "Введите текст";
            AgreementState = false;
        }
        void CheckField(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.Topic:
                    if (Topic.StartsWith("Тема"))
                    {
                        Topic = string.Empty;
                    }
                    break;
                case FieldType.Name:
                    if (Name.StartsWith("Имя"))
                    {
                        Name = string.Empty;
                    }
                    break;
                case FieldType.EmailOrPhone:
                    if (EmailOrPhone.StartsWith("E-mail / телефон"))
                    {
                        EmailOrPhone = string.Empty;
                    }
                    break;
                case FieldType.Text:
                    if (Text.StartsWith("Введите текст"))
                    {
                        Text = string.Empty;
                    }
                    break;
            }

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
                CheckField(FieldType.Topic);
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
                CheckField(FieldType.Name);
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
                CheckField(FieldType.EmailOrPhone);
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
                CheckField(FieldType.Text);
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
