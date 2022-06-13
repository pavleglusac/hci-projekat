using HCIProjekat.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HCIProjekat.views.manager.pages
{
    /// <summary>
    /// Interaction logic for NameChanger.xaml
    /// </summary>
    public partial class NameChanger : Window
    {

        public Train Train { get; set; }
        public string Name { get; set; }
        public NameChanger()
        {
            InitializeComponent();
        }

        public NameChanger(Train train)
        {
            DataContext = this;
            InitializeComponent();
            Train = train;
            Name = train.Name;
            NameInput.Text = Name;
        }

        public void SaveButtonClicked(object sender, EventArgs e)
        {
            bool valid = !model.Database.Trains.Any(x => x.Name == NameInput.Text.Trim() && x != Train);
            if(!valid)
            {
                BindingExpression bindingExpression = BindingOperations.GetBindingExpression(NameInput, TextBox.TextProperty);

                BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(NameInput, TextBox.TextProperty);

                ValidationError validationError = new ValidationError(new ExceptionValidationRule(), bindingExpression);
                validationError.ErrorContent = "Već postoji voz sa tim imenom!";

                Validation.MarkInvalid(bindingExpressionBase, validationError);
                return;
            }

            if (!Validation.GetHasError(NameInput) && valid)
            {
                Train.Name = NameInput.Text.Trim();
                this.Close();
            }

        }

        private void NameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valid = !model.Database.Trains.Any(x => x.Name == NameInput.Text.Trim() && x != Train);
            if (!valid)
            {
                BindingExpression bindingExpression = BindingOperations.GetBindingExpression(NameInput, TextBox.TextProperty);

                BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(NameInput, TextBox.TextProperty);

                ValidationError validationError = new ValidationError(new ExceptionValidationRule(), bindingExpression);
                validationError.ErrorContent = "Već postoji voz sa tim imenom!";

                Validation.MarkInvalid(bindingExpressionBase, validationError);
                return;
            }

            SaveButton.IsEnabled = !Validation.GetHasError(NameInput) && valid;
        }
    }
}
