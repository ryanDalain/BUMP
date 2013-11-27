using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BeefUpMyPassword
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int questionNum = 0;
        int charLimimitMin = 6;
        List<String> questions;
        public MainPage()
        {
            this.InitializeComponent();
            questions = new List<String>();
            generatePassButton.Click += new RoutedEventHandler(generatePassButton_Click);
            askAgainButton.Click += new RoutedEventHandler(askAgainButton_Click);
            toggle8chars.Checked += new RoutedEventHandler(toggle8chars_Checked);
            toggle8chars.Unchecked += new RoutedEventHandler(toggle8chars_Unchecked);
            questions.Add("What is a funny name for a pet?");
            questions.Add("Where do you want to go for your next vacation?");
            questions.Add("What food do you crave right now?");
            questions.Add("What is the last thing you drank?");
            questions.Add("Can you describe yourself in one word?");
            questions.Add("What is your favorite symmetrical thing?");
            questions.Add("What are you afraid of?");
            questions.Add("Who is your hero?");
            questions.Add("What is one place you hard to avoid?");
            questionTB.Text = questions[0];
        }

        private void generatePassButton_Click(object sender, RoutedEventArgs e)
        {
            string startingPoint = field1.Text;
            
            if (string.IsNullOrEmpty(field1.Text))
                passwordTB.Text = "Fill out the first question at least.";
            else
            {

                // first check if field1 has any requirements
                Boolean hasNumber = false;
                Boolean hasCapital = false;
                Boolean hasSymbol = false;
                Boolean hasMinChars = (startingPoint.Length >= charLimimitMin);

                for(int i = 0; i < startingPoint.Length; i++)
                {
                    if(Char.IsDigit(startingPoint[i]))
                        hasNumber = true;
                    if (Char.IsUpper(startingPoint[i]))
                        hasCapital = true;
                    if (Char.IsSymbol(startingPoint[i]) || Char.IsPunctuation(startingPoint[i]))
                        hasSymbol = true;
                }

                // if field1 has a valid password
                if(hasNumber && hasCapital && hasSymbol && hasMinChars)
                    passwordTB.Text = "Your password already meets all requirements!";
                // else beef up the password
                else
                {

                    int numMissing = charLimimitMin - (startingPoint.Length);

                    // if the base word has no capital letters
                    if (!hasCapital)
                    {
                        // first check does the base even have any letters
                        // if there are some letters, change the first occurence to capital
                        Boolean hasLetters = false;
                        for (int i = 0; i < startingPoint.Length; i++)
                            if (Char.IsLetter(startingPoint[i]))
                            {

                                System.Text.StringBuilder endingPoint = new System.Text.StringBuilder(startingPoint);
                                endingPoint[i] = Char.ToUpper(startingPoint[i]);
                                startingPoint = endingPoint.ToString();
                                hasLetters = true;
                                break;
                            }

                        // if no letters are in the base
                        if(!hasLetters)
                        {
                            // then add one randomly
                            char[] symbolList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                            Random index = new Random();
                            int i = index.Next(symbolList.Length);
                            startingPoint += symbolList[i];

                            // we may have met our min char limit by adding the 1 character
                            numMissing--;
                            if (startingPoint.Length >= charLimimitMin)
                                hasMinChars = true;
                        }
                    }

                    // after this we may end up adding characters so detract from numMissing accordingly
                    if (!hasSymbol)
                        numMissing--;

                    if(!hasMinChars)
                    {
                        for (int i = 0; i < numMissing; i++)
                            startingPoint += i+1;

                        hasMinChars = true;
                        hasNumber = true; 
                    }
                            
                    if(!hasNumber)
                    {
                        var r = new Random();
                        startingPoint += r.Next(1, 10);
                        hasNumber = true;
                    }

                    if(!hasSymbol)
                    {
                        char[] symbolList = "~`!@#$%^&*()_-+=".ToCharArray();
                        Random index = new Random();
                        int i = index.Next(symbolList.Length);
                        startingPoint += symbolList[i];
                        hasSymbol = true;
                    }
                    passwordTB.Text = startingPoint;
                }
            }

        }

        private void askAgainButton_Click(object sender, RoutedEventArgs e)
        {
            questionNum += 1;
            if(questionNum >= questions.Count)
                questionNum = 0;
            questionTB.Text = questions[questionNum];
        }

        private void toggle8chars_Checked(object sender, RoutedEventArgs e)
        {
            charLimimitMin = 8;
        }
        private void toggle8chars_Unchecked(object sender, RoutedEventArgs e)
        {
            charLimimitMin = 6;
        }
    }
}
