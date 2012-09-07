using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SkypeNet.Lib.Core
{
    public abstract class AbstractNotifyPropertyChanged : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName )
        {
            PropertyChanged.Raise(this, propertyName);
        }
        protected void OnPropertyChanged<TPropertyType>(Expression<Func<TPropertyType>> expression )
        {
            OnPropertyChanged( expression.GetPropertyName() );
        }

        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;
        protected void OnPropertyChanging(string propertyName)
        {
            PropertyChanging.Raise(this, propertyName);
        }
        protected void OnPropertyChanging<TPropertyType>(Expression<Func<TPropertyType>> expression)
        {
            OnPropertyChanging(expression.GetPropertyName());
        }

        /// <summary>
        /// Set a property and raise event notifications both <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberField"></param>
        /// <param name="value"></param>
        /// <param name="expression"></param>
        /// <returns>true if the value was changed, false if not</returns>
        protected bool SetProperty<T>(ref T memberField, T value, Expression<Func<T>> expression)
        {
            if( !ReferenceEquals(memberField, value) && 
                !EqualityComparer<T>.Default.Equals(memberField, value))
            {
                OnPropertyChanging(expression);
                memberField = value;
                OnPropertyChanged(expression);

                return true;
            }
            return false;
        }
    }
}
