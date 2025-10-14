using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using BrendensFuntimeApp.core;
using LearningApp1.Core;

namespace BrendensFuntimeApp.ViewModel
{
    public class CalculatorViewModel : ObservableObject
    {
        public ObservableCollection<NibbleViewModel> Nibbles
        {
            get { return _nibbles; }
            set
            {
                _nibbles = value;
                OnPropertyChanged(); 
            }
        }

        public long DecimalValue
        {
            get { return _decimalValue; }
            set
            {
                _decimalValue = value;
                OnPropertyChanged();
            }
        }

        public string HexValue
        {
            get { return _hexValue; }
            set
            {
                _hexValue = value;
                OnPropertyChanged();
            }
        }

        public CalculatorViewModel()
        {
            Nibbles = new ObservableCollection<NibbleViewModel>();

            for (byte i = 0; i < 16; i++)
            {
                NibbleViewModel nibble = new NibbleViewModel((byte)((i)*4)); // Multiply nibble number by 4 to get starting bit position

                Nibbles.Insert(0, nibble); // insert at the front so the front-most nibble is 15
                foreach (var bitViewModel in nibble.Bits)
                {
                    bitViewModel.PropertyChanged += Bit_PropertyChanged;
                }
            }
        }

        private ObservableCollection<NibbleViewModel> _nibbles;
        private long _decimalValue = 0;
        private string _hexValue = "0";

        private void Bit_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BitViewModel.BitValue))
            {
                BitViewModel bit = (BitViewModel)sender;
                UpdateCalculator(bit);
            }
        }

        private void UpdateCalculator(BitViewModel bit)
        {
            long inputValue = 1;

            // last bit is negative, so turning it on adds a negative and turing it off adds a positive
            if (bit.BitPosition == 63) 
            {
                inputValue *= -1;
            }

            // if turing off the bit
            if (bit.BitValue == false)
            {
                inputValue *= -1;
            }

            inputValue *= (long)Math.Pow(2, bit.BitPosition); // get 2^(BitPosition), multiply by inputValue to make it negative or positive

            DecimalValue += inputValue;
            HexValue = DecimalValue.ToString("X"); // this weird toString returns a number as a string formatted to read as Hex
        }
    }
}
