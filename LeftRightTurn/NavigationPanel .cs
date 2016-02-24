using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LeftRightTurn
{
    public class NavigationPanel : StackPanel
    {
        public event EventHandler AnimationComplte;
        private static double COUNT_OF_PAGE;
        private TranslateTransform NavigationPanelTranslateTransform;


        private static readonly TimeSpan DURATION = TimeSpan.FromMilliseconds(250);

        public NavigationPanel()
            : base()
        {
            this.Orientation = Orientation.Horizontal;


            NavigationPanelTranslateTransform = new TranslateTransform();

            this.Loaded += new RoutedEventHandler(NavigationPanel_Loaded);
        }

        void NavigationPanel_Loaded(object sender, RoutedEventArgs e)
        {
            COUNT_OF_PAGE = this.Children.Count;
            CurrentIndex = 0;
            foreach (FrameworkElement fe in this.Children)
            {
                fe.RenderTransform = NavigationPanelTranslateTransform;
            }
        }

        public void Next()
        {
            AnimationChildren(true);
        }

        public void Previous()
        {
            AnimationChildren(false);
        }


        private bool ValidateNext()
        {
            return CurrentIndex < (COUNT_OF_PAGE - 1) && CurrentIndex >= 0;
        }


        private bool ValidatePrevious()
        {
            return CurrentIndex <= (COUNT_OF_PAGE - 1) && CurrentIndex > 0;
        }

        private bool ValidateCurrentIndex()
        {
            if (CurrentIndex > -1 && CurrentIndex < this.Children.Count)
            {
                return true;
            }

            return false;
        }

        private void AnimationChildren(bool forward)
        {
            double currentTranX = NavigationPanelTranslateTransform.X;
            double nextTranX = currentTranX;
            if (forward)
            {
                if (ValidateCurrentIndex())
                {
                    nextTranX -= (this.Children[CurrentIndex] as FrameworkElement).ActualWidth;
                }

            }
            else
            {
                if (ValidateCurrentIndex())
                {
                    nextTranX += (this.Children[CurrentIndex] as FrameworkElement).ActualWidth;
                }

            }

            DoubleAnimation da = new DoubleAnimation(currentTranX, nextTranX, DURATION);
            da.Completed += (o1, e1) =>
            {
                CurrentIndex += forward ? 1 : -1;
                if (AnimationComplte != null)
                {
                    AnimationComplte(this, e1);
                }
            };

            NavigationPanelTranslateTransform.BeginAnimation(TranslateTransform.XProperty, da);
        }

        #region CurrentIndex

        /// <summary>  
        /// The <see cref="CurrentIndex" /> dependency property's name.  
        /// </summary>  
        public const string CurrentIndexPropertyName = "CurrentIndex";

        /// <summary>  
        /// Gets or sets the value of the <see cref="CurrentIndex" />  
        /// property. This is a dependency property.  
        /// </summary>  
        public int CurrentIndex
        {
            get
            {
                return (int)GetValue(CurrentIndexProperty);
            }
            set
            {
                SetValue(CurrentIndexProperty, value);
            }
        }

        /// <summary>  
        /// Identifies the <see cref="CurrentIndex" /> dependency property.  
        /// </summary>  
        public static readonly DependencyProperty CurrentIndexProperty = DependencyProperty.Register(
            CurrentIndexPropertyName,
            typeof(int),
            typeof(NavigationPanel),
            new UIPropertyMetadata(-1, OnCurrentIndexChanged));

        private static void OnCurrentIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NavigationPanel).OnCurrentIndexChanged((int)e.OldValue, (int)e.NewValue);
        }

        protected virtual void OnCurrentIndexChanged(int oldIndex, int newIndex)
        {
            NextIsValid = ValidateNext();
            PreviousIsValid = ValidatePrevious();
        }

        #endregion // CurrentIndex


        #region NextIsValid

        /// <summary>  
        /// The <see cref="NextIsValid" /> dependency property's name.  
        /// </summary>  
        public const string NextIsValidPropertyName = "NextIsValid";

        /// <summary>  
        /// Gets or sets the value of the <see cref="NextIsValid" />  
        /// property. This is a dependency property.  
        /// </summary>  
        public bool NextIsValid
        {
            get
            {
                return (bool)GetValue(NextIsValidProperty);
            }
            set
            {
                SetValue(NextIsValidProperty, value);
            }
        }

        /// <summary>  
        /// Identifies the <see cref="NextIsValid" /> dependency property.  
        /// </summary>  
        public static readonly DependencyProperty NextIsValidProperty = DependencyProperty.Register(
            NextIsValidPropertyName,
            typeof(bool),
            typeof(NavigationPanel),
            new UIPropertyMetadata(false));

        #endregion // NextIsValid

        #region PreviousIsValid

        /// <summary>  
        /// The <see cref="PreviousIsValid" /> dependency property's name.  
        /// </summary>  
        public const string PreviousIsValidPropertyName = "PreviousIsValid";

        /// <summary>  
        /// Gets or sets the value of the <see cref="PreviousIsValid" />  
        /// property. This is a dependency property.  
        /// </summary>  
        public bool PreviousIsValid
        {
            get
            {
                return (bool)GetValue(PreviousIsValidProperty);
            }
            set
            {
                SetValue(PreviousIsValidProperty, value);
            }
        }

        /// <summary>  
        /// Identifies the <see cref="PreviousIsValid" /> dependency property.  
        /// </summary>  
        public static readonly DependencyProperty PreviousIsValidProperty = DependencyProperty.Register(
            PreviousIsValidPropertyName,
            typeof(bool),
            typeof(NavigationPanel),
            new UIPropertyMetadata(false));

        #endregion // PreviousIsValid




    }  
}
