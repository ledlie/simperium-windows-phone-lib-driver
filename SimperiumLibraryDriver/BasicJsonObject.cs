using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SimperiumLibraryDriver
{
    [DataContractAttribute]
    public class BasicJsonObject
    {

        [DataMember]
        public string key1 { get; set; }

        [DataMember]
        public string key2 { get; set; }

        [DataMember]
        public string key3 { get; set; }

        [DataMember]
        public string key4 { get; set; }

        public BasicJsonObject()
        {
        }


    }
}
