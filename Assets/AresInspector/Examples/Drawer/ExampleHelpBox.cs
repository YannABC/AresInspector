using Ares;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleHelpBox : ScriptableObject, IAresObjectV
    {
        public bool on = true;

        public bool On => on;

        public bool GetOn() { return on; }

        [ADHelpBox(ADHelpBox.MessageType.Info, "This is info.", null)]
        public int info;

        [ADHelpBox(ADHelpBox.MessageType.Warning, "This is warning.", null)]
        [ACLabelColor(1, 1, 0)]
        public int warn;

        [ADHelpBox(ADHelpBox.MessageType.Error, "This is error.", null)]
        [ACLabelColor(1, 0, 0)]
        public int error;

        [ADHelpBox(ADHelpBox.MessageType.None, "no icon.", null)]
        public int no_icon;

        [ADHelpBox(ADHelpBox.MessageType.Info, "This is info.", "on")]
        public int info1;

        [ADHelpBox(ADHelpBox.MessageType.Warning, "This is warning.", "On")]
        public int warn1;

        [ADHelpBox(ADHelpBox.MessageType.Error, "This is error.This is error.This is error.This is error.This is error.This is error.This is error.This is error.This is error.This is error.This is error.This is error.", "GetOn")]
        public int error1;
    }
}
